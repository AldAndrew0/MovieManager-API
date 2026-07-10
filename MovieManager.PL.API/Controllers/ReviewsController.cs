using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IGenericService<ReviewModel> _reviewService;

        public ReviewsController(IGenericService<ReviewModel> reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ReviewModel>>> GetAll()
        {
            return Ok(await _reviewService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewModel>> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null) return NotFound($"Recensione con ID {id} non trovata.");
            return Ok(review);
        }

        [HttpPost]
        public async Task<ActionResult<ReviewModel>> Create([FromBody] ReviewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _reviewService.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewModel model)
        {
            if (id != model.Id) return BadRequest("L'ID non corrisponde.");
            var success = await _reviewService.UpdateAsync(model);
            if (!success) return NotFound($"Recensione con ID {id} non trovata.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reviewService.DeleteAsync(id);
            if (!success) return NotFound($"Recensione con ID {id} non trovata.");
            return NoContent();
        }
    }
}