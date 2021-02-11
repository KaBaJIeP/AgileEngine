using Newtonsoft.Json;

namespace AE.ImageGallery.Supplier.Application.Contracts
{
    public class AuthRequestDto
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}