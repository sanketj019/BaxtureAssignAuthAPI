using System.ComponentModel.DataAnnotations;

namespace BaxtureAssignAuthAPI.Models
{
    public class Hobby
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}