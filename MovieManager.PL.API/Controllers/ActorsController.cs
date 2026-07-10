using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IGenericService<ActorModel> _actorService;

        public ActorsController(IGenericService<ActorModel> actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ActorModel>>> GetAll()
        {
            return Ok(await _actorService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorModel>> GetById(int id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor == null) return NotFound($"Attore con ID {id} non trovato.");
            return Ok(actor);
        }

        [HttpPost]
        public async Task<ActionResult<ActorModel>> Create([FromBody] ActorModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _actorService.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActorModel model)
        {
            if (id != model.Id) return BadRequest("L'ID non corrisponde.");
            var success = await _actorService.UpdateAsync(model);
            if (!success) return NotFound($"Attore con ID {id} non trovato.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _actorService.DeleteAsync(id);
            if (!success) return NotFound($"Attore con ID {id} non trovato.");
            return NoContent();
        }
    }
}