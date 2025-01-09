using System.ComponentModel.DataAnnotations;

namespace mptc.dgc.isaac.verify.service.Dtos.Todo
{
    public class TodoDto : BaseSanitizerDto
    {
        public int Id { get; set; } // Unique identifier
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty; // Task title
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty; // Task details
        public bool IsCompleted { get; set; } = false; // Completion status
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp
    }
}