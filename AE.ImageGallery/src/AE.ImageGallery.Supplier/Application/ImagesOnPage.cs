using System.Collections.Generic;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class ImagesOnPage
    {
        public List<PictureResponseDto> Pictures { get; set; }
        public int PageCount { get; set; }
    }
}