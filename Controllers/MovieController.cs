using compulsoryRest.Models;
using compulsoryRest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace compulsoryRest.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MovieController : BaseController<Movie> {
    private readonly MovieRepository _movieRepository;

    public MovieController(MovieRepository movieRepository)
        : base(movieRepository) {
        _movieRepository = movieRepository;
    }

    // GET: api/movie/genre/{genre}
    [HttpGet("genre/{genre}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(string genre) {
        var movies = await _movieRepository.GetMoviesByGenreAsync(genre);
        return Ok(movies);
    }

    // GET: api/movie/year/{year}
    [HttpGet("year/{year}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByYear(int year) {
        var movies = await _movieRepository.GetMoviesByYearAsync(year);
        return Ok(movies);
    }
}