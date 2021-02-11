using Newtonsoft.Json;

namespace AE.ImageGallery.Supplier.Application.Contracts
{
    public class StatusResponseDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}