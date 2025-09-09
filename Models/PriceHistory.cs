namespace AmazonPriceTracker.API.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Relación con el Producto
        public double Price { get; set; }
        public DateTime CheckDate { get; set; } = DateTime.Now;
    }
}