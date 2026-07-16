using Microsoft.AspNetCore.Mvc;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;

namespace MovieManager.PL.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly IGenericService<ReviewModel> _reviewService;

        public ReviewsController(IGenericService<ReviewModel> reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ReviewModel>>> GetAll(CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetAllAsync(cancellationToken);
            return Ok(reviews);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var review = await _reviewService.GetByIdAsync(id, cancellationToken);
            if (review == null) return NotFound($"Recensione con ID {id} non trovata.");
            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewModel>> Create([FromBody] ReviewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _reviewService.CreateAsync(model, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewModel model, CancellationToken cancellationToken)
        {
            if (id != model.Id) return BadRequest("L'ID non corrisponde.");
            var success = await _reviewService.UpdateAsync(model, cancellationToken);
            if (!success) return NotFound($"Recensione con ID {id} non trovata.");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var success = await _reviewService.DeleteAsync(id, cancellationToken);
            if (!success) return NotFound($"Recensione con ID {id} non trovata.");
            return NoContent();
        }
    }
}