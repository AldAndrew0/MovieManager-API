using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces; 

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieActorsController : ControllerBase
    {
        private readonly IMovieActorService _movieActorService;

        public MovieActorsController(IMovieActorService movieActorService)
        {
            _movieActorService = movieActorService;
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IReadOnlyList<MovieActorModel>>> GetByMovieId(int movieId)
        {
            var cast = await _movieActorService.GetByMovieIdAsync(movieId);
            return Ok(cast);
        }

        [HttpGet("{movieId}/{actorId}")]
        public async Task<ActionResult<MovieActorModel>> GetByIds(int movieId, int actorId)
        {
            var relation = await _movieActorService.GetByIdsAsync(movieId, actorId);

            if (relation == null)
            {
                return NotFound($"Relazione tra Film {movieId} e Attore {actorId} non trovata.");
            }

            return Ok(relation);
        }

        [HttpPost]
        public async Task<ActionResult<MovieActorModel>> Create([FromBody] MovieActorModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _movieActorService.CreateAsync(model);

            return CreatedAtAction(nameof(GetByIds), new { movieId = created.MovieId, actorId = created.ActorId }, created);
        }

        [HttpDelete("{movieId}/{actorId}")]
        public async Task<IActionResult> Delete(int movieId, int actorId)
        {
            var success = await _movieActorService.DeleteAsync(movieId, actorId);

            if (!success)
            {
                return NotFound("Impossibile eliminare: Relazione non trovata.");
            }

            return NoContent();
        }
    }
}