namespace Hvld.Parser
{
    /// <summary>
    /// Signal data type.
    /// </summary>
    public enum DataTypeEnum 
    {
        UInt8 = 0,
        UInt16,
        UInt32,
        Int32,
        Float32,
        Float64
    }
    /// <summary>
    /// Classification type.
    /// </summary>
    public enum ClassificationEnum : byte
    {
        InSpecs  = 0,
        OutSpecs = 255
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ClampingModeEnum : byte 
    {
        None = 0,
        Simple,
        Accurate
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ClampingSide : byte
    {
        Upper = 0,
        Lower,
        Both
    }
}
