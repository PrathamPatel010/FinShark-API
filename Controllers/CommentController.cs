using api.Interfaces;
using api.Models;
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
        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var results = comments.Select(comment => comment.ToCommentDto());
            return Ok(results);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequestDto createCommendRequestDto)
        {
            var commentModel = createCommendRequestDto.ToCommentFromCreateDto();
            var comment = await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(Get), new { id = comment.Id }, comment.ToCommentDto());
        }
    }
}