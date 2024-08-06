using static System.Net.Mime.MediaTypeNames;

namespace V2.CarDealer.API.DTOs
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int Type_id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime Year { get; set; }
        public decimal Price { get; set; }
        public int Mileage { get; set; }
        public string Engine { get; set; }
        public string Horsepower { get; set; }
        public bool Sold { get; set; }
        public string Type { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Sale> Sales { get; set; }

        public class Image
        {
            public int Id { get; set; }
            public int VehicleId { get; set; }
            public string ImageUrl { get; set; }
            public Vehicle Vehicle { get; set; }
        }

        public class Sale
        {
            public int Id { get; set; }
            public int VehicleId { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public decimal Price { get; set; }
            public DateTime SaleDate { get; set; }

            // Navigation property
            public Vehicle Vehicle { get; set; }
        }
    }
}
