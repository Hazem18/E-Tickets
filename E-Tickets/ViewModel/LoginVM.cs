using System.ComponentModel.DataAnnotations;

namespace E_Tickets.ViewModel
{
    public class LoginVM
    {
        public  int Id { get; set; }
        [Required]
        public string? User { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
