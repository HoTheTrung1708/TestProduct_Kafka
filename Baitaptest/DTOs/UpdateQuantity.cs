namespace Baitaptest.DTOs
{
    public class UpdateQuantity
    {
        public int   ProductId { get; set; }
        public int   Quantity { get; set; }
        public bool  Increase  { get; set; }
        public decimal Price { get; set; }

    }
}
