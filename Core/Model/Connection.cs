using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Connection
    {
        public virtual Guid Id { get; protected set; }
        public virtual User User1 { get; protected set; }
        public virtual User User2 { get; protected set; }

        public Connection(User user1, User user2)
        {
            User1 = user1;
            User2 = user2;
        }

        protected Connection() { }
    }
}
