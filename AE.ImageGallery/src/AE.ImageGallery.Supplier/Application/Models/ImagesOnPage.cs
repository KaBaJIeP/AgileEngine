using System.Collections.Generic;
using AE.ImageGallery.Supplier.Application.Contracts;

namespace AE.ImageGallery.Supplier.Application
{
    public class ImagesOnPage : Page
    {
        public List<PictureResponseDto> Pictures { get; set; }
    }
}