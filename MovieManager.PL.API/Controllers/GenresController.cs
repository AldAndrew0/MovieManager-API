using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieManager.PL.API.Controllers
{
    /// <summary>
    /// Controller per la gestione dei Generi Cinematografici.
    /// Espone gli endpoint RESTful comunicando esclusivamente con il Business Logic Layer.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GenresController : ControllerBase
    {
        private readonly IGenericService<GenreModel> _genreService;

        public GenresController(IGenericService<GenreModel> genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<GenreModel>>> GetAll(CancellationToken cancellationToken)
        {
            var genres = await _genreService.GetAllAsync(cancellationToken);
            return Ok(genres); 
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var genre = await _genreService.GetByIdAsync(id, cancellationToken);

            if (genre == null)
            {
                return NotFound($"Genere con ID {id} non trovato."); 
            }

            return Ok(genre); 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GenreModel>> Create([FromBody] GenreModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdGenre = await _genreService.CreateAsync(model, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdGenre.Id }, createdGenre);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] GenreModel model, CancellationToken cancellationToken)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID nell'URL non corrisponde all'ID nel corpo della richiesta.");
            }

            var success = await _genreService.UpdateAsync(model, cancellationToken);

            if (!success)
            {
                return NotFound($"Impossibile aggiornare: Genere con ID {id} non trovato.");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _genreService.DeleteAsync(id, cancellationToken);

            if (!success)
            {
                return NotFound($"Impossibile eliminare: Genere con ID {id} non trovato.");
            }

            return NoContent();
        }
    }
}