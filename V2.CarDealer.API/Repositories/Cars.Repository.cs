using Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using V2.CarDealer.API.DTOs;

namespace V2.CarDealer.API.CarsRepository
{
    public class Cars
    {
        public static List<Vehicle> GetAllCars()
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = "select * from vehicles";
                    List<Vehicle> result = connection.Query<Vehicle>(query).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }

        public static List<Vehicle> GetByType(int typeId)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = "select v.*, vt.type from vehicles v inner join vehicle_types vt on v.type_id = vt.id where vt.id = @typeId";
                    List<Vehicle> result = connection.Query<Vehicle>(query, new { typeId }).ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
