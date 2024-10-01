using System;
using System.Diagnostics;
using System.Linq;
using XXHashCs;

namespace Hvld.Parser
{
    /// <summary>
    /// Class representing an HVLD 2.0 frame.
    /// </summary>
    public sealed class HvldFrame
    {
        /// <summary>
        /// Frame header size.
        /// </summary>
        private const int HEADER_SIZE = 8;
        /// <summary>
        /// *WARNING*: the network file dump from GRETEL starts with 
        /// a 4 bytes integer with the payload length: they are not part of the frame.
        /// </summary>
        private const int BYTES_TO_BE_SKIPPED = 0;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public HvldFrame(byte[] frame)
        {
            // Used for diagnostic purposes.
            var sw = Stopwatch.StartNew();

            try
            {
                // Skips the first BYTES_TO_BE_SKIPPED bytes.
                frame = frame.SubArray(BYTES_TO_BE_SKIPPED, frame.Length - BYTES_TO_BE_SKIPPED);
                // Null-check + length check on the frame.
                if (frame is null || frame.Length < HEADER_SIZE)
                    throw new HvldFrameException("Frame::ctor: null frame buffer reference or wrong frame size.");
               
                Version = frame[0];
                Id = frame[1];
                NumberOfSignals = frame[2];
                PayloadType = frame[3];
                PayloadSize = BitConverter.ToInt32(frame, 4);

                if (PayloadSize <= 0)
                    throw new HvldFrameException("Frame::ctor: invalid payload size.");
                // Gets the data block for the payload.
                PayloadData = frame.SubArray(HEADER_SIZE, PayloadSize);
                // Creates a new payload object.
                Payload = new HvldPayload(PayloadData, NumberOfSignals);
                // Converts the CRC to hexadecimal string.
                var fileCrc = BitConverter.ToString(frame.SubArray(frame.Length - HEADER_SIZE, frame.Length - (frame.Length - HEADER_SIZE)));
                CRC = fileCrc.Replace("-", string.Empty).ToUpper();
                // Saves the frame CRC buffer.
                FrameWithoutCRC = frame.SubArray(0, frame.Length - HEADER_SIZE).ToArray();
            }
            catch (Exception ex)
            {
                throw new HvldFrameException("Frame::ctor: exception raised during the frame parsing.", ex);
            }
            // Saving the processing time for speed investigation.
            ProcessingTimeMs = sw.ElapsedMilliseconds;
        }
        /// <summary>
        /// 
        /// </summary>
        public long ProcessingTimeMs { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte Version { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte Id { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte NumberOfSignals { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte PayloadType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int PayloadSize { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] PayloadData { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public HvldPayload Payload { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string CRC { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] FrameWithoutCRC { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateCRC()
        {
            try
            {
                var hash = XXHash.GetHashXXH3Bytes64(FrameWithoutCRC).ToUpper();
                return hash == CRC;
            }
            catch (DllNotFoundException dnfex)
            {
                throw new HvldFrameException("Frame::ValidateCRC: XXHash*.dll missing.", dnfex);
            }
            catch (Exception ex)
            {
                throw new HvldFrameException("Frame::ValidateCRC: exception raised during CRC validation.", ex);
            }
        }
    }
}
