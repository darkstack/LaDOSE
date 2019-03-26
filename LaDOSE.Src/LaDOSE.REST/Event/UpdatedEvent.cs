using System;
using LaDOSE.DTO;

namespace LaDOSE.REST.Event
{
    public class UpdatedJwtEventHandler : EventArgs
    {
        private readonly ApplicationUserDTO msg;
        public UpdatedJwtEventHandler(ApplicationUserDTO applicationUser)
        {
            this.msg = applicationUser;
        }

        public ApplicationUserDTO Message => msg;
    }
}