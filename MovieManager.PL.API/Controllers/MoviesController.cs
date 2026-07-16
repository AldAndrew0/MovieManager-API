using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MoviesController : ControllerBase
    {
        private readonly IGenericService<MovieModel> _movieService;

        public MoviesController(IGenericService<MovieModel> movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<MovieModel>>> GetAll(CancellationToken cancellationToken)
        {
            var movies = await _movieService.GetAllAsync(cancellationToken);
            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieService.GetByIdAsync(id, cancellationToken);

            if (movie == null)
            {
                return NotFound($"Film con ID {id} non trovato.");
            }

            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieModel>> Create([FromBody] MovieModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMovie = await _movieService.CreateAsync(model, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdMovie.Id }, createdMovie);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] MovieModel model, CancellationToken cancellationToken)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID nell'URL non corrisponde all'ID nel corpo della richiesta.");
            }

            var success = await _movieService.UpdateAsync(model, cancellationToken);

            if (!success)
            {
                return NotFound($"Impossibile aggiornare: Film con ID {id} non trovato.");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _movieService.DeleteAsync(id, cancellationToken);

            if (!success)
            {
                return NotFound($"Impossibile eliminare: Film con ID {id} non trovato.");
            }

            return NoContent();
        }
    }
}