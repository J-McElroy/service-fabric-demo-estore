using System.Fabric;

namespace ServiceFabric.Demo.EStore.ProductService.Settings
{
    internal class ProductServiceSettings : IProductServiceSettings
    {
        private readonly CodePackageActivationContext activationContext;
        private readonly object lockObj = new object();

        public ProductServiceSettings(CodePackageActivationContext activationContext)
        {
            this.activationContext = activationContext;
            SetValues();
            this.activationContext.ConfigurationPackageModifiedEvent += (s, e) => SetValues();
        }

        public string DefaultDescription { get; private set; }

        private void SetValues()
        {
            lock (lockObj)
            {
                var configPkg = activationContext.GetConfigurationPackageObject("Config");

                DefaultDescription = configPkg.Settings.Sections["ProductService"].Parameters["DefaultDescription"].Value;
            }
        }
    }
}
