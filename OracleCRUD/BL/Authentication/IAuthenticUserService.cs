using BL.User;

namespace BL.Authentication
{
    public interface IAuthenticUserService
    {
        bool CheckConnection(string username, string password);
        ApplicationUser GetMyInfo(string username, string password);
        void UpdateMyInfo(ApplicationUser updatedUserInfo);
    }
}