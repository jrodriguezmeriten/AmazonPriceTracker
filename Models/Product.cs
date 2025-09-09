namespace AmazonPriceTracker.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; } // El '?' lo hace nullable
        public double? TargetPrice { get; set; } // Precio objetivo para la alerta
        public bool IsTracking { get; set; } // Si está activo el rastreo
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Este método ayuda a mostrar info en consola
        public override string ToString()
        {
            return $"#{Id} - {Name} (Rastreo: {(IsTracking ? "ON" : "OFF")})";
        }
    }
}