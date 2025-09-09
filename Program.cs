using AmazonPriceTracker.API.Database;
using AmazonPriceTracker.API.Models;

namespace AmazonPriceTracker.API
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🧪 Probando la base de datos...\n");

            // Crear una instancia del administrador de la BD (esto creará las tablas automáticamente)
            var dbManager = new SqliteDatabaseManager();

            // Crear un producto de prueba
            var testProduct = new Product
            {
                Name = "Echo Dot (4ª generación)",
                Url = "https://www.amazon.es/dp/B08H95Y452",
                TargetPrice = 49.99,
                IsTracking = true
            };

            // Añadir el producto a la base de datos
            Console.WriteLine("➕ Añadiendo producto de prueba...");
            dbManager.AddProduct(testProduct);
            Console.WriteLine("✅ Producto añadido.");

            // Recuperar todos los productos de la BD
            Console.WriteLine("\n📋 Listando todos los productos:");
            var allProducts = dbManager.GetAllProducts();

            if (allProducts.Count == 0)
            {
                Console.WriteLine("No hay productos en la base de datos.");
            }
            else
            {
                foreach (var product in allProducts)
                {
                    Console.WriteLine($" - {product}");
                }
            }

            // Simular que hemos rastreado un precio y añadirlo al histórico
            Console.WriteLine("\n💾 Simulando adición de precio al histórico...");
            if (allProducts.Count > 0)
            {
                var firstProduct = allProducts[0];
                var newPriceEntry = new PriceHistory
                {
                    ProductId = firstProduct.Id,
                    Price = 47.99
                };
                dbManager.AddPriceHistory(newPriceEntry);
                Console.WriteLine($"✅ Precio de {newPriceEntry.Price}€ añadido para el producto #{firstProduct.Id}.");
            }

            Console.WriteLine("\n🎉 ¡Prueba de base de datos completada con éxito!");
            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}