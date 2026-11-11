using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models.DTO
{
    /// <summary>Client-provided fields used to create a message.</summary>
    public class AddMessageRequest
    {
        /// <summary>The recipient's email address.</summary>
        [Required(ErrorMessage = "The To field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100, ErrorMessage = "The To field must be a maximum of 100 characters.")]
        public string To { get; set; } = null!;

        [Required(ErrorMessage = "The From field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string From { get; set; } = null!;

        [Required(ErrorMessage = "The Content field is required.")]
        [StringLength(500, ErrorMessage = "The Content field must be a maximum of 500 characters.")]
        public string Content { get; set; } = null!;
    }
}
