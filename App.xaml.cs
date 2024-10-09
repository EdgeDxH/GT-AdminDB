using GT_AdminDB.Clases;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GT_AdminDB
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; set; }
        public ConfiguracionApp ConfiguracionApp { get; private set; }
        public Window m_window { get; set; }
        public App()
        {
            this.InitializeComponent();
            this.CargarConfiguracion();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private void CargarConfiguracion()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            ConfiguracionApp = Configuration.GetSection("ConfiguracionApp").Get<ConfiguracionApp>();
        }

        public void GuardarConfiguracion()
        {
            ConfiguracionRaiz configTemp = new ConfiguracionRaiz();
            configTemp.ConfiguracionApp = ConfiguracionApp;

            var configJson = JsonConvert.SerializeObject(configTemp, Formatting.Indented);
            File.WriteAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "config.json"), configJson);
        }
    }
}
