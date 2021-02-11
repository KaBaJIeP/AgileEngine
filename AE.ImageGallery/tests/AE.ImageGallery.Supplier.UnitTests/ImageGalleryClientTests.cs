using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AE.ImageGallery.Supplier.Application;
using AE.ImageGallery.Supplier.Application.Api;
using AE.ImageGallery.Supplier.Application.Contracts;
using AE.ImageGallery.Supplier.Configs;
using FakeItEasy;
using FizzWare.NBuilder;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace AE.ImageGallery.Supplier.UnitTests
{
    [TestFixture]
    public class ImageGalleryClientTests
    {
        private IImageGalleryClient _client;
        private IImageGalleryApi _api;

        [SetUp]
        public void SetUp()
        {
            var config = A.Fake<IOptions<AgileEngineConfig>>();
            _api = A.Fake<IImageGalleryApi>();
            _client = new ImageGalleryClient(_api, config);
        }

        [Test]
        public async Task Auth_WithValidApiKey_Success()
        {
            // arrange
            var validApiKey = "any_valid_api_key";
            var validToken = "any_valid_token";
            var expectedResponse = Builder<AuthResponseDto>.CreateNew()
                .With(x => x.Auth = true)
                .With(x => x.Token = validToken)
                .Build();
            A.CallTo(() => _api.Auth(A<AuthRequestDto>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.Auth(validApiKey);

            // assert
            Assert.AreEqual(expectedResponse.Auth, response.Auth);
            Assert.AreEqual(expectedResponse.Token, response.Token);
        }

        [Test]
        public async Task Auth_WithEmptyApiKey_NoToken()
        {
            // arrange
            var invalidApiKey = string.Empty;
            var expectedResponse = Builder<AuthResponseDto>.CreateNew()
                .With(x => x.Auth = false)
                .With(x => x.Token = null)
                .Build();
            A.CallTo(() => _api.Auth(A<AuthRequestDto>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.Auth(invalidApiKey);

            // assert
            Assert.AreEqual(expectedResponse.Auth, response.Auth);
            Assert.AreEqual(expectedResponse.Token, response.Token);
        }

        [Test]
        public async Task Auth_WithNullApiKey_NoToken()
        {
            // arrange
            string invalidApiKey = null;
            var expectedResponse = Builder<AuthResponseDto>.CreateNew()
                .With(x => x.Auth = false)
                .With(x => x.Token = null)
                .Build();

            // act
            var response = await _client.Auth(invalidApiKey);

            // assert
            Assert.AreEqual(expectedResponse.Auth, response.Auth);
            Assert.AreEqual(expectedResponse.Token, response.Token);
        }

        [Test]
        public async Task GetImages_Success()
        {
            // arrange
            var pictureDtoList = Builder<PictureDto>.CreateListOfSize(10).Build().ToList();
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Page = 1)
                .With(x => x.PageCount = 26)
                .With(x => x.Pictures = pictureDtoList)
                .With(x => x.HasMore = true)
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages();

            // assert
            Assert.AreEqual(expectedResponse.Page, response.Page);
            Assert.AreEqual(expectedResponse.PageCount, response.PageCount);
            Assert.AreEqual(expectedResponse.Pictures.Count, response.Pictures.Count);
            Assert.AreEqual(expectedResponse.HasMore, response.HasMore);
        }

        [Test]
        public async Task GetImages_WithInvalidToken_Unauthorized()
        {
            // arrange
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Status = "Unauthorized")
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages();

            // assert
            Assert.AreEqual(expectedResponse.Status, response.Status);
        }

        [Test]
        public async Task GetImages_WithNoImages_Success()
        {
            // arrange
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Page = 1)
                .With(x => x.PageCount = 1)
                .With(x => x.Pictures = new List<PictureDto>())
                .With(x => x.HasMore = false)
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages();

            // assert
            Assert.AreEqual(expectedResponse.Page, response.Page);
            Assert.AreEqual(expectedResponse.PageCount, response.PageCount);
            Assert.AreEqual(expectedResponse.Pictures.Count, response.Pictures.Count);
            Assert.AreEqual(expectedResponse.HasMore, response.HasMore);
        }

        [Test]
        public async Task GetImages_WithSecondPage_Success()
        {
            // arrange
            var pageNumber = 2;
            var pictureDtoList = Builder<PictureDto>.CreateListOfSize(10).Build().ToList();
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Page = pageNumber)
                .With(x => x.PageCount = 26)
                .With(x => x.Pictures = pictureDtoList)
                .With(x => x.HasMore = true)
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored, pageNumber)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages(pageNumber);

            // assert
            Assert.AreEqual(expectedResponse.Page, response.Page);
            Assert.AreEqual(expectedResponse.PageCount, response.PageCount);
            Assert.AreEqual(expectedResponse.Pictures.Count, response.Pictures.Count);
            Assert.AreEqual(expectedResponse.HasMore, response.HasMore);
        }

        [Test]
        public async Task GetImages_WithLastPage_Success()
        {
            // arrange
            var lastPage = 26;
            var pictureDtoList = Builder<PictureDto>.CreateListOfSize(9).Build().ToList();
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Page = lastPage)
                .With(x => x.PageCount = 26)
                .With(x => x.Pictures = pictureDtoList)
                .With(x => x.HasMore = false)
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored, lastPage)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages(lastPage);

            // assert
            Assert.AreEqual(expectedResponse.Page, response.Page);
            Assert.AreEqual(expectedResponse.PageCount, response.PageCount);
            Assert.AreEqual(expectedResponse.Pictures.Count, response.Pictures.Count);
            Assert.AreEqual(expectedResponse.HasMore, response.HasMore);
        }

        [Test]
        public async Task GetImages_WithPageExceedLast_Success()
        {
            // arrange
            var lastPage = 100;
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Page = lastPage)
                .With(x => x.PageCount = 26)
                .With(x => x.Pictures = new List<PictureDto>())
                .With(x => x.HasMore = false)
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored, lastPage)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages(lastPage);

            // assert
            Assert.AreEqual(expectedResponse.Page, response.Page);
            Assert.AreEqual(expectedResponse.PageCount, response.PageCount);
            Assert.AreEqual(expectedResponse.Pictures.Count, response.Pictures.Count);
            Assert.AreEqual(expectedResponse.HasMore, response.HasMore);
        }

        [Test]
        public async Task GetImages_WithPageLessThanOne_ReturnFirstPage()
        {
            // arrange
            var lastPage = -1;
            var pictureDtoList = Builder<PictureDto>.CreateListOfSize(10).Build().ToList();
            var expectedResponse = Builder<PicturePageResponseDto>.CreateNew()
                .With(x => x.Page = 1)
                .With(x => x.PageCount = 26)
                .With(x => x.Pictures = pictureDtoList)
                .With(x => x.HasMore = true)
                .Build();
            A.CallTo(() => _api.GetImages(A<string>.Ignored, lastPage)).Returns(expectedResponse);

            // act
            var response = await _client.GetImages(lastPage);

            // assert
            Assert.AreEqual(expectedResponse.Page, response.Page);
            Assert.AreEqual(expectedResponse.PageCount, response.PageCount);
            Assert.AreEqual(expectedResponse.Pictures.Count, response.Pictures.Count);
            Assert.AreEqual(expectedResponse.HasMore, response.HasMore);
        }

        [Test]
        public async Task GetImage_WithValidId_Success()
        {
            // arrange
            var validImageId = "some_valid_image_id";
            var expectedResponse = Builder<PictureResponseDto>.CreateNew()
                .With(x => x.Status = string.Empty)
                .Build();
            A.CallTo(() => _api.GetImage(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.GetImage(validImageId);

            // assert
            Assert.AreEqual(expectedResponse.Id, response.Id);
            Assert.AreEqual(expectedResponse.Author, response.Author);
            Assert.AreEqual(expectedResponse.Camera, response.Camera);
            Assert.AreEqual(expectedResponse.Tags, response.Tags);
            Assert.AreEqual(expectedResponse.CroppedPicture, response.CroppedPicture);
            Assert.AreEqual(expectedResponse.FullPicture, response.FullPicture);
            Assert.AreEqual(expectedResponse.Status, response.Status);
        }

        [Test]
        public async Task GetImage_WithInvalidId_Success()
        {
            // arrange
            var invalidImageId = "some_invalid_image_id";
            var expectedResponse = Builder<PictureResponseDto>.CreateNew()
                .With(x => x.Status = "Not found")
                .Build();
            A.CallTo(() => _api.GetImage(A<string>.Ignored, A<string>.Ignored)).Returns(expectedResponse);

            // act
            var response = await _client.GetImage(invalidImageId);

            // assert
            Assert.AreEqual(expectedResponse.Status, response.Status);
        }
    }
}