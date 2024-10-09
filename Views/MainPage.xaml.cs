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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GT_AdminDB.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MiembroCollection miembroCollection = new MiembroCollection();
        List<Miembro> miembrosRemovidos = new List<Miembro>();

        bool orderByLogin = false;
        public MainPage()
        {
            this.InitializeComponent();
            miembroCollection.GetMiembros();
            lvwMiembros.ItemsSource = miembroCollection.Miembros;
        }
        //METODOS DE PAGINA
        private void Grid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Grid control = sender as Grid;
            Miembro miembroTemp = control.DataContext as Miembro;

            Frame.Navigate(typeof(ViewParticipacion));
            var frameActual = Frame.Content as ViewParticipacion;
            frameActual.CargarLista(miembroTemp);
        }

        private void btnCheckLogin_Click(object sender, RoutedEventArgs e)
        {
            if (lvwMiembros.SelectedItems.Count() != 0)
            {
                foreach (Miembro miembroSelect in lvwMiembros.SelectedItems)
                {
                    miembroSelect.Ultimo_Login = DateOnly.FromDateTime(DateTime.Now);
                }
                miembroCollection.ActualizarLogin(lvwMiembros.SelectedItems.Cast<Miembro>().ToList());
            }
        }
        //FILTROS
        public void OnFilterChanged(string textoRecibido)
        {
            if (textoRecibido == "")
            {
                miembrosRemovidos.Clear();
                miembroCollection.GetMiembros(orderByLogin);
            }
            else
            {
                List<Miembro> sumListas = miembroCollection.Miembros.ToList();
                for (int i = miembrosRemovidos.Count() - 1; i >= 0; i--)
                {
                    sumListas.Add(miembrosRemovidos[i]);
                }
                var filtered = sumListas.Where(miembro => Filter(miembro, textoRecibido));
                Remove_NonMatching(filtered);
                AddBack_Contacts(filtered);
            }
        }
        private bool Filter(Miembro miembroTemp, string textoRecibido)
        {
            return miembroTemp.Nombre.Contains(textoRecibido, StringComparison.InvariantCultureIgnoreCase);
        }
        private void Remove_NonMatching(IEnumerable<Miembro> filteredData)
        {
            for (int i = miembroCollection.Miembros.Count - 1; i >= 0; i--)
            {
                Miembro miembroActual = miembroCollection.Miembros[i];
                // If contact is not in the filtered argument list, remove it from the ListView's source.
                if (!filteredData.Contains(miembroActual))
                {
                    miembrosRemovidos.Add(miembroActual);
                    miembroCollection.Miembros.Remove(miembroActual);
                }
            }
        }
        private void AddBack_Contacts(IEnumerable<Miembro> filteredData)
        {
            foreach (var item in filteredData)
            {
                // If item in filtered list is not currently in ListView's source collection, add it back in
                if (!miembroCollection.Miembros.Contains(item))
                {
                    miembrosRemovidos.Remove(item);
                    miembroCollection.Miembros.Add(item);
                }
            }
        }
        public void OrderByLogin(bool check)
        {
            miembrosRemovidos.Clear();
            orderByLogin = check;
            miembroCollection.GetMiembros(orderByLogin);
        }
        //METODOS TRANSITORIOS
        public void AgregarMiembro(Miembro miembroTemp)
        {
            int ultimoId = 0;
            if (miembroCollection.Miembros.Count > 0)
            {
                ultimoId = miembroCollection.Miembros.Max(x => x.Id);
                miembroTemp.Id = ultimoId + 1;
            }
            else
            {
                miembroTemp.Id = ultimoId;
            }
            bool error = miembroCollection.Add(miembroTemp);
            switch (error)
            {
                case true:
                    ((App.Current as App).m_window as MainWindow).InfoResultado(2,"Error al crear miembro");
                    break;
                case false:
                    ((App.Current as App).m_window as MainWindow).InfoResultado(1,"Miembro creado");
                    break;
            }
        }

        public void EditarMiembro(Miembro miembroRecibido)
        {
            miembroCollection.Update(miembroRecibido);
            miembroCollection.Miembros.Where(x => x.Id == miembroRecibido.Id);
        }

        private void MenuFlyoutItemEditar_Click(object sender, RoutedEventArgs e)
        {
            Miembro miembroEditar = (sender as MenuFlyoutItem).DataContext as Miembro;
            MainWindow mainWindow = (App.Current as App).m_window as MainWindow;
            mainWindow.EditarMiembro(miembroEditar);
        }

        private void MenuFlyoutItemEliminar_Click(object sender, RoutedEventArgs e)
        {
            int idMiembro = ((sender as MenuFlyoutItem).DataContext as Miembro).Id;
            miembroCollection.DeleteById(idMiembro);
        }

        public void ModoSeleccion(ListViewSelectionMode seleccion)
        {
            lvwMiembros.SelectionMode = seleccion;
            if (seleccion == ListViewSelectionMode.Multiple)
            {
                btnEliminar.Visibility = Visibility.Visible;
            }
            else
            {
                btnEliminar.Visibility = Visibility.Collapsed;
            }
        }
    }
    public class FechaIngresoADiasConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateOnly fechaIngreso)
            {
                // Calcular los días entre la fecha de ingreso y hoy
                DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
                int resultado = fechaActual.DayNumber - fechaIngreso.DayNumber;
                return "LogOut Days: "+resultado;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
