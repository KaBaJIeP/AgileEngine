using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AE.ImageGallery.Application.Api;
using AE.ImageGallery.Application.Models;
using AE.ImageGallery.Infrastructure.DbModels;
using AE.ImageGallery.Supplier.Configs;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AE.ImageGallery.Infrastructure
{
    public class ImageRepository: IImageRepository
    {
        private string _databaseName = "imageGallery";
        private string _imageCollectionName = "images";
        private readonly IMapper _mapper;
        private readonly IOptions<AgileEngineConfig> _options;

        public ImageRepository(IMapper mapper, IOptions<AgileEngineConfig> options)
        {
            _mapper = mapper;
            _options = options;
        }

        public async Task<List<ImageModel>> GetImages(List<string> ids)
        {
            var client = new MongoClient(_options.Value.MongoConnectionString);
            var database = client.GetDatabase(_databaseName);
            var collection = database.GetCollection<ImageDbModel>(_imageCollectionName);
            var dbResults = await collection.Find(x => ids.Contains(x.Id)).ToListAsync<ImageDbModel>();

            var result = dbResults.Select(x => _mapper.Map<ImageModel>(x)).ToList();

            return result;
        }
    }
}