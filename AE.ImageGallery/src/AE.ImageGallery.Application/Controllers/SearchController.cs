using System.Collections.Generic;
using System.Threading.Tasks;
using AE.ImageGallery.Application.Handlers;
using AE.ImageGallery.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AE.ImageGallery.Application.Controllers
{
    [Route("[controller]")]
    public class SearchController: ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{searchTerm}")]
        public async Task<List<ImageModel>> Search([FromRoute]string searchTerm)
        {
            var query = new GetImagesBySearchTermQuery
            {
                SearchTerm = searchTerm
            };
            return await _mediator.Send(query);
        }
    }
}