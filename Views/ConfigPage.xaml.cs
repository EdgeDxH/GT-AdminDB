using GT_AdminDB.Clases;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GT_AdminDB.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page
    {
        ConfiguracionApp _configuracionApp = (App.Current as App).ConfiguracionApp;
        public ConfigPage()
        {
            this.InitializeComponent();
            txtPathDB.Text = (App.Current as App).ConfiguracionApp.PathDB;
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                Debug.WriteLine("GoBack");
            }
            else
            {
                Frame.Navigate(typeof(MainPage));
                Debug.WriteLine("MainPage");
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            _configuracionApp.PathDB = txtPathDB.Text;
            (App.Current as App).GuardarConfiguracion();
        }

        private async void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var window = (App.Current as App).m_window as MainWindow;

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add(".json");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                List<Miembro> miembrosImport = new List<Miembro>();
                List<Raid> raidImport = new List<Raid>();
                List<Participacion> participacionImport = new List<Participacion>();
                if (file.Name.Contains("Export"))
                {
                    string jsonFile = File.ReadAllText(file.Path);
                    Dictionary<string, object> jsonDataMerged = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonFile);
                    //Obtener raids individuales
                    int raidNuevoID = 1;
                    Dictionary<int,int> convetirIdFechaRaid = new Dictionary<int,int>();
                    foreach (JObject raidJson in jsonDataMerged["Raids"] as JArray)
                    {
                        convetirIdFechaRaid.Add(int.Parse(raidJson["Id"].ToString()), raidNuevoID);
                        Raid raidTemp = new Raid();
                        raidTemp.Id = raidNuevoID;
                        raidTemp.Fecha_Inicio = DateOnly.Parse(raidJson["Fecha_Inicio"].ToString());
                        raidImport.Add(raidTemp);
                        raidNuevoID++;
                    }
                    //Obtener miembros y formatear el id
                    int miembroNuevoId = 1;
                    Dictionary<int, int> convetirIdMiembro = new Dictionary<int, int>(); // <antiguoId, nuevoId>
                    foreach (JObject miembroJson in jsonDataMerged["Miembros"] as JArray)
                    {
                        Miembro miembroTemp = new Miembro();
                        convetirIdMiembro.Add(Int32.Parse(miembroJson["Id"].ToString()), miembroNuevoId);
                        miembroTemp.Id= miembroNuevoId;
                        miembroTemp.Nombre = miembroJson["Nombre"].ToString();
                        miembroTemp.Fecha_Ingreso = DateOnly.Parse(miembroJson["Fecha_Ingreso"].ToString());
                        miembroTemp.Ultimo_Login = DateOnly.Parse(miembroJson["Ultimo_Login"].ToString());
                        miembrosImport.Add(miembroTemp);
                        miembroNuevoId++;
                    }
                    //Obtener participaciones y acutalizar el id de las raid
                    int participacionNuevoId = 1;
                    foreach (JObject participacionJson in jsonDataMerged["Participaciones"] as JArray)
                    {
                        Participacion participacionTemp = new Participacion();
                        participacionTemp.Id = participacionNuevoId;
                        participacionTemp.Total_Damage = Int32.Parse(participacionJson["Total_Damage"].ToString());
                        participacionTemp.Intentos_Totales = Int32.Parse(participacionJson["Intentos_Totales"].ToString());
                        participacionTemp.Fk_Id_Raid = convetirIdFechaRaid[int.Parse(participacionJson["Fk_Id_Raid"].ToString())];
                        participacionTemp.Fk_Id_Miembro = convetirIdMiembro[Int32.Parse(participacionJson["Fk_Id_Miembro"].ToString())];
                        participacionImport.Add(participacionTemp);
                        participacionNuevoId++;
                    }
                    MiembroCollection miembroCollection = new MiembroCollection();
                    foreach (Miembro miembroAdd in miembrosImport)
                    {
                        miembroCollection.Add(miembroAdd);
                    }
                    RaidCollection raidCollection = new RaidCollection();
                    foreach (Raid raidAdd in raidImport)
                    {
                        raidCollection.Add(raidAdd);
                    }
                    ParticipacionCollection participacionCollection = new ParticipacionCollection();
                    foreach (Participacion partiAdd in participacionImport)
                    {
                        participacionCollection.Add(partiAdd);
                    }
                    Debug.WriteLine("Lissttttooooo");
                }
            }
            else
            {
                Debug.WriteLine("Operacion cancelada");
            }

        }

        private void btnExportar_Click(object sender, RoutedEventArgs e)
        {
            MiembroCollection miembroCollection = new MiembroCollection();
            miembroCollection.GetMiembros();
            RaidCollection raidCollection = new RaidCollection();
            raidCollection.GetRaids();
            ParticipacionCollection participacionCollection = new ParticipacionCollection();
            participacionCollection.GetParticipaciones();

            ExportarFormat exportarJson = new ExportarFormat()
            {
                Miembros = miembroCollection.Miembros.ToList(),
                Raids = raidCollection.Raids.ToList(),
                Participaciones = participacionCollection.Participaciones.ToList()
            };
            var archivoJson = JsonConvert.SerializeObject(exportarJson, Formatting.Indented); //Las fechas se guardan en el formato yyyy-mm-dd
            string nombreJson = "Export-"+DateOnly.FromDateTime(DateTime.Now).ToString().Replace("/","-")+".json";
            File.WriteAllText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nombreJson), archivoJson);
            ((App.Current as App).m_window as MainWindow).InfoResultado(1,"Datos exportados con exito.");
        }
    }

    class ExportarFormat
    {
        public List<Miembro> Miembros { get; set; }
        public List<Raid> Raids { get; set; }
        public List<Participacion> Participaciones { get; set; }
    }
}
