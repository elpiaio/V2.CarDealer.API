namespace V2.CarDealer.API
{
    public class Settings
    {
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// String de conexão com o SQL
        /// </summary>
        public static string SQLConnectionString => Configuration.GetConnectionString("DefaultConnection");

    }
}
