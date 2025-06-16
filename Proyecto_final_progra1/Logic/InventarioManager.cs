// --------------------------------------------------------
// Author: Ricardo Márquez
// GitHub: https://github.com/Ricardo21Josee
// LinkedIn: https://www.linkedin.com/in/ric21marquez
// Instagram: @mar_quez_g
// Threads: @mar_quez_g
// Email: josemarquez21garcia@gmail.com
// --------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Proyecto_final_progra1.Data;
using Proyecto_final_progra1.Models;

namespace Proyecto_final_progra1.Logic
{
    internal class InventarioManager
    {
        public List<Producto> ObtenerInventario()
        {
            var productos = new List<Producto>();
            using (var conn = Database.GetConnection())
            using (var cmd = new NpgsqlCommand("SELECT id, nombre, descripcion, precio, existencia FROM productos_inventario ORDER BY id", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    productos.Add(new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.GetString(2),
                        Precio = reader.GetDouble(3),
                        Existencia = reader.GetInt32(4)
                    });
                }
            }
            return productos;
        }

        public Producto ObtenerProductoPorId(int id)
        {
            using (var conn = Database.GetConnection())
            using (var cmd = new NpgsqlCommand("SELECT id, nombre, descripcion, precio, existencia FROM productos_inventario WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Producto
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Descripcion = reader.GetString(2),
                            Precio = reader.GetDouble(3),
                            Existencia = reader.GetInt32(4)
                        };
                    }
                }
            }
            return null;
        }

        public bool AgregarProducto(Producto producto)
        {
            using (var conn = Database.GetConnection())
            using (var cmd = new NpgsqlCommand("INSERT INTO productos_inventario (nombre, descripcion, precio, existencia) VALUES (@nombre, @descripcion, @precio, @existencia)", conn))
            {
                cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                cmd.Parameters.AddWithValue("@precio", producto.Precio);
                cmd.Parameters.AddWithValue("@existencia", producto.Existencia);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ActualizarExistencias(int productoId, int nuevaExistencia)
        {
            using (var conn = Database.GetConnection())
            using (var cmd = new NpgsqlCommand("UPDATE productos_inventario SET existencia = @existencia WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("@existencia", nuevaExistencia);
                cmd.Parameters.AddWithValue("@id", productoId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ReducirExistencias(int productoId, int cantidad)
        {
            using (var conn = Database.GetConnection())
            {
                // Verificar existencia actual
                int existenciaActual = 0;
                using (var cmd = new NpgsqlCommand("SELECT existencia FROM productos_inventario WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", productoId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            existenciaActual = reader.GetInt32(0);
                        }
                        else
                        {
                            return false; // Producto no encontrado
                        }
                    }
                }

                if (existenciaActual < cantidad)
                    return false; // No hay suficiente existencia

                // Reducir existencia
                using (var cmd = new NpgsqlCommand("UPDATE productos_inventario SET existencia = existencia - @cantidad WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@id", productoId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
