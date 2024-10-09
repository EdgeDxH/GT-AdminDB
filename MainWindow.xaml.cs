using GT_AdminDB.Clases;
using GT_AdminDB.Views;
using Microsoft.UI;
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
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GT_AdminDB
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        MiembroCollection listaMiembros = new MiembroCollection();
        RaidCollection listaRaids = new RaidCollection();
        Miembro miembroAuxiliar = new Miembro();
        Participacion participacionAuxiliar = new Participacion();

        bool editarMiembro = false;
        bool editarParticipacion = false;
        bool expanderMiembrosOpen = false;
        bool expanderRaidOpen = false;

        public MainWindow()
        {
            this.InitializeComponent();
            dpkFechaIngreso.DateFormat = "{day.integer}/{month.integer}/{year.full}";
            dpkFechaIngreso.Date = DateTime.Now;
            dpkFechaIngreso.MaxDate = DateTime.Now;
            dpkUltimoLogin.DateFormat = "{day.integer}/{month.integer}/{year.full}";
            dpkUltimoLogin.Date = DateTime.Now;
            dpkUltimoLogin.MaxDate = DateTime.Now;
            dpkFechaRaid.DateFormat = "{day.integer}/{month.integer}/{year.full}";
            dpkFechaRaid.Date = DateTime.Now;
            contentFrame.Navigate(typeof(MainPage));
            listaRaids.GetRaids();
        }
        //METODOS DE PAGINA
        private void btnOpenPane_Click(object sender, RoutedEventArgs e)
        {
            switch (splitView.IsPaneOpen)
            {
                case true:
                    splitView.IsPaneOpen = false;
                    break;
                case false:
                    splitView.IsPaneOpen = true;
                    break;
            }
        }

        private void btnAgregarMiembro_Click(object sender, RoutedEventArgs e)
        {
            if (contentFrame.Content.GetType().Name.Equals("MainPage"))
            {
                if (txtNombre.Text.Trim() != "")
                {
                    //Editar Miembro
                    if (editarMiembro)
                    {
                        miembroAuxiliar.Nombre = txtNombre.Text;
                        miembroAuxiliar.Fecha_Ingreso = DateOnly.FromDateTime(dpkFechaIngreso.Date.Value.DateTime);
                        miembroAuxiliar.Ultimo_Login = DateOnly.FromDateTime(dpkUltimoLogin.Date.Value.DateTime);

                        var mainPage = contentFrame.Content as MainPage;
                        mainPage.EditarMiembro(miembroAuxiliar);

                        btnCancelarMiembro.Visibility = Visibility.Collapsed;
                        btnAgregarMiembro.Content = "Agregar";
                        editarMiembro = false;

                        ResetFields(0);
                    }
                    //Agregar Miembro
                    else
                    {
                        DateTime tempFechaIngreso = dpkFechaIngreso.Date.Value.DateTime;
                        DateTime tempUltimoLogin = dpkUltimoLogin.Date.Value.DateTime;
                        Miembro miembroTemp = new Miembro()
                        {
                            //El ID se agrega en el MainPage
                            Nombre = txtNombre.Text,
                            Fecha_Ingreso = DateOnly.FromDateTime(tempFechaIngreso),
                            Ultimo_Login = DateOnly.FromDateTime(tempUltimoLogin)
                        };
                        var frameTemp = contentFrame.Content as MainPage;
                        frameTemp.AgregarMiembro(miembroTemp);

                        ResetFields(0);
                    }
                }
                else { Debug.WriteLine("Debe ingresar un nombre"); }
            }
        }

        private void btnCancelarMiembro_Click(object sender, RoutedEventArgs e)
        {
            btnAgregarMiembro.Content = "Agregar";
            btnCancelarMiembro.Visibility = Visibility.Collapsed;
            ResetFields(0);
        }

        private void btnAgregarRaid_Click(object sender, RoutedEventArgs e)
        {
            if (!editarParticipacion)
            {

            }
            else
            {
                if (contentFrame.Content.GetType().Name == "ViewParticipacion")
                {
                    ViewParticipacion frameActual = contentFrame.Content as ViewParticipacion;
                    participacionAuxiliar.Total_Damage = (int)txtDanoTotal.Value;
                    participacionAuxiliar.Intentos_Totales = (int)txtCantidadRaid.Value;
                    frameActual.EditarParticipacion(participacionAuxiliar);
                    btnEliminarRaid_Click(null, null);
                }
            }
        }

        private void btnEliminarRaid_Click(object sender, RoutedEventArgs e)
        {
            if (!editarParticipacion)
            {
                Raid raidTemp = listaRaids.Raids.FirstOrDefault(x => x.Fecha_Inicio == DateOnly.FromDateTime(dpkFechaRaid.Date.Value.DateTime));
                if (raidTemp != null)
                {
                    bool error = listaRaids.DeleteRaidById(raidTemp.Id);
                    switch (error)
                    {
                        case true:
                            InfoResultado(2, "Error al eliminar raid");
                            break;
                        case false:
                            InfoResultado(1, "Raid Eliminada");
                            break;
                    }
                }
                else
                {
                    InfoResultado(0, "Raid no encontrada");
                }
            }
            else
            {
                editarParticipacion = false;
                ResetFields(1);
                btnAgregarRaid.Content = "Agregar";
                btnEliminarRaid.Content = "Eliminar";
                btnStartRaid.Visibility = Visibility.Visible;
            }
        }

        private void btnStartRaid_Click(object sender, RoutedEventArgs e)
        {
            if ((listaRaids.Raids.FirstOrDefault(x => x.Fecha_Inicio == DateOnly.FromDateTime(dpkFechaRaid.Date.Value.DateTime))) != null)
            {
                Debug.WriteLine("Raid ya existe");
            }
            else
            {
                Raid raid = new Raid
                {
                    Fecha_Inicio = DateOnly.FromDateTime(dpkFechaRaid.Date.Value.DateTime)
                };
                bool error = listaRaids.AddRaidParticipacion(raid);
                switch (error)
                {
                    case true:
                        InfoResultado(2, "Error al iniciar raid");
                        break;
                    case false:
                        InfoResultado(1, "Raid Iniciada");
                        break;
                }
            }
        }

        private void chkSeleccionMultiple_Unchecked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.Content.GetType().Name == "MainPage")
            {
                MainPage mainPage = contentFrame.Content as MainPage;
                mainPage.ModoSeleccion(ListViewSelectionMode.Single);
            }
        }

        private void chkSeleccionMultiple_Checked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.Content.GetType().Name == "MainPage")
            {
                MainPage mainPage = contentFrame.Content as MainPage;
                mainPage.ModoSeleccion(ListViewSelectionMode.Multiple);
            }
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.NavigateToType(typeof(ConfigPage), null, new FrameNavigationOptions
            {
                IsNavigationStackEnabled = false
            });
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            string fechaTest = "2024-12-01";
            DateOnly dateWea = DateOnly.Parse(fechaTest);
            Debug.WriteLine(dateWea);
        }
        //METODOS TRANSITORIOS
        public void EditarMiembro(Miembro miembroRecibido)
        {
            if (!splitView.IsPaneOpen)
            {
                btnOpenPane_Click(null, null);
            }
            if (!expMiembros.IsExpanded)
            {
                expMiembros.IsExpanded = true;
                expanderMiembrosOpen = true;
            }
            editarMiembro = true;
            btnCancelarMiembro.Visibility = Visibility.Visible;
            btnAgregarMiembro.Content = "Editar";
            this.miembroAuxiliar = miembroRecibido;
            txtNombre.Text = this.miembroAuxiliar.Nombre;
            dpkFechaIngreso.Date = this.miembroAuxiliar.Fecha_Ingreso.ToDateTime(TimeOnly.FromDateTime(DateTime.Now));
            dpkUltimoLogin.Date = this.miembroAuxiliar.Ultimo_Login.ToDateTime(TimeOnly.FromDateTime(DateTime.Now));
        }
        public void EditarParticipacion(Participacion participacionEdit)
        {
            if (!splitView.IsPaneOpen)
            {
                btnOpenPane_Click(null, null);
            }
            if (!expRaid.IsExpanded)
            {
                expRaid.IsExpanded = true;
                expanderRaidOpen = true;
            }
            editarParticipacion= true;
            btnAgregarRaid.Content = "Guardar";
            btnEliminarRaid.Content = "Cancelar";
            btnStartRaid.Visibility = Visibility.Collapsed;
            dpkFechaRaid.Date = participacionEdit.Raid.Fecha_Inicio.ToDateTime(TimeOnly.FromDateTime(DateTime.Now));
            txtCantidadRaid.Text = participacionEdit.Intentos_Totales.ToString();
            txtDanoTotal.Text = participacionEdit.Total_Damage.ToString();
            participacionAuxiliar = participacionEdit;
        }
        //FILTROS
        private void chkFiltroLogout_Checked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.Content.GetType().Name == "MainPage")
            {
                MainPage mainPage = contentFrame.Content as MainPage;
                mainPage.OrderByLogin(true);
            }
        }

        private void chkFiltroLogout_Unchecked(object sender, RoutedEventArgs e)
        {
            if (contentFrame.Content.GetType().Name == "MainPage")
            {
                MainPage mainPage = contentFrame.Content as MainPage;
                mainPage.OrderByLogin(false);
            }
        }

        private void OnFilterChanged(object sender, TextChangedEventArgs e)
        {
            if (contentFrame.Content.GetType().Name.Equals("MainPage"))
            {
                MainPage frameMainPage = contentFrame.Content as MainPage;
                frameMainPage.OnFilterChanged(txtFiltroNombre.Text);
            }
        }
        //METODOS DE UTILIDAD
        private void ResetFields(int expNro)
        {
            switch (expNro)
            {
                case 0:
                    txtNombre.Text = String.Empty;
                    dpkFechaIngreso.Date = DateTime.Now;
                    dpkUltimoLogin.Date = DateTime.Now;
                    break;
                case 1:
                    dpkFechaRaid.Date = DateTime.Now;
                    txtCantidadRaid.Text = "0";
                    txtDanoTotal.Text = "0";
                    break;
            }
        }

        public void InfoResultado(int status, string mensaje)
        {
            switch (status)
            {
                case 0:
                    infobarMain.Severity = InfoBarSeverity.Informational;
                    infobarMain.Background = GetSolidColorBrush("#323232");
                    break;
                case 1:
                    infobarMain.Severity = InfoBarSeverity.Success;
                    infobarMain.Background = GetSolidColorBrush("#027200");
                    break;
                case 2:
                    infobarMain.Severity = InfoBarSeverity.Error;
                    infobarMain.Background = GetSolidColorBrush("#9D0B00");
                    break;
            }
            infobarMain.Message = mensaje;
            infobarMain.IsOpen = true;
        }

        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            int hexLenght = hex.Length;
            if (hexLenght == 6)
            {
                Color color = Windows.UI.ColorHelper.FromArgb(
                        255,
                        Convert.ToByte(hex.Substring(0, 2), 16),
                        Convert.ToByte(hex.Substring(2, 2), 16),
                        Convert.ToByte(hex.Substring(4, 2), 16)
                        );
                SolidColorBrush myBrush = new SolidColorBrush(color);
                return myBrush;
            }
            else
            {
                Color color = Windows.UI.ColorHelper.FromArgb(
                        Convert.ToByte(hex.Substring(0, 2), 16),
                        Convert.ToByte(hex.Substring(2, 2), 16),
                        Convert.ToByte(hex.Substring(4, 2), 16),
                        Convert.ToByte(hex.Substring(6, 2), 16)
                        );
                SolidColorBrush myBrush = new SolidColorBrush(color);
                return myBrush;
            }
        }
    }
}
