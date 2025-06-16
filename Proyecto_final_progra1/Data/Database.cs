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

namespace Proyecto_final_progra1.Data
{
    public static class Database
    {
        private const string ConnectionString = "Host=localhost;Port=5432;Database=Tienda_Facturacion_DB;Username=ricardo;Password=ricgio921";

        public static NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}
