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
    public class GenresController : ControllerBase
    {
        private readonly IGenericService<GenreModel> _genreService;

        public GenresController(IGenericService<GenreModel> genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GenreModel>>> GetAll()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenreModel>> GetById(int id)
        {
            var genre = await _genreService.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound($"Genere con ID {id} non trovato."); 
            }

            return Ok(genre); 
        }

        [HttpPost]
        public async Task<ActionResult<GenreModel>> Create([FromBody] GenreModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdGenre = await _genreService.CreateAsync(model);

            return CreatedAtAction(nameof(GetById), new { id = createdGenre.Id }, createdGenre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GenreModel model)
        {
            if (id != model.Id)
            {
                return BadRequest("L'ID nell'URL non corrisponde all'ID nel corpo della richiesta.");
            }

            var success = await _genreService.UpdateAsync(model);

            if (!success)
            {
                return NotFound($"Impossibile aggiornare: Genere con ID {id} non trovato.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _genreService.DeleteAsync(id);

            if (!success)
            {
                return NotFound($"Impossibile eliminare: Genere con ID {id} non trovato.");
            }

            return NoContent();
        }
    }
}