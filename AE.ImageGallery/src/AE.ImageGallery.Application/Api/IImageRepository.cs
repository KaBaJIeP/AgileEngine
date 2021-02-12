using System.Collections.Generic;
using System.Threading.Tasks;
using AE.ImageGallery.Application.Models;

namespace AE.ImageGallery.Application.Api
{
    public interface IImageRepository
    {
        public Task<List<ImageModel>> GetImages(List<string> ids);
    }
}