using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TheHouseLogin.Model;
using TheHouseLogin.Repositories;
using System.Globalization;
using System.Windows.Data;
using TheHouseLogin.Helpers;

namespace TheHouseLogin.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Producto> Productos { get; set; }
        public ICollectionView ProductosFiltrados { get; set; }
        public ObservableCollection<Producto> Ventas { get; set; }
        public ICommand LimpiarVentasCommand { get; }
        public decimal TotalVentas => Ventas.Sum(p => p.Precio);

        public decimal TotalCigarrillos => Ventas
            .Where(p => p.Nombre.ToLower().Contains("cigarro") || p.Nombre.ToLower().Contains("cigarrillo"))
            .Sum(p => p.Precio);

        private Producto _productoSeleccionado;
        public Producto ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                _productoSeleccionado = value;
                OnPropertyChanged(nameof(ProductoSeleccionado));
            }
        }
        public ICommand BorrarVentaCommand { get; }

        private Producto _ventaSeleccionada;
        public Producto VentaSeleccionada
        {
            get => _ventaSeleccionada;
            set
            {
                _ventaSeleccionada = value;
                OnPropertyChanged(nameof(VentaSeleccionada));
                (BorrarVentaCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _filtroBusqueda;
        public string FiltroBusqueda
        {
            get => _filtroBusqueda;
            set
            {
                _filtroBusqueda = value;
                OnPropertyChanged(nameof(FiltroBusqueda));
                ProductosFiltrados.Refresh(); // Filtrar en tiempo real
            }
        }

        public MainViewModel()
        {
            var listaDesdeDB = ProductoRepository.ObtenerTodos();
            Productos = new ObservableCollection<Producto>(listaDesdeDB);

            // Inicializar la vista filtrada
            ProductosFiltrados = CollectionViewSource.GetDefaultView(Productos);
            ProductosFiltrados.Filter = FiltrarProductos;

            // Inicializar lista de ventas VACÍA
            Ventas = new ObservableCollection<Producto>();

            // Inicializar comando LIMPIAR
            LimpiarVentasCommand = new RelayCommand(LimpiarVentas);

            BorrarVentaCommand = new RelayCommand(BorrarVenta, PuedeBorrarVenta);
        }

        private bool FiltrarProductos(object obj)
        {
            if (obj is Producto producto)
            {
                if (string.IsNullOrWhiteSpace(FiltroBusqueda))
                    return true;

                string filtro = FiltroBusqueda.ToLower();

                // Si el filtro se puede convertir a decimal, comparamos numéricamente
                if (decimal.TryParse(FiltroBusqueda, out decimal filtroDecimal))
                {
                    if (producto.Precio == filtroDecimal)
                        return true;
                }

                // Si no coincide precio exacto, busca nombre o id
                return producto.Nombre.ToLower().Contains(filtro)
                       || producto.Id.ToString().Contains(filtro);
            }

            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string nombre)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        private void LimpiarVentas()
        {
            Ventas.Clear();
            OnPropertyChanged(nameof(TotalVentas));
            OnPropertyChanged(nameof(TotalCigarrillos));
        }

        private void BorrarVenta()
        {
            if (VentaSeleccionada != null)
            {
                Ventas.Remove(VentaSeleccionada);
                OnPropertyChanged(nameof(TotalVentas));
                OnPropertyChanged(nameof(TotalCigarrillos));
            }
        }

        private bool PuedeBorrarVenta()
        {
            return VentaSeleccionada != null;
        }

    }
}