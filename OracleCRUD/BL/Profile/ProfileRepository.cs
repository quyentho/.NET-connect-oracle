using BL.Profile;
using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BL.Profile
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IConfiguration _configuration;

        public ProfileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<Profile> GetAll()
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return db.Query<Profile>
                ("Select * From dotnetcore.profiles").ToList();
            }
        }
    }
}
