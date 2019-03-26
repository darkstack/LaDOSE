
using System;

namespace LaDOSE.DTO
{
    public class ApplicationUserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }

}