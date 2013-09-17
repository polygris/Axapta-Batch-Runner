using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using AxaptaCOMConnector;

namespace AxBatchRunner.AxWrapper
{
    internal sealed class AxBusinessConnectorCom : IAxBusinessConnector
    {
        private IAxapta2 _axaptaAdapter;
        public string ComPlusAppl { get; set; }
        private readonly ThreadSafeCounter _connections = new ThreadSafeCounter();

        /// <summary>
        ///   Default Constructor.
        ///   <para>
        ///     Creates a new instance of COM Axapta object.
        ///   </para>
        /// </summary>
        /// <exception cref = "AxException">Thrown when an exception occurs trying to create an instance of the Axapta object</exception>
        public AxBusinessConnectorCom()
        {
            try
            {
                _axaptaAdapter = new Axapta2Class();
            }
            catch (COMException exception)
            {
                throw new AxException(string.Format("Can't create Axapta BC object: {0}", exception), exception);
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
        public void Logon(string user, string password, string company, string language, string configuration)
        {
            try
            {
                if (_connections.Value > 0)
                {
                    Logoff();
                    _connections.Decrement();
                    _axaptaAdapter = new Axapta2Class();
                }
                _axaptaAdapter.Logon2(user, password, company, language, "", "", configuration, false, null, null);
                _connections.Increment();
            }
            catch (COMException exception)
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
                if (_connections.Value > 0)
                {
                    ShutdownComPlus();
                    _connections.Decrement();
                }
            }
            catch (COMException exception)
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
                return CallStatic(className, methodName, null);
            }
            catch (COMException exception)
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
                return CallStatic(className, methodName, param1, null);
            }
            catch (COMException exception)
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
                return _axaptaAdapter.CallStaticClassMethod(className, methodName, param1, param2, null, null, null,
                                                            null);
            }
            catch (COMException exception)
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
        /// <exception cref="AxException"></exception>
        /// <returns>Thrown when there is an exception in calling the static method.</returns>
        public object CallStatic(string className, string methodName, object param1, object param2, object param3)
        {
            try
            {
                return _axaptaAdapter.CallStaticClassMethod(className, methodName, param1, param2, param3, null, null,
                                                            null);
            }
            catch (COMException exception)
            {
                throw new AxException(string.Format("Call static method failed: {0}", exception),
                                      exception.InnerException);
            }
        }

        /// <summary>
        ///   Shutdown COM object of Navision Axapta Bussines Connector
        /// </summary>
        private void ShutdownComPlus()
        {
            object target = Activator.CreateInstance(Type.GetTypeFromProgID("COMAdmin.COMAdminCatalog"));
            string _comPlusAppl = string.IsNullOrEmpty(ComPlusAppl) ? "Navision Axapta Business Connector" : ComPlusAppl;
            object[] args = new object[] {_comPlusAppl};

            target.GetType().InvokeMember("ShutdownApplication", BindingFlags.InvokeMethod, null, target, args);
            Thread.Sleep(100);

            Marshal.ReleaseComObject(target);
            GC.GetTotalMemory(true);
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
                if ((_axaptaAdapter != null))
                {
                    try
                    {
                        _axaptaAdapter.Logoff();
                        ShutdownComPlus();
                    }
                    catch
                    {
                    }
                }
                _axaptaAdapter = null;
            }
        }

        /// <summary>
        ///   Destructor. Calls the <c>Dispose</c> method.
        /// </summary>
        ~AxBusinessConnectorCom()
        {
            Dispose();
        }
    }
}