using System.Collections.Generic;
using Newtonsoft.Json;

namespace AE.ImageGallery.Supplier.Application.Contracts
{
    public class PicturePageResponseDto : StatusResponseDto
    {
        [JsonProperty("pictures")]
        public List<PictureDto> Pictures { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }
    }
}