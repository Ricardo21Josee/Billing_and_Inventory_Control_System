<h1 align="center">Invoice and Inventory System Documentation</h1>

<h2>📌 Overview</h2>
<p>This project is an invoice and inventory management system developed in C# with PostgreSQL database connectivity. It provides functionality to manage products, customers, and invoices.</p>

<h2>📚 Project Structure</h2>
<ul>
    <li><strong>Data</strong>: Contains database connection logic</li>
    <li><strong>Logic</strong>: Handles business logic (invoices and inventory)</li>
    <li><strong>Models</strong>: Defines data models</li>
</ul>

<h2>🔗 Class Diagram</h2>
<pre>
+-------------------+       +-------------------+       +-------------------+
|     Invoice       |       |     Product       |       |     Customer      |
+-------------------+       +-------------------+       +-------------------+
| - Id: int         |       | - Id: int         |       | - Id: int         |
| - Code: string    |       | - Name: string    |       | - Name: string    |
| - CompanyName     |       | - Description     |       | - LastName: string|
| - CompanyAddress  |       | - Price: double   |       | - Address         |
| - CompanyTaxId    |       | - Stock: int      |       | - Dpi: string     |
| - CustomerId      |       +-------------------+       | - TaxId: string   |
| - CfCustomerId    |                                  +-------------------+
| - SubtotalNoTax   |
| - Subtotal        |       +-------------------+
| - Tax             |       |    FinalConsumer  |
| - Total           |       +-------------------+
| - RequiresTaxId   |       | - Id: int         |
| - Products        |       | - Name: string    |
+-------------------+       | - Address         |
                            +-------------------+
</pre>

<h2>📂 Class Documentation</h2>

<h3>1. Database.cs</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Data<br>
<strong>Purpose:</strong> Provides connection to PostgreSQL database.<br>
<strong>Methods:</strong>
<ul>
    <li><code>public static NpgsqlConnection GetConnection()</code>: Gets an open database connection.</li>
</ul>
<strong>Connection:</strong> Uses connection string "Host=localhost; Port=5432; Database=Tienda_Facturacion_DB; Username=*****; Password=*****"
</div>

<h3>2. InvoiceManager.cs</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Logic<br>
<strong>Purpose:</strong> Manages invoice-related operations.<br>
<strong>Methods:</strong>
<ul>
    <li><code>public bool SaveInvoice(Invoice invoice, string customerName, string taxId = "", string address = "", string dpi = "")</code>: Saves an invoice to database including customer and products.</li>
    <li><code>public Invoice GetInvoiceById(int invoiceId)</code>: Retrieves a specific invoice by ID.</li>
    <li><code>public List&lt;Invoice&gt; GetInvoices()</code>: Retrieves all registered invoices.</li>
</ul>
</div>

<h3>3. InventoryManager.cs</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Logic<br>
<strong>Purpose:</strong> Manages product inventory operations.<br>
<strong>Methods:</strong>
<ul>
    <li><code>public List&lt;Product&gt; GetInventory()</code>: Retrieves all inventory products.</li>
    <li><code>public Product GetProductById(int id)</code>: Retrieves a specific product by ID.</li>
    <li><code>public bool AddProduct(Product product)</code>: Adds a new product to inventory.</li>
    <li><code>public bool UpdateStock(int productId, int newStock)</code>: Updates product stock quantity.</li>
    <li><code>public bool ReduceStock(int productId, int quantity)</code>: Reduces product stock quantity.</li>
</ul>
</div>

<h3>4. Models (Classes)</h3>
<div style="background:#f8f9fa;padding:15px;border-radius:5px;">
<strong>Namespace:</strong> Proyecto_final_progra1.Models<br>
<strong>Classes:</strong>
<ul>
    <li><strong>Customer</strong>: Represents a customer with tax ID.
        <ul>
            <li>Properties: Id, Name, LastName, Address, Dpi, TaxId</li>
        </ul>
    </li>
    <li><strong>FinalConsumer</strong>: Represents a final consumer (no tax ID).
        <ul>
            <li>Properties: Id, Name, Address</li>
        </ul>
    </li>
    <li><strong>Invoice</strong>: Represents an invoice.
        <ul>
            <li>Properties: Id, Code, CompanyName, CompanyAddress, CompanyTaxId, CustomerId, CfCustomerId, SubtotalNoTax, Subtotal, Tax, Total, RequiresTaxId, Products (list)</li>
        </ul>
    </li>
    <li><strong>Product</strong>: Represents an inventory product.
        <ul>
            <li>Properties: Id, Name, Description, Price, Stock</li>
        </ul>
    </li>
</ul>
</div>

<h2>⚙️ Configuration</h2>
<ol>
    <li>Configure connection string in <code>Database.cs</code></li>
    <li>Ensure PostgreSQL is installed and database is created</li>
    <li>Run necessary SQL scripts to create tables</li>
</ol>

<h2>📝 Usage Example</h2>
<pre><code>// Create a new invoice
var invoice = new Invoice {
    Code = "INV-001",
    CompanyName = "My Company",
    // ... other properties
};

var manager = new InvoiceManager();
bool result = manager.SaveInvoice(invoice, "Example Customer", "12345678");</code></pre>

<h2>🛠 Dependencies</h2>
<ul>
    <li>.NET Core</li>
    <li>Npgsql (PostgreSQL connector)</li>
</ul>

<h2>👨‍💻 Author</h2>
<p>Ricardo Márquez - <a href="mailto:josemarquez21garcia@gmail.com">josemarquez21garcia@gmail.com</a></p>
<p>GitHub: <a href="https://github.com/Ricardo21Josee">Ricardo21Josee</a></p>
