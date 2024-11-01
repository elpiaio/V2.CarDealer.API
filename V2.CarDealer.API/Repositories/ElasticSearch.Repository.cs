using System.Data.SqlClient;
using Dapper;
using Nest;
using Newtonsoft.Json;
using V2.CarDealer.API.DTOs;
using V2.CarDealer.API.DTOs.CarsObjects;

namespace V2.CarDealer.API.Repositories
{ 
    public class ElasticSearchRepository
    {
        public static string InsertVehicles()
        {
            var client = new ElasticClient(Settings.ElasticSearchSettings);
            
            using (var connection = new SqlConnection(Settings.SQLConnectionString))
            {
                var vehicles = connection.Query<Vehicle>("SELECT * FROM vehicles").ToList(); 

                var bulkResponse = client.Bulk(b => b
                    .Index("vehicles")
                        .IndexMany(vehicles, (descriptor, vehicle) => descriptor.Id(vehicle.Id)) 
                );

                if (bulkResponse.Errors)
                {
                    return "Erro na indexação";
                }
                else
                {
                    return "Indexação bem-sucedida!";
                }
            }
        }

        public static string InsertVehicle(Vehicle vehicle)
        {
            var client = new ElasticClient(Settings.ElasticSearchSettings);

            try
            {
                var indexResponse = client.IndexDocument(vehicle);

                if (indexResponse.IsValid)
                {
                    return "Indexação bem-sucedida!";
                }
                else
                {
                    return $"Erro na indexação: {indexResponse.ServerError.Error.Reason}";
                }
            }
            catch (Exception ex)
            {
                return $"Ocorreu um erro ao indexar o veículo: {ex.Message}";
            }
        }

        public static IReadOnlyCollection<Vehicle> GetAllVehicles()
        {
            var client = new ElasticClient(Settings.ElasticSearchSettings);
            var indexResponse = client.Search<Vehicle>(s => s.Query(q => q.MatchAll()).Size(1000)).Documents;

            return indexResponse;
        }


        public static IReadOnlyCollection<Vehicle> SearchVehiclesByBrandAndModel(PaginationParam param)
        {
            var client = new ElasticClient(Settings.ElasticSearchSettings);

            var searchResponse = client.Search<Vehicle>(s => s
                .Index("vehicles")
                .From(param.page)
                .Size(param.limit)
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(param.text)
                        .Fields(f => f.Field(v => v.Brand).Field(v => v.Model))
                        .Fuzziness(Fuzziness.EditDistance(2))
                        .MinimumShouldMatch("75%")
                    )
                )
            );

            if (searchResponse.IsValid)
            {
                return searchResponse.Documents;
            }
            else
            {
               throw new Exception("Erro na pesquisa: " + searchResponse.DebugInformation);
            }
        }
    }
}
