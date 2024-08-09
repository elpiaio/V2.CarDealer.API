namespace V2.CarDealer.API.DTOs
{
    public class Sale
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public DateTime SaleDate { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
    }

    public class Sales
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public DateTime SaleDate { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public User user { get; set; }  
    }
}
