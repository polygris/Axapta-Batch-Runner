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

        public AxConfig(string serialNo, string licenseName, bool isAos)
        {
            _serialNo = serialNo;
            _licenceName = licenseName;
            _isAos = isAos;
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
            return string.Format("SerialNo: {0}, LicenseName: {1}, IsAOS: {2}", SerialNo, LicenseName, IsAos);
        }
    }
}