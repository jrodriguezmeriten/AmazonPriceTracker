using Dapper;
using System.Data.SQLite;
using AmazonPriceTracker.API.Models;
using System.Linq;

namespace AmazonPriceTracker.API.Database
{
    public class SqliteDatabaseManager
    {
        private readonly string _connectionString;

        public SqliteDatabaseManager()
        {
            // La base de datos se guardará en un archivo .db en la carpeta del programa
            _connectionString = "Data Source=AmazonPriceTracker.db;Version=3;";
            InitializeDatabase(); // Aseguramos que las tablas existan al crearse el Manager
        }

        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // SQL para crear la tabla de Productos si no existe
                string createProductTableSql = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Url TEXT NOT NULL UNIQUE,
                        ImageUrl TEXT,
                        TargetPrice REAL,
                        IsTracking BOOLEAN DEFAULT 1,
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                // SQL para crear la tabla de Historial de Precios si no existe
                string createPriceHistoryTableSql = @"
                    CREATE TABLE IF NOT EXISTS PriceHistory (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductId INTEGER NOT NULL,
                        Price REAL NOT NULL,
                        CheckDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (ProductId) REFERENCES Products (Id) ON DELETE CASCADE
                    )";

                // Ejecutar los comandos SQL
                connection.Execute(createProductTableSql);
                connection.Execute(createPriceHistoryTableSql);
            }
        }

        // --- MÉTODOS PARA PRODUCTOS ---

        public void AddProduct(Product product)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO Products (Name, Url, ImageUrl, TargetPrice, IsTracking)
                    VALUES (@Name, @Url, @ImageUrl, @TargetPrice, @IsTracking)";
                connection.Execute(sql, product);
            }
        }

        public List<Product> GetAllProducts()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = "SELECT * FROM Products ORDER BY Id DESC";
                return connection.Query<Product>(sql).ToList();
            }
        }

        // --- MÉTODOS PARA HISTORIAL DE PRECIOS ---

        public void AddPriceHistory(PriceHistory history)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO PriceHistory (ProductId, Price)
                    VALUES (@ProductId, @Price)";
                connection.Execute(sql, history);
            }
        }

        public List<PriceHistory> GetPriceHistoryForProduct(int productId)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = "SELECT * FROM PriceHistory WHERE ProductId = @productId ORDER BY CheckDate ASC";
                return connection.Query<PriceHistory>(sql, new { productId }).ToList();
            }
        }
    }
}