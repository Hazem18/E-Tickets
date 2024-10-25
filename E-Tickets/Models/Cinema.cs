using System.ComponentModel.DataAnnotations;

namespace E_Tickets.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3-100 characters")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Description must be 3-500 characters")]
        [MaxLength(500)]
        public string? Description { get; set; }
        public string? cinemaLogo { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Address must be 3-500 characters")]
        [MaxLength(500)]
        public string? Address { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
