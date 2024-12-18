using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
//Todo imbeded message metadata (8bytes) 4 bytes length 4 bytes checksum (md4) md4 length is only 2 bytes so i wanna use 6bytes for the length and 2 for the md5 checksum

namespace BmpSteganography
{ 
    public class SteganographyService
    {
        public int CalculateMaxMessageSize(string bmpFilePath)
        {
            using (var mmf = MemoryMappedFile.CreateFromFile(bmpFilePath, FileMode.Open))
            {
                using (var accessor = mmf.CreateViewAccessor())
                {
                    int headerSize = accessor.ReadInt32(10); // Header size from offset 10
                    int width = accessor.ReadInt32(18);      // Read Image width from offset 18
                    int height = accessor.ReadInt32(22);     // Read Image height from offset 22
                    short bitsPerPixel = accessor.ReadInt16(28); // Reads Bits per pixel from offset 28

                    int totalPixels = width * height;

                    // wenn z.b. 24 (RGB) = 3 wenn 32 (RGBA) 4 bits pro pixel   
                    int availableBytes = (totalPixels * bitsPerPixel / 8);

                    // Subtract space for length prefix and checksum
                    return Math.Max(0, availableBytes - 8);
                }
            }
        }

        /// <summary>
        /// Embeds a message into a BMP file
        /// </summary>
        /// <param name="inputBmpPath">Path to input BMP file</param>
        /// <param name="outputBmpPath">Path to output BMP file</param>
        /// <param name="message">Message to embed</param>
        public void EmbedMessage(string inputBmpPath, string outputBmpPath, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            int maxMessageSize = CalculateMaxMessageSize(inputBmpPath);

            if (messageBytes.Length > maxMessageSize)
            {
                throw new InvalidOperationException($"Message too large. Max size: {maxMessageSize} bytes");
            }

            // Prepare message with length prefix and checksum
            byte[] fullMessage = PrepareMessage(messageBytes);

            using (var inputMmf = MemoryMappedFile.CreateFromFile(inputBmpPath, FileMode.Open))
            using (var outputMmf = MemoryMappedFile.CreateFromFile(outputBmpPath, FileMode.Create, null, new FileInfo(inputBmpPath).Length))
            {
                using (var inputAccessor = inputMmf.CreateViewAccessor())
                using (var outputAccessor = outputMmf.CreateViewAccessor())
                {
                    // Copy header first
                    int headerSize = inputAccessor.ReadInt32(10);
                    for (int i = 0; i < headerSize; i++)
                    {
                        byte headerByte = inputAccessor.ReadByte(i);
                        outputAccessor.Write(i, headerByte);
                    }

                    // Embed message using LSB
                    int messageIndex = 0;
                    int bitIndex = 0;

                    for (int i = headerSize; i < inputAccessor.Capacity && messageIndex < fullMessage.Length; i++)
                    {
                        byte pixelByte = inputAccessor.ReadByte(i);
                        byte messageBit = (byte)((fullMessage[messageIndex] >> (7 - bitIndex)) & 1);

                        // Modify least significant bit
                        pixelByte = (byte)((pixelByte & 0xFE) | messageBit);
                        outputAccessor.Write(i, pixelByte);

                        bitIndex++;
                        if (bitIndex == 8)
                        {
                            bitIndex = 0;
                            messageIndex++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracts a message from a BMP file
        /// </summary>
        /// <param name="bmpFilePath">Path to BMP file with embedded message</param>
        /// <returns>Extracted message</returns>
        public string ExtractMessage(string bmpFilePath)
        {
            using (var mmf = MemoryMappedFile.CreateFromFile(bmpFilePath, FileMode.Open))
            {
                using (var accessor = mmf.CreateViewAccessor())
                {
                    int headerSize = accessor.ReadInt32(10);

                    // Extract length prefix (first 4 bytes)
                    byte[] lengthPrefix = new byte[4];
                    for (int i = 0; i < 32; i++)
                    {
                        byte pixelByte = accessor.ReadByte(headerSize + i);
                        int bit = pixelByte & 1;
                        lengthPrefix[i / 8] |= (byte)(bit << (7 - (i % 8)));
                    }
                    int messageLength = BitConverter.ToInt32(lengthPrefix, 0);

                    // Extract checksum (next 4 bytes)
                    byte[] storedChecksum = new byte[4];
                    for (int i = 0; i < 32; i++)
                    {
                        byte pixelByte = accessor.ReadByte(headerSize + 32 + i);
                        int bit = pixelByte & 1;
                        storedChecksum[i / 8] |= (byte)(bit << (7 - (i % 8)));
                    }

                    // Extract message
                    byte[] messageBytes = new byte[messageLength];
                    for (int i = 0; i < messageLength * 8; i++)
                    {
                        byte pixelByte = accessor.ReadByte(headerSize + 64 + i);
                        int bit = pixelByte & 1;
                        messageBytes[i / 8] |= (byte)(bit << (7 - (i % 8)));
                    }

                    // Verify checksum
                    byte[] calculatedChecksum = ComputeChecksum(messageBytes);
                    if (!StructuralComparisons.StructuralEqualityComparer.Equals(storedChecksum, calculatedChecksum))
                    {
                        throw new InvalidOperationException("Message integrity check failed");
                    }

                    return Encoding.UTF8.GetString(messageBytes);
                }
            }
        }

        /// <summary>
        /// Prepares message by adding length prefix and checksum
        /// </summary>
        private byte[] PrepareMessage(byte[] messageBytes)
        {
            // Length prefix (4 bytes)
            byte[] lengthPrefix = BitConverter.GetBytes(messageBytes.Length);

            // Checksum (4 bytes)
            byte[] checksum = ComputeChecksum(messageBytes);

            // Combine all parts
            byte[] fullMessage = new byte[lengthPrefix.Length + checksum.Length + messageBytes.Length];
            Buffer.BlockCopy(lengthPrefix, 0, fullMessage, 0, lengthPrefix.Length);
            Buffer.BlockCopy(checksum, 0, fullMessage, lengthPrefix.Length, checksum.Length);
            Buffer.BlockCopy(messageBytes, 0, fullMessage, lengthPrefix.Length + checksum.Length, messageBytes.Length);

            return fullMessage;
        }

        /// <summary>
        /// Computes a simple 4-byte checksum for message integrity
        /// </summary>
        private byte[] ComputeChecksum(byte[] messageBytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(messageBytes);
                return hash.Take(4).ToArray(); // First 4 bytes of MD5 hash
            }
        }
    }

    /// <summary>
    /// Service for file operations related to steganography
    /// </summary>
    public class FileService
    {
        /// <summary>
        /// Validates if the file is a valid BMP
        /// </summary>
        public bool ValidateBmpFile(string filePath)
        {
            try
            {
                using (var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open))
                {
                    using (var accessor = mmf.CreateViewAccessor())
                    {
                        // Check BMP signature (first two bytes should be 'BM')
                        if (accessor.ReadByte(0) != 'B' || accessor.ReadByte(1) != 'M')
                        {
                            return false;
                        }

                        // Validate header size and file size
                        int headerSize = accessor.ReadInt32(10);
                        long fileSize = new FileInfo(filePath).Length;

                        return headerSize > 0 && fileSize > headerSize;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Combines a text file with a BMP file
        /// </summary>
        public void CombineBmpWithText(string bmpFilePath, string textFilePath, string outputBmpPath)
        {
            string textContent = File.ReadAllText(textFilePath);

            var steganographyService = new SteganographyService();
            steganographyService.EmbedMessage(bmpFilePath, outputBmpPath, textContent);
        }
    }
}