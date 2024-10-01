using System;

namespace Hvld
{
    /// <summary>
    /// 
    /// </summary>
    public class HvldSignalInfoException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public HvldSignalInfoException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public HvldSignalInfoException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public HvldSignalInfoException(string message, Exception inner) : base(message, inner) { }
    }
}
