using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AddUser
{
    public class AddUserResponse
    {
        public const string UsernameTaken = "That user name is already in use.";
        public const string EmailTaken = "That Email Address is already in use.";

        public bool Success { get; set; }
        public string FailureDescription { get; set; }
    }
}
