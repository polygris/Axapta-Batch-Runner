using System;

namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   Incapsulated Axapta settings;
    /// </summary>
    public class AxSettings
    {
        /// <summary>
        ///   Initalize default values
        /// </summary>
        public AxSettings()
        {
            ComPlusAppl = "Navision Axapta Business Connector";
            BatchRunnerClass = "AxBatchRunner";
            BatchRunnerMethod = "mainDo";
            CancelJobIfError = true;
            TimerInterval = 10;
        }

        private ushort _daxVersion;

        /// <summary>
        ///   User for login to Axapta (for version 3)
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///   Password for user (for version 3)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///   Default company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        ///   Default language
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        ///   Name of COM+ application (default: Navision Axapta Business Connector)
        /// </summary>
        public string ComPlusAppl { get; set; }

        /// <summary>
        ///   Name of AxConfig configuration
        /// </summary>
        public string Configuration { get; set; }

        public string ObjectServer { get; set; }
        public string BatchRunnerMethod { get; set; }
        public string BatchRunnerClass { get; set; }
        public string BatchGroup { get; set; }
        public bool CancelJobIfError { get; set; }

        /// <summary>
        ///   Timer interval in sec.
        /// </summary>
        public uint TimerInterval { get; set; }

        /// <summary>
        ///   Validate settings
        /// </summary>
        public void ValidateSettings()
        {
            if (DaxVersion == 3)
            {
                if (string.IsNullOrEmpty(User))
                    throw new ArgumentNullException("User", "User can't be empty.");

                if (string.IsNullOrEmpty(Configuration))
                    throw new ArgumentNullException("Configuration", "Configuration can't be empty.");
            }
            else
            {
                if (string.IsNullOrEmpty(ObjectServer))
                    throw new ArgumentNullException("ObjectServer", "ObjectServer can't be empty.");
            }

            if (TimerInterval < 10)
                throw new ArgumentOutOfRangeException("TimerInterval", TimerInterval,
                                                      "TimerInterval can't be less 10 sec.");
        }

        /// <summary>
        ///   DAX version
        /// </summary>
        public ushort DaxVersion
        {
            get { return _daxVersion; }
            set
            {
                if (_daxVersion < 3 && _daxVersion > 5)
                    throw new ArgumentOutOfRangeException("DaxVersion", value,
                                                          "DAX version can be in range from 3 to 5.");
                _daxVersion = value;
            }
        }

        public override string ToString()
        {
            return string.Format("Configuration: {0}, User: {1}, Password: {2}, Language: {3}, Company: {4}",
                                 Configuration, User, Password, Language, Company);
        }
    }
}