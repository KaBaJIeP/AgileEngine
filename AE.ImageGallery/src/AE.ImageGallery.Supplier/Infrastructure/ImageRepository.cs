using System.Linq;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Configs;
using AE.ImageGallery.Supplier.Infrastructure.DbModels;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AE.ImageGallery.Supplier.Infrastructure
{
    public class ImageRepository: IImageRepository
    {
        private string _databaseName = "imageGallery";
        private string _imageCollectionName = "images";
        private readonly IOptions<AgileEngineConfig> _options;
        private readonly IMapper _mapper;

        public ImageRepository(IOptions<AgileEngineConfig> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
        }

        public Task Save(ImagesOnPage imagesOnPage)
        {
            var client = new MongoClient(_options.Value.MongoConnectionString);
            var database = client.GetDatabase(_databaseName);
            var collection = database.GetCollection<ImageDbModel>(_imageCollectionName);
            var images = imagesOnPage.Images.Select(_mapper.Map<ImageDbModel>).ToList();
            return collection.InsertManyAsync(images);
        }

        public async Task DeleteAllImages()
        {
            var client = new MongoClient(_options.Value.MongoConnectionString);
            var database = client.GetDatabase(_databaseName);
            await database.DropCollectionAsync(_imageCollectionName);
        }
    }
}