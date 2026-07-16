using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DirectorsController : ControllerBase
    {
        private readonly IGenericService<DirectorModel> _directorService;

        public DirectorsController(IGenericService<DirectorModel> directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<DirectorModel>>> GetAll(CancellationToken cancellationToken)
        {
            var directors = await _directorService.GetAllAsync(cancellationToken);
            return Ok(directors);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DirectorModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var director = await _directorService.GetByIdAsync(id, cancellationToken);
            if (director == null) return NotFound($"Regista con ID {id} non trovato.");
            return Ok(director);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DirectorModel>> Create([FromBody] DirectorModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _directorService.CreateAsync(model, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] DirectorModel model, CancellationToken cancellationToken)
        {
            if (id != model.Id) return BadRequest("L'ID non corrisponde.");
            var success = await _directorService.UpdateAsync(model, cancellationToken);
            if (!success) return NotFound($"Regista con ID {id} non trovato.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _directorService.DeleteAsync(id, cancellationToken);
            if (!success) return NotFound($"Regista con ID {id} non trovato.");
            return NoContent();
        }
    }
}