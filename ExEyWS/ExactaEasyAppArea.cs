namespace ExactaEasyEng
{
    /// <summary>
    /// Defines a rectangular application area for the supervisor.
    /// </summary>
    public struct ExactaEasyAppArea
    {
        /// <summary>
        /// 
        /// </summary>
        public ExactaEasyAppArea(int topLeftX, int topLeftY, int width, int height)
        {
            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// 
        /// </summary>
        public int TopLeftX { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int TopLeftY { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Height { get; private set; }
    }
}
