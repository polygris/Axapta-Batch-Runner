using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   <c>AxException</c> class provides an abstraction for all the
    ///   error messages thrown by the Axapta business connector.
    /// </summary>
    [Serializable]
    public class AxException : ApplicationException
    {
        private int errorCode;

        /// <summary>
        ///   Default Constructor
        /// </summary>
        public AxException()
        {
        }

        /// <summary>
        ///   Parameterized Constructor
        /// </summary>
        /// <param name = "message">Error Message</param>
        public AxException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///   Parameterized Constructor
        /// </summary>
        /// <param name = "message">Error message</param>
        /// <param name = "innerException">Inner Exception</param>
        public AxException(string message, Exception innerException)
            : base(message, innerException)
        {
            COMException comException = innerException as COMException;
            if (comException != null)
            {
                errorCode = comException.ErrorCode;
            }
        }

        /// <summary>
        ///   Parameterized Constructor. Facilitates Serialization
        /// </summary>
        /// <param name = "info">Serialization info</param>
        /// <param name = "context">Streaming Context</param>
        protected AxException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///   HRESULT from COM+ object
        /// </summary>
        public int ErrorCode
        {
            get { return errorCode; }
            internal set { errorCode = value; }
        }
    }
}