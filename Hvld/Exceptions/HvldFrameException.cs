using System;

namespace Hvld
{
    /// <summary>
    /// 
    /// </summary>
    public class HvldFrameException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HvldFrameException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public HvldFrameException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public HvldFrameException(string message, Exception inner) : base(message, inner) { }
    }
}
