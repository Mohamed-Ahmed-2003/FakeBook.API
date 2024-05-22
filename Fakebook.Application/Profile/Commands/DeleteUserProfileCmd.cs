using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakebook.Application.Profile.Commands
{
    public class DeleteUserProfileCmd(Guid id) : IRequest
    {
        public Guid UserProfileId { get; set; } = id;

    }
}
