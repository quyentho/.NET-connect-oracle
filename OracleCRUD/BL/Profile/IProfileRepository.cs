using System.Collections.Generic;

namespace BL.Profile
{
    public interface IProfileRepository
    {
        List<Profile> GetAll();
    }
}