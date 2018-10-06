using System.Collections.Generic;
using LaDOSE.Entity;

namespace LaDOSE.Business.Interface
{
    public interface IUserService
    {
        ApplicationUser Authenticate(string username, string password);
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser GetById(int id);
        ApplicationUser Create(ApplicationUser user, string password);
        void Update(ApplicationUser user, string password = null);
        void Delete(int id);
    }
}