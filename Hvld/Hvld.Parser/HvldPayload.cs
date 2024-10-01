using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hvld.Parser
{
    /// <summary>
    /// Class representing an HVLD 2.0 payload.
    /// </summary>
    public sealed class HvldPayload
    {
        /// <summary>
        /// Private strucure used to store the offset/data size for each signal in the GRETEL frame.
        /// </summary>
        private struct SignalBufferInfo
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="readBufferSize"></param>
            /// <param name="dataOffset"></param>
            public SignalBufferInfo(int readBufferSize, int dataOffset, byte[] signalBuffer)
            {
                SignalReadBufferSize = readBufferSize;
                SignalDataOffset = dataOffset;
                SignalBuffer = signalBuffer;
            }
            /// <summary>
            /// 
            /// </summary>
            public readonly int SignalReadBufferSize;
            /// <summary>
            /// 
            /// </summary>
            public readonly int SignalDataOffset;
            /// <summary>
            /// 
            /// </summary>
            public readonly byte[] SignalBuffer;
        }
        /// <summary>
        /// Payload data buffer.
        /// </summary>
        private readonly byte[] _payload;
        /// <summary>
        /// Number of signals within the payload.
        /// </summary>
        private int _numberOfSignals;
        /// <summary>
        /// Dictionary containing all the signals data read from the GRETEL frame.
        /// </summary>
        private ConcurrentDictionary<byte, SignalInfo> _signals { get; set; } = new ConcurrentDictionary<byte, SignalInfo>();
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<byte, SignalBufferInfo> _mapSignalBufferInfo;
        /// <summary>
        /// 
        /// </summary>

        public bool GetSignal(byte index, out SignalInfo signalInfo)
        {
            signalInfo = null;
            // Mandatory condition is: the key must exist in the RAW mapSignalBufferInfo.
            // If it's not there, it can't be in _signals.
            if (!_mapSignalBufferInfo.ContainsKey(index))
                return false;
            // Check if the signal has already been costructed.
            if (_signals.Count.Equals(0) || !_signals.ContainsKey(index))
            {
                // Signal is not constructed yet.
                var sigBuffer = _mapSignalBufferInfo[index].SignalBuffer;
                // Null/length check on the SignalInfo block.
                if (sigBuffer is null || sigBuffer.Length != SignalInfo.SIGNAL_INFO_BLOCK_SIZE)
                    throw new HvldPayloadException($"Payload::GetSignal: invalid SignalInfo block at index {index}.");
                // Adds the newly created SignalInfo to the signals list.
                if (!_signals.TryAdd(index, new SignalInfo(
                    sigBuffer,
                    _payload,
                    _numberOfSignals,
                    _mapSignalBufferInfo[index].SignalDataOffset,
                    _mapSignalBufferInfo[index].SignalReadBufferSize,
                    index))) // ==> note: signal ID is overridden with its index! Fix to avoid GRETEL's bug sending the same signals more than once.
                {
                    throw new HvldPayloadException($"Payload::GetSignal: adding signal at index {index} to the dictionary of signals failed.");
                }
            }
            signalInfo = _signals[index];
            return true;
        }
        /// <summary>
        /// Class constructor.
        /// </summary>
        public HvldPayload(byte[] payload, int numberOfSignals)
        {
            _payload = payload;
            _numberOfSignals = numberOfSignals;
            // Creates a map SIGNAL -> BYTES TO READ FOR SIGNAL.
            // This mapping has a double purpose:
            // 1) give a sequential integer ID to each signal, as the ones coming from GRETEL are not reliable.
            // 2) for each signal, precalculate the read buffer size and associate it with the signal ID.
            _mapSignalBufferInfo = new Dictionary<byte, SignalBufferInfo>();
            // For each signal, calculate the bytes to read.
            for (byte i = 0; i < _numberOfSignals; i++)
            {
                // Gets the SignalInfo data block.
                var signalDataBuffer = _payload.SubArray(SignalInfo.SIGNAL_INFO_BLOCK_SIZE * i, SignalInfo.SIGNAL_INFO_BLOCK_SIZE);
                // Get some info from the header.
                var dataType = (DataTypeEnum)signalDataBuffer[1];
                var pointCount = BitConverter.ToUInt16(signalDataBuffer, 2);

                var offset = 0;
                SignalBufferInfo sbi = default;
                // Calculates the offset for the current buffer, based on the previous results.
                for (byte j = 0; j < i; j++)
                    offset += _mapSignalBufferInfo[j].SignalReadBufferSize;

                switch (dataType)
                {
                    case DataTypeEnum.UInt8:
                        // pointCount must be multiplied by 2, because of the 2 components (x, y) of the points.
                        sbi = new SignalBufferInfo(pointCount * 2, offset, signalDataBuffer);
                        break;

                    case DataTypeEnum.UInt16:
                        // pointCount must be multiplied by 2, because of the 2 components (x, y) of the points.
                        sbi = new SignalBufferInfo(pointCount * 2 * sizeof(ushort), offset, signalDataBuffer);
                        break;

                    case DataTypeEnum.Int32:
                        // pointCount must be multiplied by 2, because of the 2 components (x, y) of the points.
                        sbi = new SignalBufferInfo(pointCount * 2 * sizeof(int), offset, signalDataBuffer);
                        break;

                    case DataTypeEnum.UInt32:
                        // pointCount must be multiplied by 2, because of the 2 components (x, y) of the points.
                        sbi = new SignalBufferInfo(pointCount * 2 * sizeof(uint), offset, signalDataBuffer);
                        break;

                    case DataTypeEnum.Float32:
                        // pointCount must be multiplied by 2, because of the 2 components (x, y) of the points.
                        sbi = new SignalBufferInfo(pointCount * 2 * sizeof(float), offset, signalDataBuffer);
                        break;

                    case DataTypeEnum.Float64:
                        // pointCount must be multiplied by 2, because of the 2 components (x, y) of the points.
                        sbi = new SignalBufferInfo(pointCount * 2 * sizeof(double), offset, signalDataBuffer);
                        break;
                }
                // Loads the dictionary with the new entry.
                _mapSignalBufferInfo.Add(i, sbi);
            }
        }
    }
}
