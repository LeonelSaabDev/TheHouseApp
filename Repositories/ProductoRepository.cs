using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TheHouseLogin.Model;

namespace TheHouseLogin.Repositories
{
    public static class ProductoRepository
    {
        private static string connectionString = "Data Source=DESKTOP-VSHD1IR\\MSSQLSERVERLIO;Initial Catalog=TheHouse_DB;Integrated Security=True;Encrypt=False"; // Usualmente va en App.config

        public static void AgregarProducto(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO dbo.Productos (nombre, precio, stock)
                         VALUES (@nombre, @precio, @stock);
                         SELECT SCOPE_IDENTITY();"; // Esto devuelve el ID generado

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.Parameters.AddWithValue("@stock", producto.Stock);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int idGenerado))
                    {
                        producto.Id = idGenerado; // Guardamos el ID generado por SQL Server
                    }
                }
            }
        }
        public static void ActualizarProducto(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE dbo.Productos
                         SET nombre = @nombre, precio = @precio, stock = @stock
                         WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.Parameters.AddWithValue("@stock", producto.Stock);
                    command.Parameters.AddWithValue("@id", producto.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void EliminarProducto(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM dbo.Productos WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT id, nombre, precio, stock FROM dbo.Productos";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var producto = new Producto
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            Nombre = reader["nombre"] != DBNull.Value ? reader["nombre"].ToString() : "",
                            Precio = reader["precio"] != DBNull.Value ? Convert.ToDecimal(reader["precio"]) : 0,
                            Stock = reader["stock"] != DBNull.Value ? Convert.ToInt32(reader["stock"]) : 0
                        };

                        lista.Add(producto);
                    }
                }
            }

            return lista;
        }
    }
}
