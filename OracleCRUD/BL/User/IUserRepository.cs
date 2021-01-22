using System.Collections.Generic;

namespace BL.User
{
    public interface IUserRepository
    {
        void Create(ApplicationUser newUser);
        void Delete(double id);
        ApplicationUser FindById(double id);
        List<ApplicationUser> GetAll();
        void Update(double id, ApplicationUser user);
    }
}