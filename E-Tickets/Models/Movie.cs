namespace E_Tickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string? ImgUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MovieStatus MovieStatus { get; set; }
        public Category Category { get; set; } = null!;
        public Cinema Cinema { get; set; } = null!;
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public int CategoryId { get; set; }
        public int CinemaId { get; set; }

    }
}
