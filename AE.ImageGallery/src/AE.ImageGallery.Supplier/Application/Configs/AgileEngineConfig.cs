namespace AE.ImageGallery.Supplier.Configs
{
    public class AgileEngineConfig
    {
        public const string SectionName = "AgileEngine";
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string MongoConnectionString { get; set; }
        public string RedisConnectionString { get; set; }
    }
}