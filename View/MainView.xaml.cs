using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheHouseLogin.View;
using TheHouseLogin.Model;
using TheHouseLogin.ViewModel;
using TheHouseLogin.Repositories;

namespace TheHouseLogin.View
{
    /// <summary>
    /// Lógica de interacción para MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Permitir mover la ventana con el mouse desde la barra
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try
                {
                    this.DragMove();
                }
                catch (InvalidOperationException)
                {
                    // Solo ignora en caso de haber un excepción
                }
            }

        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ProductoWindow
            {
                Owner = this
            };

            if (ventana.ShowDialog() == true)
            {
                var nuevoProducto = ventana.Producto;

                // Agregalo a la lista de productos (asegurate que sea ObservableCollection<Producto>)
                var viewModel = DataContext as MainViewModel;
                if (viewModel != null && nuevoProducto != null)
                {
                    viewModel.Productos.Add(nuevoProducto);
                }
            }
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;

            if (viewModel?.ProductoSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un producto para editar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var ventana = new ProductoWindow(viewModel.ProductoSeleccionado)
            {
                Owner = this
            };

            if (ventana.ShowDialog() == true)
            {
                try
                {
                    ProductoRepository.ActualizarProducto(ventana.Producto);
                    // No hace falta refrescar la lista si ya estás editando la instancia seleccionada
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar en la base de datos: " + ex.Message,
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;

            if (viewModel?.ProductoSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un producto para eliminar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var resultado = MessageBox.Show(
                $"¿Estás seguro de que querés eliminar el producto '{viewModel.ProductoSeleccionado.Nombre}'?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                try
                {
                    ProductoRepository.EliminarProducto(viewModel.ProductoSeleccionado.Id);
                    viewModel.Productos.Remove(viewModel.ProductoSeleccionado);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnVender_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.ProductoSeleccionado != null)
            {
                vm.Ventas.Add(vm.ProductoSeleccionado);
                vm.OnPropertyChanged(nameof(vm.TotalVentas));
                vm.OnPropertyChanged(nameof(vm.TotalCigarrillos));
            }
            else
            {
                MessageBox.Show("Selecciona un producto para vender.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
