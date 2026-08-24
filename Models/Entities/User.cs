using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace obrazo.Models.Entities
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public ICollection<UserMedia> UserMedias { get; set; } = new List<UserMedia>();
    }
}