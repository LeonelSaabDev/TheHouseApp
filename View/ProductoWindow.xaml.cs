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
using TheHouseLogin.Model;
using TheHouseLogin.Repositories;

namespace TheHouseLogin.View
{
    /// <summary>
    /// Lógica de interacción para ProductoWindow.xaml
    /// </summary>
    public partial class ProductoWindow : Window
    {
        public Producto Producto { get; set; }

        public ProductoWindow()
        {
            InitializeComponent();
        }
        public ProductoWindow(Producto productoExistente) : this()
        {
            txtNombre.Text = productoExistente.Nombre;
            txtPrecio.Text = productoExistente.Precio.ToString();
            txtStock.Text = productoExistente.Stock.ToString();

            Producto = productoExistente;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(txtPrecio.Text, out decimal precio) &&
                int.TryParse(txtStock.Text, out int stock) &&
                !string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                // Si el Producto es null, lo instanciamos (modo Agregar)
                if (Producto == null)
                    Producto = new Producto();
                // Asignar los valores al producto existente (referencia)
                Producto.Nombre = txtNombre.Text;
                Producto.Precio = precio;
                Producto.Stock = stock;

                try
                {
                    if (Producto.Id == 0)
                    {
                        // Agregar nuevo
                        ProductoRepository.AgregarProducto(Producto);
                    }
                    else
                    {
                        // Editar existente
                        ProductoRepository.ActualizarProducto(Producto);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar en la base de datos: " + ex.Message,
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Datos inválidos. Completar todos los campos.");
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
