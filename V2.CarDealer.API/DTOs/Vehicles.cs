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
        
        public class Image
        {
            public int id { get; set; }
            public int Vehicle_Id { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
