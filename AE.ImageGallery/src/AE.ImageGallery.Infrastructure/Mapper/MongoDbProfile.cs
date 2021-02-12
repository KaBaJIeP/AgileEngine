using AE.ImageGallery.Application.Models;
using AE.ImageGallery.Infrastructure.DbModels;
using AutoMapper;

namespace AE.ImageGallery.Supplier.Infrastructure.Mapper
{
    public class MongoDbProfile: Profile
    {
        public MongoDbProfile()
        {
            CreateMap<ImageDbModel, ImageModel>();
        }
    }
}