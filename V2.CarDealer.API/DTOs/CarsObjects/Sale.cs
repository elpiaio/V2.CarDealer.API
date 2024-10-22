using V2.CarDealer.API.DTOs.UsersObjects;

namespace V2.CarDealer.API.DTOs.CarsObjects
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
    public class TotalSales
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal TotalVendas { get; set; }
    }
}
