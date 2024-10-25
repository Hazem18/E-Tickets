using System.ComponentModel.DataAnnotations;

namespace E_Tickets.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage = "Length must be 3-50 characters")]
        [MaxLength(100)]
        public string? Name { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    }
}
