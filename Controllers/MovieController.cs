using System.ComponentModel.DataAnnotations;
using compulsoryRest.Models;
using compulsoryRest.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace compulsoryRest.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MovieController : BaseController<Movie> {
    private readonly MovieRepository _movieRepository;
    private readonly ILogger<MovieController> _logger;

    public MovieController(MovieRepository movieRepository, ILogger<MovieController> logger)
        : base(movieRepository) {
        _movieRepository = movieRepository;
        _logger = logger;
    }

    // GET: api/movie
    public override async Task<ActionResult<IEnumerable<Movie>>> GetAll() {
        try {
            var movies = await _movieRepository.GetAllAsync();
            return Ok(movies);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error fetching movies");
            return StatusCode(500, "Error fetching movies");
        }
    }

    // GET: api/movie/{id}
    public override async Task<ActionResult<Movie>> GetById(string id) {
        try {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Invalid movie id format");

            var movie = await _movieRepository.GetByIdAsync(id);
            return movie == null ? NotFound("Movie not found") : Ok(movie);
        } catch (Exception ex) {
            _logger.LogError(ex, $"Error fetching movie with id {id}");
            return StatusCode(500, $"Couldn't fetch movie with id {id}");
        }
    }

    // POST: api/movie
    public override async Task<ActionResult<Movie>> Create(Movie movie) {
        try {
            ValidateMovie(movie);

            await _movieRepository.CreateAsync(movie);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        } catch (ValidationException ex) {
            return BadRequest(ex.Message);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error creating movie");
            return StatusCode(500, "Error creating movie");
        }
    }

    // PUT: api/movie/{id}
    public override async Task<ActionResult<Movie>> Update(string id, Movie movie) {
        try {
            if (id != movie.Id)
                return BadRequest("Invalid movie id format");
            ValidateMovie(movie);

            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
                return NotFound("Movie not found");
            
            await _movieRepository.UpdateAsync(id, movie);
            return NoContent();
        } catch (ValidationException ex) {
            return BadRequest(ex.Message);
        } catch (Exception ex) {
            _logger.LogError(ex, $"Error updating movie with id {id}");
            return StatusCode(500, $"Couldn't update movie with id {id}");
        }
    }

    // DELETE: api/movie/{id}
    public override async Task<ActionResult<Movie>> Delete(string id) {
        try {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
                return NotFound("Movie not found");
            
            await _movieRepository.DeleteAsync(id);
            return NoContent();
        } catch (Exception ex) {
            _logger.LogError(ex, $"Error deleting movie with id {id}");
            return StatusCode(500, $"Couldn't delete movie with id {id}");
        }
    }

    // GET: api/movie/genre/{genre}
    [HttpGet("genre/{genre}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(string genre) {
        try {
            if (string.IsNullOrEmpty(genre))
                return BadRequest("Invalid genre format");
            var movies = await _movieRepository.GetMoviesByGenreAsync(genre);
            return movies == null ? NotFound("Movies not found") : Ok(movies);
        } catch (Exception ex) {
            _logger.LogError(ex, $"Error fetching movies with genre {genre}");
            return StatusCode(500, $"Couldn't fetch movies with genre {genre}");
        }
    }

    // GET: api/movie/year/{year}
    [HttpGet("year/{year}")]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByYear(int year) {
        try {
            if (year < 1900 || year > DateTime.Now.Year)
                return BadRequest("Invalid year format");
            var movies = await _movieRepository.GetMoviesByYearAsync(year);
            return movies == null ? NotFound("Movies not found") : Ok(movies);
        } catch (Exception ex) {
            _logger.LogError(ex, $"Error fetching movies with year {year}");
            return StatusCode(500, $"Couldn't fetch movies with year {year}");
        }

    }

    //validation
    private void ValidateMovie(Movie movie)
    {
        var context = new ValidationContext(movie);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(movie, context, results, true))
        {
            throw new ValidationException(string.Join("; ", results.Select(r => r.ErrorMessage)));
        }
    }
}