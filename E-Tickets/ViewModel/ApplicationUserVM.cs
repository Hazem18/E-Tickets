using System.ComponentModel.DataAnnotations;

namespace E_Tickets.ViewModel
{
    public class ApplicationUserVM
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? City { get; set; }


    }
}
