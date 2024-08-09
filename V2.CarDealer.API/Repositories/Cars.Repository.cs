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
                    string query = @"
                        select
                            vehicles.id,
                            vehicles.type_id,
                            vehicles.brand,
                            vehicles.model,
                            vehicles.year,
                            vehicles.price,
                            vehicles.mileage,
                            vehicles.engine,
                            vehicles.horsepower,
                            vehicles.sold,
                            vehicle_types.type,
                            images.id as ImageId,
                            images.vehicle_id,
                            images.imageUrl
                        from 
                            vehicles 
                        inner join 
                            vehicle_types on vehicles.type_id = vehicle_types.id
                        left join
                            images on vehicles.id = images.vehicle_id;
                    ";

                    var vehicleDictionary = new Dictionary<int, Vehicle>();

                    var result = connection.Query<Vehicle, Vehicle.Image, Vehicle>(
                        query, (vehicle, image) => {
                    
                            if (!vehicleDictionary.TryGetValue(vehicle.Id, out var currentVehicle))
                            {
                                currentVehicle = vehicle;
                                currentVehicle.Images = new List<Vehicle.Image>();
                                vehicleDictionary.Add(currentVehicle.Id, currentVehicle);
                            }

                            if (image != null)
                            {
                                currentVehicle.Images.Add(image);
                            }

                            return currentVehicle;
                        },
                        splitOn: "ImageId").Distinct().ToList();

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
                    string query = @"
                    SELECT 
                        v.id,
                        v.type_id,
                        v.brand,
                        v.model,
                        v.year,
                        v.price,
                        v.mileage,
                        v.engine,
                        v.horsepower,
                        v.sold,
                        vt.type,
                        images.id as ImageId,
                        images.vehicle_id,
                        images.imageUrl
                    FROM 
                        vehicles v 
                    INNER JOIN 
                        vehicle_types vt 
                        ON v.type_id = vt.id 
                    LEFT JOIN 
                        images 
                        ON v.id = images.vehicle_id
                    WHERE 
                        vt.id = @typeId";

                    var vehicleDictionary = new Dictionary<int, Vehicle>();

                    var result = connection.Query<Vehicle, Vehicle.Image, Vehicle>(
                        query,
                        (vehicle, image) =>
                        {
                            if (!vehicleDictionary.TryGetValue(vehicle.Id, out var currentVehicle))
                            {
                                currentVehicle = vehicle;
                                currentVehicle.Images = new List<Vehicle.Image>();
                                vehicleDictionary.Add(currentVehicle.Id, currentVehicle);
                            }

                            if (image != null)
                            {
                                currentVehicle.Images.Add(image);
                            }

                            return currentVehicle;
                        },
                        new { typeId },
                        splitOn: "ImageId"
                    ).Distinct().ToList();

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
