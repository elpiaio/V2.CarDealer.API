using Dapper;
using System.Data.SqlClient;
using System.ComponentModel;
using V2.CarDealer.API.DTOs.CarsObjects;

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
                        SELECT
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
                            images.id AS ImageId,
                            images.vehicle_id,
                            images.imageUrl
                        FROM vehicles
                        INNER JOIN vehicle_types ON vehicles.type_id = vehicle_types.id
                        LEFT JOIN images ON vehicles.id = images.vehicle_id;
                    ";

                    var vehicleDictionary = new Dictionary<int, Vehicle>();

                    var result = connection.Query<Vehicle, Vehicle.Image, Vehicle>(query,
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
                        splitOn: "ImageId"
                    ).Distinct().ToList();

                    return result;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }

        public static VehicleCreation ReqInsertVehicle(VehicleCreation vehicle)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = @"
                        INSERT INTO vehicles (
                            brand,
                            model,
                            year,
                            price,
                            mileage,
                            engine,
                            horsepower,
                            type_id
                        )
                        VALUES (
                            @brand,
                            @model,
                            @year,
                            @price,
                            @mileage,
                            @engine,
                            @horsepower,
                            @type_id
                        );
                        SELECT SCOPE_IDENTITY();
                    ";

                    var parameters = new
                    {
                        brand = vehicle.Brand,
                        model = vehicle.Model,
                        year = vehicle.Year,
                        price = vehicle.Price,
                        mileage = vehicle.Mileage,
                        engine = vehicle.Engine,
                        horsepower = vehicle.Horsepower,
                        type_id = vehicle.Type_id
                    };

                    int newVehicleId = connection.QuerySingle<int>(query, parameters);
                    vehicle.Id = newVehicleId;
                    return vehicle;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }

        public static void ReqInsertImage(ImageForInsert image)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = @"
                        INSERT INTO images (vehicle_id, imageUrl) 
                        VALUES (@Vehicle_Id, @ImageUrl);
                        SELECT CAST(SCOPE_IDENTITY() as int);
                    ";

                    int newImageId = connection.QuerySingle<int>(query, new
                    {
                        Vehicle_Id = image.Vehicle_Id,
                        ImageUrl = image.ImageUrl
                    });
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }

        public static List<dynamic> GetBrandsRepository()
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = @"select DISTINCT v.brand from vehicles v;";
                    var result = connection.Query(query).ToList();
                    return result;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }

        public static List<dynamic> GetModelsRepository()
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = @"select DISTINCT v.model from vehicles v;";
                    var result = connection.Query(query).ToList();
                    return result;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }

        /* FIlters */

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
                            images.id AS ImageId,
                            images.vehicle_id,
                            images.imageUrl
                        FROM vehicles v
                        INNER JOIN vehicle_types vt ON v.type_id = vt.id
                        LEFT JOIN images ON v.id = images.vehicle_id
                        WHERE vt.id = @typeId;
                    ";

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
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }


        public static List<Vehicle> GetByMultiTypeRepository(Types TypesList)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    var typeIds = TypesList.TypesList.Select(t => t.typeId).ToList();
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
                            images.id AS ImageId,
                            images.vehicle_id,
                            images.imageUrl
                        FROM vehicles v
                        INNER JOIN vehicle_types vt ON v.type_id = vt.id
                        LEFT JOIN images ON v.id = images.vehicle_id
                        WHERE vt.id IN @typeIds;
                    ";

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
                        new { typeIds },
                        splitOn: "ImageId"
                    ).Distinct().ToList();

                    return result;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }
        public static List<Vehicle> FilterByHPRepository(HorsePower horsePower)
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
                            images.id AS ImageId,
                            images.vehicle_id,
                            images.imageUrl
                        FROM vehicles v
                        INNER JOIN vehicle_types vt ON v.type_id = vt.id
                        LEFT JOIN images ON v.id = images.vehicle_id
                        WHERE V.horsepower >= @MinHP AND V.horsepower <= @MaxHP;
                    ";

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
                        new { MinHP = horsePower.MinHP, MaxHP = @horsePower.MaxHP },
                        splitOn: "ImageId"
                    ).Distinct().ToList();

                    return result;
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database query failed: {ex.Message}", ex);
                }
            }
        }

    }
}
