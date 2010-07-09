using System.ComponentModel;
using System.Configuration.Install;

namespace AxBatchRunner.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
