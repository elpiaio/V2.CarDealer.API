using Dapper;
using Microsoft.Extensions.WebEncoders.Testing;
using System.Data.SqlClient;
using V2.CarDealer.API.DTOs;
using V2.CarDealer.API.DTOs.CarsObjects;

namespace V2.CarDealer.API.Repositories
{
    public class SearchesRepository
    {
        public static List<Vehicle> GlobalSearchRep(PaginationParam paginationParam)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                string query = $"EXEC SearchByBrandAndModel @input = {paginationParam.text}, @page = {paginationParam.page}, @limit = {paginationParam.limit};";
                List<Vehicle> vehicles = connection.Query<Vehicle>(query).ToList();

                return vehicles;
            }
        }
        public static List<Vehicle> SearchByBrand(PaginationParam paginationParam)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                string query = $"EXEC SearchByBrand @input = {paginationParam.text}, @page = {paginationParam.page}, @limit = {paginationParam.limit};";
                List<Vehicle> vehicles = connection.Query<Vehicle>(query).ToList();

                return vehicles;
            }
        }
        public static List<Vehicle> SearchByModel(PaginationParam paginationParam)
        {
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                string query = $"EXEC SearchByModel @input = {paginationParam.text}, @page = {paginationParam.page}, @limit = {paginationParam.limit};";
                List<Vehicle> vehicles = connection.Query<Vehicle>(query).ToList();

                return vehicles;
            }
        }
    }
}
