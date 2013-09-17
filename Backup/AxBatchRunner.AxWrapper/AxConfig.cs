namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   Class for store information about Axapta configuration
    /// </summary>
    public sealed class AxConfig
    {
        private readonly bool _isAos;
        private readonly string _licenceName;
        private readonly string _serialNo;
        private readonly string _aosName;

        public AxConfig(string serialNo, string licenseName, bool isAos, string aosName)
        {
            _serialNo = serialNo;
            _aosName = aosName;
            _licenceName = licenseName;
            _isAos = isAos;
        }

        /// <summary>
        ///   Application object server name
        /// </summary>
        public string AOSName
        {
            get { return _aosName; }
        }

        /// <summary>
        ///   Running on AOS?
        /// </summary>
        public bool IsAos
        {
            get { return _isAos; }
        }

        /// <summary>
        ///   License holder name
        /// </summary>
        public string LicenseName
        {
            get { return _licenceName; }
        }

        /// <summary>
        ///   Serial number for Axapta installation
        /// </summary>
        public string SerialNo
        {
            get { return _serialNo; }
        }

        public override string ToString()
        {
            return string.Format("SerialNo: {0}, LicenseName: {1}, IsAOS: {2}, AOSName: {3}", SerialNo, LicenseName,
                                 IsAos, AOSName);
        }
    }
}