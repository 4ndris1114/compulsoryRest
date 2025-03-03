using Microsoft.AspNetCore.Mvc;
using compulsoryRest.Repositories;
using compulsoryRest.Models;

namespace YourProjectNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : BaseController<Movie>
    {
        public MoviesController(BaseRepository<Movie> repository) : base(repository)
        {
        }
    }
}
