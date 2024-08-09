using Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using V2.CarDealer.API.DTOs;

namespace V2.CarDealer.API.Repositories
{
    public class SalesRepository
    {
        public static string ReqCreateSale(Sale sale)
        {

            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = @"
                        insert into sales (
                            brand, 
                            model, 
                            saleDate, 
                            price, 
                            userId, 
                            vehicleId
                        ) 
                        values (
                            @brand, 
                            @model, 
                            @saleDate, 
                            @price, 
                            @userId, 
                            @vehicleId
                        );
                    ";

                    string queryUpdateSale = @"
                        UPDATE 
                            vehicles 
                        SET 
                            sold = 1 
                        WHERE 
                            vehicles.id = @userId;
                    ";

                    var parameters = new
                    {
                        brand = sale.Brand,
                        model = sale.Model,
                        saleDate = sale.SaleDate,
                        price = sale.Price,
                        userId = sale.UserId,
                        vehicleId = sale.VehicleId
                    };

                    connection.Execute(queryUpdateSale, parameters);
                    connection.Execute(query, parameters);

                    return "Sucess!";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    throw;
                }
            }

        }
        public static List<Sales> ReqGetSales()
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                try
                {
                    string query = @"
                        SELECT 
                            s.Id as SaleId, v.Brand, v.Model, s.Price, s.SaleDate, s.UserId, s.VehicleId,
                            v.Id as VehicleId, v.Brand, v.Model, v.Year, v.Price as VehiclePrice, v.Mileage, v.Engine, v.Horsepower, v.Sold,
	                        vt.type,
                            u.Id as UserId, u.Name, u.Email, u.Cpf, u.Birthdate, u.Gender
                        FROM 
                            sales s
                        INNER JOIN 
                            vehicles v ON s.VehicleId = v.Id
                        INNER JOIN
	                        vehicle_types vt on v.type_id = vt.id
                        INNER JOIN
                            users u ON s.UserId = u.Id
                    ";

                    var salesDictionary = new Dictionary<int, Sales>();

                    var sales = connection.Query<Sales, Vehicle, User, Sales>(query,
                        (sale, vehicle, user) =>
                        {
                            if (!salesDictionary.TryGetValue(sale.VehicleId, out var currentSale))
                            {
                                currentSale = sale;
                                salesDictionary.Add(currentSale.VehicleId, currentSale);
                            }

                            currentSale.Vehicle = vehicle;
                            currentSale.user = user;

                            return currentSale;
                        },
                        splitOn: "VehicleId,UserId").Distinct().ToList();

                    return sales;
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
