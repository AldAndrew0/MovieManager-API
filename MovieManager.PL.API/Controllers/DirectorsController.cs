using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IGenericService<DirectorModel> _directorService;

        public DirectorsController(IGenericService<DirectorModel> directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DirectorModel>>> GetAll()
        {
            return Ok(await _directorService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DirectorModel>> GetById(int id)
        {
            var director = await _directorService.GetByIdAsync(id);
            if (director == null) return NotFound($"Regista con ID {id} non trovato.");
            return Ok(director);
        }

        [HttpPost]
        public async Task<ActionResult<DirectorModel>> Create([FromBody] DirectorModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _directorService.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DirectorModel model)
        {
            if (id != model.Id) return BadRequest("L'ID non corrisponde.");
            var success = await _directorService.UpdateAsync(model);
            if (!success) return NotFound($"Regista con ID {id} non trovato.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _directorService.DeleteAsync(id);
            if (!success) return NotFound($"Regista con ID {id} non trovato.");
            return NoContent();
        }
    }
}