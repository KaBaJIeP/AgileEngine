using Newtonsoft.Json;

namespace AE.ImageGallery.Supplier.Application.Contracts
{
    public class AuthResponseDto
    {
        [JsonProperty("auth")]
        public bool Auth { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}