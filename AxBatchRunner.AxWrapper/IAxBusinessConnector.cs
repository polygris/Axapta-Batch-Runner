using System;

namespace AxBatchRunner.AxWrapper
{
    public interface IAxBusinessConnector : IDisposable
    {
        /// <summary>
        ///   Login to Axapta
        /// </summary>
        /// <param name = "user">User name to login</param>
        /// <param name = "password">Password of the user</param>
        /// <param name = "company">Name of the company to logon</param>
        /// <param name = "language">Language of the user</param>
        /// <param name = "configuration">Configuration name to use for the client</param>
        /// <param name = "objectServer">Axapta Server to logon (AOS)</param>
        /// <exception cref = "AxException">Thrown when logon attempt fails</exception>
        void Logon(string user, string password, string company, string language, string configuration,
                   string objectServer);

        /// <summary>
        ///   Logoff from Axapta
        /// </summary>
        /// <returns>Success or Failure</returns>
        /// <exception cref = "AxException">Thrown when logoff attempt fails</exception>
        void Logoff();

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        object CallStatic(string className, string methodName);

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <param name = "param1">Parameter 1</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        object CallStatic(string className, string methodName, object param1);

        /// <summary>
        ///   Calls an Axapta class static method.
        /// </summary>
        /// <param name = "className">Name of the class</param>
        /// <param name = "methodName">Name of the static method</param>
        /// <param name = "param1">Parameter 1</param>
        /// <param name = "param2">Parameter 1</param>
        /// <returns>Value returned by Axapta</returns>
        /// <exception cref = "AxException">Thrown when there is an exception in calling the static method.</exception>
        object CallStatic(string className, string methodName, object param1, object param2);
    }
}