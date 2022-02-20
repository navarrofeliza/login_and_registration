using System.Collections.Generic;

namespace LoginandReg.Models
{
    public class Wrapper
    {
        public User User { get; set; }
        public List<User> AllUsers { get; set; }
        public LoginUser UserLogin { get; set; }
    }
}