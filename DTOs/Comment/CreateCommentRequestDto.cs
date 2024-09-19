using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must of atleast 5 characters")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must of atleast 5 characters")]
        [MaxLength(300, ErrorMessage = "Content cannot exceed 100 characters")]
        public string Content { get; set; } = string.Empty;
    }
}