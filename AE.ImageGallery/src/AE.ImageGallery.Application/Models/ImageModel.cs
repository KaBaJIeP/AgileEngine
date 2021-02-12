using Newtonsoft.Json;

namespace AE.ImageGallery.Application.Models
{
    public class ImageModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("camera")]
        public string Camera { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("cropped_picture")]
        public string CroppedPicture { get; set; }

        [JsonProperty("full_picture")]
        public string FullPicture { get; set; }
    }
}