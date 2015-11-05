using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.RegisterUser
{
    public class RegisterUserHandler : CommandHandler<RegisterUser, Result>, IAuthenticatedCommandHandler<RegisterUser, Result>
    {

        public RegisterUserHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(RegisterUser command)
        {
            User user = User.Register(command.ProcessId, command.UserId, command.UserName, command.PrimaryEmail);
            EventRepository.Save(user);

            RegisteredUser registeredUser = new RegisteredUser(command.AuthUserId, command.UserId, command.UserName);
            Session.Save(registeredUser);

            return Success();
        }
    }
}
