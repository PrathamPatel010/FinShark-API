using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.DTOs.Comment;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var results = comments.Select(comment => comment.ToCommentDto());
            return Ok(results);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto createCommendRequestDto)
        {
            var stockExist = await _stockRepo.IsStockExist(stockId);
            if (stockExist == false)
            {
                return BadRequest("The Stock Does not exist");
            }
            var commentModel = createCommendRequestDto.ToCommentFromCreateDto(stockId);
            var comment = await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(Get), new { id = comment.Id }, comment.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateComment)
        {
            var updatedComment = await _commentRepo.UpdateAsync(id, updateComment);
            if (updatedComment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(updatedComment.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepo.DeleteAsync(id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDto());
        }
    }
}