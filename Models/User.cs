using System.ComponentModel.DataAnnotations;

namespace FAP_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
    }
}
