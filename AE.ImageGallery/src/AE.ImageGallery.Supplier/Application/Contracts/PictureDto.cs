using Newtonsoft.Json;

namespace AE.ImageGallery.Supplier.Application.Contracts
{
    public class PictureDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("id")]
        public string CroppedPicture { get; set; }
    }
}