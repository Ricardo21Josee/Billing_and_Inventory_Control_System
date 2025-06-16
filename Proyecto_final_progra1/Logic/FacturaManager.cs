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
using Npgsql;
using Proyecto_final_progra1.Data;
using Proyecto_final_progra1.Models;

namespace Proyecto_final_progra1.Logic
{
    internal class FacturaManager
    {
        public bool GuardarFactura(Factura factura, string nombreCliente, string nit = "", string direccion = "", string dpi = "")
        {
            using (var conn = Database.GetConnection())
            using (var tran = conn.BeginTransaction())
            {
                try
                {
                    int clienteId = 0;
                    int cfClienteId = 0;

                    if (factura.RequiereNit)
                    {
                        // Insertar cliente con NIT
                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO clientes (nombre, apellido, direccion, dpi, nit) VALUES (@nombre, '', @direccion, @dpi, @nit) RETURNING id", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@nombre", nombreCliente);
                            cmd.Parameters.AddWithValue("@direccion", direccion);
                            cmd.Parameters.AddWithValue("@dpi", dpi);
                            cmd.Parameters.AddWithValue("@nit", nit);
                            clienteId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                    else
                    {
                        // Insertar consumidor final
                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO clientes_cf (nombre, direccion) VALUES (@nombre, @direccion) RETURNING id", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@nombre", nombreCliente);
                            cmd.Parameters.AddWithValue("@direccion", direccion);
                            cfClienteId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }

                    // Insertar factura
                    int facturaId;
                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO facturas (nombre_empresa, direccion_empresa, nit_empresa, codigo_factura, cliente_id, cf_cliente_id, subtotal_sin_iva, subtotal, iva, total, requiere_nit) " +
                        "VALUES (@nombreEmpresa, @direccionEmpresa, @nitEmpresa, @codigo, @clienteId, @cfClienteId, @subtotalSinIva, @subtotal, @iva, @total, @requiereNit) RETURNING factura_id",
                        conn, tran))
                    {
                        cmd.Parameters.AddWithValue("@nombreEmpresa", factura.NombreEmpresa);
                        cmd.Parameters.AddWithValue("@direccionEmpresa", factura.DireccionEmpresa);
                        cmd.Parameters.AddWithValue("@nitEmpresa", factura.NitEmpresa);
                        cmd.Parameters.AddWithValue("@codigo", factura.Codigo);
                        cmd.Parameters.AddWithValue("@clienteId", clienteId > 0 ? (object)clienteId : DBNull.Value);
                        cmd.Parameters.AddWithValue("@cfClienteId", cfClienteId > 0 ? (object)cfClienteId : DBNull.Value);
                        cmd.Parameters.AddWithValue("@subtotalSinIva", factura.SubtotalSinIva);
                        cmd.Parameters.AddWithValue("@subtotal", factura.Subtotal);
                        cmd.Parameters.AddWithValue("@iva", factura.Iva);
                        cmd.Parameters.AddWithValue("@total", factura.Total);
                        cmd.Parameters.AddWithValue("@requiereNit", factura.RequiereNit);
                        facturaId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Insertar detalles de factura
                    foreach (var producto in factura.Productos)
                    {
                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO pre_detalle_factura (factura_id, producto_id, cantidad, precio_unitario, subtotal) " +
                            "VALUES (@facturaId, @productoId, @cantidad, @precioUnitario, @subtotal)", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@facturaId", facturaId);
                            cmd.Parameters.AddWithValue("@productoId", producto.Id);
                            cmd.Parameters.AddWithValue("@cantidad", producto.Existencia);
                            cmd.Parameters.AddWithValue("@precioUnitario", producto.Precio);
                            cmd.Parameters.AddWithValue("@subtotal", producto.Precio * producto.Existencia);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                }
            }
        }

        public Factura ObtenerFacturaPorId(int facturaId)
        {
            using (var conn = Database.GetConnection())
            {
                Factura factura = null;
                using (var cmd = new NpgsqlCommand(
                    "SELECT factura_id, nombre_empresa, direccion_empresa, nit_empresa, codigo_factura, cliente_id, cf_cliente_id, subtotal_sin_iva, subtotal, iva, total, requiere_nit " +
                    "FROM facturas WHERE factura_id = @facturaId", conn))
                {
                    cmd.Parameters.AddWithValue("@facturaId", facturaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            factura = new Factura
                            {
                                Id = reader.GetInt32(0),
                                NombreEmpresa = reader.GetString(1),
                                DireccionEmpresa = reader.GetString(2),
                                NitEmpresa = reader.GetString(3),
                                Codigo = reader.GetString(4),
                                ClienteId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                                CfClienteId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                                SubtotalSinIva = reader.GetDouble(7),
                                Subtotal = reader.GetDouble(8),
                                Iva = reader.GetDouble(9),
                                Total = reader.GetDouble(10),
                                RequiereNit = reader.GetBoolean(11),
                                Productos = new List<Producto>()
                            };
                        }
                    }
                }

                if (factura != null)
                {
                    // Obtener productos de la factura
                    using (var cmd = new NpgsqlCommand(
                        "SELECT producto_id, cantidad, precio_unitario FROM pre_detalle_factura WHERE factura_id = @facturaId", conn))
                    {
                        cmd.Parameters.AddWithValue("@facturaId", factura.Id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                factura.Productos.Add(new Producto
                                {
                                    Id = reader.GetInt32(0),
                                    Existencia = reader.GetInt32(1),
                                    Precio = reader.GetDouble(2)
                                });
                            }
                        }
                    }
                }

                return factura;
            }
        }

        public List<Factura> ObtenerFacturas()
        {
            var facturas = new List<Factura>();
            using (var conn = Database.GetConnection())
            using (var cmd = new NpgsqlCommand(
                "SELECT factura_id, nombre_empresa, direccion_empresa, nit_empresa, codigo_factura, cliente_id, cf_cliente_id, subtotal_sin_iva, subtotal, iva, total, requiere_nit FROM facturas", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    facturas.Add(new Factura
                    {
                        Id = reader.GetInt32(0),
                        NombreEmpresa = reader.GetString(1),
                        DireccionEmpresa = reader.GetString(2),
                        NitEmpresa = reader.GetString(3),
                        Codigo = reader.GetString(4),
                        ClienteId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        CfClienteId = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        SubtotalSinIva = reader.GetDouble(7),
                        Subtotal = reader.GetDouble(8),
                        Iva = reader.GetDouble(9),
                        Total = reader.GetDouble(10),
                        RequiereNit = reader.GetBoolean(11),
                        Productos = new List<Producto>()
                    });
                }
            }
            return facturas;
        }
    }
}
