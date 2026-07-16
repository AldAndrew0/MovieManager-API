using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces; 

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MovieActorsController : ControllerBase
    {
        private readonly IMovieActorService _movieActorService;

        public MovieActorsController(IMovieActorService movieActorService)
        {
            _movieActorService = movieActorService;
        }

        [HttpGet("movie/{movieId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<MovieActorModel>>> GetByMovieId(int movieId, CancellationToken cancellationToken)
        {
            var cast = await _movieActorService.GetByMovieIdAsync(movieId, cancellationToken);
            return Ok(cast);
        }

        [HttpGet("{movieId:int}/{actorId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieActorModel>> GetByIds(int movieId, int actorId, CancellationToken cancellationToken)
        {
            var relation = await _movieActorService.GetByIdsAsync(movieId, actorId, cancellationToken);

            if (relation == null)
            {
                return NotFound($"Relazione tra Film {movieId} e Attore {actorId} non trovata.");
            }

            return Ok(relation);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieActorModel>> Create([FromBody] MovieActorModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _movieActorService.CreateAsync(model, cancellationToken);

            return CreatedAtAction(nameof(GetByIds), new { movieId = created.MovieId, actorId = created.ActorId }, created);
        }

        [HttpDelete("{movieId:int}/{actorId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int movieId, int actorId, CancellationToken cancellationToken)
        {
            var success = await _movieActorService.DeleteAsync(movieId, actorId, cancellationToken);

            if (!success)
            {
                return NotFound("Impossibile eliminare: Relazione non trovata.");
            }

            return NoContent();
        }
    }
}