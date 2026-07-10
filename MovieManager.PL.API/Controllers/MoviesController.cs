using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IGenericService<MovieModel> _movieService;

        public MoviesController(IGenericService<MovieModel> movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MovieModel>>> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieModel>> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound($"Film con ID {id} non trovato.");
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieModel>> Create([FromBody] MovieModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMovie = await _movieService.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID nell'URL non corrisponde all'ID nel corpo della richiesta.");
            }

            var success = await _movieService.UpdateAsync(model);

            if (!success)
            {
                return NotFound($"Impossibile aggiornare: Film con ID {id} non trovato.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _movieService.DeleteAsync(id);

            if (!success)
            {
                return NotFound($"Impossibile eliminare: Film con ID {id} non trovato.");
            }

            return NoContent();
        }
    }
}