using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using GT_AdminDB.Clases;
using System.Globalization;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GT_AdminDB.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewParticipacion : Page
    {
        Miembro miembroAuxiliar = new Miembro();
        ParticipacionCollection participacionCollection = new ParticipacionCollection();
        public ViewParticipacion()
        {
            this.InitializeComponent();
            lvwParticipacion.ItemsSource = participacionCollection.Participaciones;
        }
        //METODOS DE PAGINA
        public void CargarLista(Miembro miembroTemp)
        {
            miembroAuxiliar = miembroTemp;
            txbNombre.Text = miembroAuxiliar.Nombre;
            participacionCollection.GetById(miembroTemp.Id, true);
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        //METODOS TRANSITORIOS
        public void EditarParticipacion(Participacion participacionEdit)
        {
            participacionCollection.UpdateParticipacion(participacionEdit);
        }
        private void MenuFlyoutItemEditar_Click(object sender, RoutedEventArgs e)
        {
            Participacion participacionTemp = (sender as MenuFlyoutItem).DataContext as Participacion;
            MainWindow mainWindow = (App.Current as App).m_window as MainWindow;
            mainWindow.EditarParticipacion(participacionTemp);
        }
        private void MenuFlyoutItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            Participacion participacionTemp = (sender as MenuFlyoutItem).DataContext as Participacion;
            MainWindow mainWindow = (App.Current as App).m_window as MainWindow;
            
            participacionCollection.DeleteParticipacionById(participacionTemp.Id);
        }
    }

    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int number)
            {
                // Crear una cultura personalizada con el separador de miles como punto
                var cultureInfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                cultureInfo.NumberFormat.NumberGroupSeparator = ".";

                // Formatear el número con separadores de miles usando la cultura personalizada
                return number.ToString("N0", cultureInfo);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Omitir conversión hacia atrás si no es necesaria
            return value;
        }
    }
}
