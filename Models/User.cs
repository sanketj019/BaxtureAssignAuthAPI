using System.ComponentModel.DataAnnotations;

namespace BaxtureAssignAuthAPI.Models
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid().ToString(); // Generating a unique identifier on the server side
            IsAdmin = false; // Default value for isAdmin
            Hobbies = new List<Hobby>(); // Initializing the hobbies list
        }

        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Password should be alphanumeric")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Required]
        public int Age { get; set; }

        public List<Hobby> Hobbies { get; set; }
    }
}
