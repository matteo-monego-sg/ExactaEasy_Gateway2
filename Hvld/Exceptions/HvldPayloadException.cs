using System;

namespace Hvld
{
    /// <summary>
    /// 
    /// </summary>
    public class HvldPayloadException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HvldPayloadException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public HvldPayloadException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public HvldPayloadException(string message, Exception inner) : base(message, inner) { }
    }
}
