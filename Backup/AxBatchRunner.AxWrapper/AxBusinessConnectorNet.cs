using System;
using Microsoft.Dynamics;
using Microsoft.Dynamics.BusinessConnectorNet;

namespace AxBatchRunner.AxWrapper
{
    internal sealed class AxBusinessConnectorNet : IAxBusinessConnector
    {
        private readonly Axapta _axaptaAdapter;

        /// <summary>
        ///   Default Constructor.
        ///   <para>
        ///     Creates a new instance of Microsoft.Dynamics.BusinessConnectorNext.Axapta object.
        ///   </para>
        /// </summary>
        /// <exception cref = "AxException">Thrown when an exception occurs trying to create an instance of the Axapta object</exception>
        public AxBusinessConnectorNet()
        {
            try
            {
                _axaptaAdapter = new Axapta();
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Can't create Axapta BC object: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Login to Axapta
        /// </summary>
        /// <param name = "user">User name to login</param>
        /// <param name = "password">Password of the user</param>
        /// <param name = "company">Name of the company to logon</param>
        /// <param name = "language">Language of the user</param>
        /// <param name = "configuration">Configuration name to use for the client</param>
        /// <exception cref = "AxException">Thrown when logon attempt fails</exception>
        /// <remarks>
        ///   Calls the Logon method with the appropriate parameters
        /// </remarks>
        public void Logon(string user, string password, string company, string language, string configuration)
        {
            try
            {
                _axaptaAdapter.Logon(company, language, null, configuration);
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Logon failed to axapta: {0}", exception), exception.InnerException);
            }
        }

        /// <summary>
        ///   Logoff from Axapta
        /// </summary>
        /// <exception cref = "AxException">Thrown when logoff attempt fails</exception>
        public void Logoff()
        {
            try
            {
                _axaptaAdapter.Logoff();
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Logoff failed from axapta: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        public object CallStatic(string className, string methodName)
        {
            try
            {
                return _axaptaAdapter.CallStaticClassMethod(className, methodName);
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Call static method failed: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <param name = "param1">Parameter 1</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        public object CallStatic(string className, string methodName, object param1)
        {
            try
            {
                return _axaptaAdapter.CallStaticClassMethod(className, methodName, param1);
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Call static method failed: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <param name = "param1">Parameter 1</param>
        /// <param name = "param2">Parameter 2</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        public object CallStatic(string className, string methodName, object param1, object param2)
        {
            try
            {
                return _axaptaAdapter.CallStaticClassMethod(className, methodName, param1, param2);
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Call static method failed: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <param name = "param1">Parameter 1</param>
        /// <param name = "param2">Parameter 2</param>
        /// <param name = "param3">Parameter 3</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        public object CallStatic(string className, string methodName, object param1, object param2, object param3)
        {
            try
            {
                return _axaptaAdapter.CallStaticClassMethod(className, methodName, param1, param2, param3);
            }
            catch (AxaptaException exception)
            {
                throw new AxException(string.Format("Call static method failed: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Implementation of the Dispose Pattern
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Implementation of the Dispose Pattern
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _axaptaAdapter)
                {
                    _axaptaAdapter.Dispose();
                }
            }
        }

        /// <summary>
        ///   Destructor. Calls the <c>Dispose</c> method.
        /// </summary>
        ~AxBusinessConnectorNet()
        {
            Dispose(false);
        }
    }
}