using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Hvld.Parser
{
    /// <summary>
    /// Class representing an HVLD 2.0 SignalInfo.
    /// </summary>
    public sealed class SignalInfo
    {
        /// <summary>
        /// Size of a SignalInfo block (see C++ struct HvldSignalInfo).
        /// </summary>
        public const int SIGNAL_INFO_BLOCK_SIZE = 268;
        /// <summary>
        /// 
        /// </summary>
        public byte GretelId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DataTypeEnum DataType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ushort NumberOfPoints { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public uint RGBA { get; private set; }
        /// <summary>
        /// NOTE: the color code from GRETEL always have the alpha component
        /// set to 0 (transparent). We force it to 0xFF.
        /// </summary>
        public Color SignalColor
        {
            get
            {
                return HvldUtilities.GretelRGBAToColor(RGBA);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ClassificationEnum Classification { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte OverlaySignalId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int YAxisMinClamp { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int YAxisMaxClamp { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<Point2D> Points { get; private set; } = new List<Point2D>();
        /// <summary>
        /// Parallel construction class initializer.
        /// </summary>
        public SignalInfo(
            byte[] signalInfo,
            byte[] payload, 
            int numberOfSignals, 
            int dataOffset, 
            int bytesToRead,
            byte? signalIdOverride = null)
        {
            try
            {
                if (signalIdOverride.HasValue)
                    GretelId = signalIdOverride.Value;
                else
                    GretelId = signalInfo[0];

                DataType = (DataTypeEnum)signalInfo[1];
                NumberOfPoints = BitConverter.ToUInt16(signalInfo, 2);
                RGBA = BitConverter.ToUInt32(signalInfo, 4);
                Classification = (ClassificationEnum)signalInfo[8];
                OverlaySignalId = signalInfo[9];
                // These following values can be int.MaxValue to indicate +inf,-inf.
                YAxisMinClamp = BitConverter.ToInt32(signalInfo, 12);
                YAxisMaxClamp = BitConverter.ToInt32(signalInfo, 16);
                var buffer = signalInfo.SubArray(20, SIGNAL_INFO_BLOCK_SIZE - 20);

                Title = Encoding.UTF8.GetString(buffer, 0, buffer.Length).Trim().ToUpper();
                // Replaces all the occurrences of the null terminator.
                if (!string.IsNullOrWhiteSpace(Title))
                    Title = Title.Replace("\0", string.Empty);
                // Data block containing all the SignalInfo sections.
                var pointDataBlock = payload.SubArray(SIGNAL_INFO_BLOCK_SIZE * numberOfSignals, payload.Length - SIGNAL_INFO_BLOCK_SIZE * numberOfSignals);
                // Buffer for the signal data points.
                byte[] signalData;
                // Size of the DataType.
                var typeSize = 0;

                switch (DataType)
                {
                    case DataTypeEnum.UInt8:
                        // No need for typeSize here, as for a byte is 1.
                        signalData = pointDataBlock.SubArray(dataOffset, bytesToRead);
                        for (var j = 0; j < NumberOfPoints; j++)
                            Points.Add(new Point2D(signalData[j], signalData[j + NumberOfPoints]));
                        break;

                    case DataTypeEnum.UInt16:
                      
                        typeSize = sizeof(ushort);
                        signalData = pointDataBlock.SubArray(dataOffset, bytesToRead);
                        for (var j = 0; j < NumberOfPoints; j++)
                        {
                            Points.Add(new Point2D(
                                BitConverter.ToUInt16(signalData, j * typeSize),
                                BitConverter.ToUInt16(signalData, (j * typeSize) + (NumberOfPoints * typeSize))));
                        }
                        break;

                    case DataTypeEnum.UInt32:

                        typeSize = sizeof(uint);
                        signalData = pointDataBlock.SubArray(dataOffset, bytesToRead);
                        for (var j = 0; j < NumberOfPoints; j++)
                        {
                            Points.Add(new Point2D(
                                BitConverter.ToUInt32(signalData, j * typeSize),
                                BitConverter.ToUInt32(signalData, (j * typeSize) + (NumberOfPoints * typeSize))));
                        }
                        break;

                    case DataTypeEnum.Int32:

                        typeSize = sizeof(int);
                        signalData = pointDataBlock.SubArray(dataOffset, bytesToRead);
                        for (var j = 0; j < NumberOfPoints; j++)
                        {
                            Points.Add(new Point2D(
                                BitConverter.ToInt32(signalData, j * typeSize),
                                BitConverter.ToInt32(signalData, (j * typeSize) + (NumberOfPoints * typeSize))));
                        }
                        break;

                    case DataTypeEnum.Float32:

                        typeSize = sizeof(float);
                        signalData = pointDataBlock.SubArray(dataOffset, bytesToRead);
                        for (var j = 0; j < NumberOfPoints; j++)
                        {
                            Points.Add(new Point2D(
                                BitConverter.ToSingle(signalData, j * typeSize),
                                BitConverter.ToSingle(signalData, (j * typeSize) + (NumberOfPoints * typeSize))));
                        }
                        break;

                    case DataTypeEnum.Float64:

                        typeSize = sizeof(double);
                        signalData = pointDataBlock.SubArray(dataOffset, bytesToRead);
                        for (var j = 0; j < NumberOfPoints; j++)
                        {
                            Points.Add(new Point2D(
                                BitConverter.ToDouble(signalData, j * typeSize),
                                BitConverter.ToDouble(signalData, (j * typeSize) + (NumberOfPoints * typeSize))));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new HvldSignalInfoException("SignalInfo::ctor: exception raised during the SignalInfo block parsing.", ex);
            }
        }
    }
}
