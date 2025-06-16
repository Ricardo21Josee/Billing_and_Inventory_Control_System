<h1 align="center">Documentación del Sistema de Facturación e Inventario</h1>

<h2>📌 Descripción General</h2>
<p>Este proyecto es un sistema de facturación e inventario desarrollado en C# con conexión a una base de datos PostgreSQL. Proporciona funcionalidades para gestionar productos, clientes y facturas.</p>

<h2>📚 Estructura del Proyecto</h2>
<ul>
    <li><strong>Data</strong>: Contiene la conexión a la base de datos</li>
    <li><strong>Logic</strong>: Maneja la lógica de negocio (facturas e inventario)</li>
    <li><strong>Models</strong>: Define los modelos de datos</li>
</ul>

<h2>🔗 Diagrama de Clases</h2>
<pre>
+-------------------+       +-------------------+       +-------------------+
|     Factura       |       |     Producto      |       |     Cliente       |
+-------------------+       +-------------------+       +-------------------+
| - Id: int         |       | - Id: int         |       | - Id: int         |
| - Codigo: string  |       | - Nombre: string  |       | - Nombre: string  |
| - NombreEmpresa   |       | - Descripcion     |       | - Apellido: string|
| - DireccionEmpresa|       | - Precio: double  |       | - Direccion       |
| - NitEmpresa      |       | - Existencia: int |       | - Dpi: string     |
| - ClienteId       |       +-------------------+       | - Nit: string     |
| - CfClienteId     |                                  +-------------------+
| - SubtotalSinIva  |
| - Subtotal        |       +-------------------+
| - Iva             |       |    ClienteCF      |
| - Total           |       +-------------------+
| - RequiereNit     |       | - Id: int         |
| - Productos       |       | - Nombre: string  |
+-------------------+       | - Direccion       |
                            +-------------------+
</pre>

<h2>📂 Documentación de Clases</h2>

<h3>1. Database.cs</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Data<br>
<strong>Propósito:</strong> Proporciona la conexión a la base de datos PostgreSQL.<br>
<strong>Métodos:</strong>
<ul>
    <li><code>public static NpgsqlConnection GetConnection()</code>: Obtiene una conexión abierta a la base de datos.</li>
</ul>
<strong>Conexión:</strong> Utiliza el connection string "Host=localhost; Port=5432; Database=Tienda_Facturacion_DB; Username=******; Password=******"
</div>

<h3>2. FacturaManager.cs</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Logic<br>
<strong>Propósito:</strong> Gestiona las operaciones relacionadas con facturas.<br>
<strong>Métodos:</strong>
<ul>
    <li><code>public bool GuardarFactura(Factura factura, string nombreCliente, string nit = "", string direccion = "", string dpi = "")</code>: Guarda una factura en la base de datos, incluyendo cliente y productos.</li>
    <li><code>public Factura ObtenerFacturaPorId(int facturaId)</code>: Obtiene una factura específica por su ID.</li>
    <li><code>public List&lt;Factura&gt; ObtenerFacturas()</code>: Obtiene todas las facturas registradas.</li>
</ul>
</div>

<h3>3. InventarioManager.cs</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Logic<br>
<strong>Propósito:</strong> Gestiona las operaciones del inventario de productos.<br>
<strong>Métodos:</strong>
<ul>
    <li><code>public List&lt;Producto&gt; ObtenerInventario()</code>: Obtiene todos los productos del inventario.</li>
    <li><code>public Producto ObtenerProductoPorId(int id)</code>: Obtiene un producto específico por su ID.</li>
    <li><code>public bool AgregarProducto(Producto producto)</code>: Agrega un nuevo producto al inventario.</li>
    <li><code>public bool ActualizarExistencias(int productoId, int nuevaExistencia)</code>: Actualiza las existencias de un producto.</li>
    <li><code>public bool ReducirExistencias(int productoId, int cantidad)</code>: Reduce las existencias de un producto.</li>
</ul>
</div>

<h3>4. Modelos (Clases)</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Models<br>
<strong>Clases:</strong>
<ul>
    <li><strong>Cliente</strong>: Representa un cliente con NIT.
        <ul>
            <li>Propiedades: Id, Nombre, Apellido, Direccion, Dpi, Nit</li>
        </ul>
    </li>
    <li><strong>ClienteCF</strong>: Representa un consumidor final.
        <ul>
            <li>Propiedades: Id, Nombre, Direccion</li>
        </ul>
    </li>
    <li><strong>Factura</strong>: Representa una factura.
        <ul>
            <li>Propiedades: Id, Codigo, NombreEmpresa, DireccionEmpresa, NitEmpresa, ClienteId, CfClienteId, SubtotalSinIva, Subtotal, Iva, Total, RequiereNit, Productos (lista)</li>
        </ul>
    </li>
    <li><strong>Producto</strong>: Representa un producto en el inventario.
        <ul>
            <li>Propiedades: Id, Nombre, Descripcion, Precio, Existencia</li>
        </ul>
    </li>
</ul>
</div>

<h2>⚙️ Configuración</h2>
<ol>
    <li>Configurar la cadena de conexión en <code>Database.cs</code></li>
    <li>Asegurarse de tener PostgreSQL instalado y la base de datos creada</li>
    <li>Ejecutar los scripts SQL necesarios para crear las tablas</li>
</ol>

<h2>📝 Ejemplo de Uso</h2>
<pre><code>// Crear una nueva factura
var factura = new Factura {
    Codigo = "FAC-001",
    NombreEmpresa = "Mi Empresa",
    // ... otras propiedades
};

var manager = new FacturaManager();
bool resultado = manager.GuardarFactura(factura, "Cliente Ejemplo", "12345678");</code></pre>

<h2>🛠 Dependencias</h2>
<ul>
    <li>.NET Core</li>
    <li>Npgsql (PostgreSQL connector)</li>
</ul>

<h2>👨‍💻 Autor</h2>
<p>Ricardo Márquez - <a href="mailto:josemarquez21garcia@gmail.com">josemarquez21garcia@gmail.com</a></p>
<p>GitHub: <a href="https://github.com/Ricardo21Josee">Ricardo21Josee</a></p>
