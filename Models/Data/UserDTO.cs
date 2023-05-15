using CloudinaryDotNet;
using System.ComponentModel.DataAnnotations;

namespace OrganicShop2.Models.Data
{
    public class UserDTO
    {
        [Key]
        public int Id { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
