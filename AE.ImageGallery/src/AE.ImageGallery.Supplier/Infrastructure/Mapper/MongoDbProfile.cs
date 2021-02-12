using AE.ImageGallery.Supplier.Application.Contracts;
using AE.ImageGallery.Supplier.Infrastructure.DbModels;
using AutoMapper;

namespace AE.ImageGallery.Supplier.Infrastructure.Mapper
{
    public class MongoDbProfile: Profile
    {
        public MongoDbProfile()
        {
            CreateMap<PictureResponseDto, ImageDbModel>();
        }
    }
}