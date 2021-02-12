using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AE.ImageGallery.Application.Controllers
{
    [Route("[controller]")]
    public class SearchController: ControllerBase
    {
        [HttpGet("{searchTerm}")]
        public Task<string> Search([FromRoute]string searchTerm)
        {
            return Task.FromResult(searchTerm);
        }
    }
}