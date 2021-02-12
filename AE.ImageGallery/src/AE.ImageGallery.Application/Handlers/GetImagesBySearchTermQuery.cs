using System.Collections.Generic;
using AE.ImageGallery.Application.Models;
using MediatR;

namespace AE.ImageGallery.Application.Handlers
{
    public class GetImagesBySearchTermQuery: IRequest<List<ImageModel>>
    {
        public string SearchTerm { get; set; }
    }
}