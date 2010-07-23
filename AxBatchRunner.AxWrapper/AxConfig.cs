namespace AxBatchRunner.AxWrapper
{
    /// <summary>
    ///   Class for store information about Axapta configuration
    /// </summary>
    public class AxConfig
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

        public string AOSName
        {
            get { return _aosName; }
        }

        public bool IsAos
        {
            get { return _isAos; }
        }

        public string LicenseName
        {
            get { return _licenceName; }
        }

        public string SerialNo
        {
            get { return _serialNo; }
        }

        public override string ToString()
        {
            return string.Format("SerialNo: {0}, LicenseName: {1}, IsAOS: {2}, AOSName: {3}", SerialNo, LicenseName, IsAos, AOSName);
        }
    }
}