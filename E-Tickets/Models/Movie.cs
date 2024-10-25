using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Tickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3-100 characters")]
        [MaxLength(100)]
        public string? Name { get; set; }
        
        [Required]
        [MinLength(3, ErrorMessage = "Description must be 10-500 characters")]
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0,100000)]
        public double Price { get; set; }
        [ValidateNever]
        public string? ImgUrl { get; set; }
        [Required]
        public string? TrailerUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public MovieStatus MovieStatus { get; set; }
        [ValidateNever]
        public Category Category { get; set; } = null!;
        [ValidateNever]
        public Cinema Cinema { get; set; } = null!;
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public int CategoryId { get; set; }
        public int CinemaId { get; set; }

    }
}
