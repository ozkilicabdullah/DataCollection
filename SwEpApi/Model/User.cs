using System;
using System.Collections.Generic;

namespace SwEpApi.Model
{
    public class User
    {
        public string Username { get; set; }        
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public DateTime LastTokenDate { get; set; }
        public string LastToken  { get; set; }
        public Dictionary<string, List<string>> Perms { get; set; }
        
    }   
}
