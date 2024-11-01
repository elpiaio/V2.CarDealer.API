using Nest;

namespace V2.CarDealer.API
{
    public class Settings
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// String de conexão com o SQL
        /// </summary>
        public static string SQLConnectionString => Configuration.GetConnectionString("DefaultConnection");
        public static ConnectionSettings ElasticSearchSettings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("vehicles").DisableDirectStreaming();
    }
}
