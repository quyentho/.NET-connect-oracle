using BL.User;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Text;

namespace BL.Authentication
{
    public class AuthenticUserService : IAuthenticUserService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool CheckConnection(string username, string password)
        {
            string connectionString = GetConnectionString(username, password);
            using (IDbConnection db = new OracleConnection(connectionString))
            {
                db.Open();
                if (db.State == ConnectionState.Open)
                {
                    return true;
                }

                return false;
            }
        }
        public ApplicationUser GetMyInfo(string username, string password)
        {
            string connectionString = GetConnectionString(username, password);

            using (IDbConnection db = new OracleConnection(connectionString))
            {
                string sqlString = $"Select * From dotnetcore.users Where username = '{username}'";
                return db.QuerySingle<ApplicationUser>(sqlString);
            }
        }

        public void UpdateMyInfo(ApplicationUser updatedUserInfo)
        {
            _userRepository.Update(updatedUserInfo.Id, updatedUserInfo);
        }

        private string GetConnectionString(string username, string password)
        {
            string connectionString = $@"User Id={username};Password=""{password}"";Data Source=DESKTOP-UPOOFN0:1521/orclpdb.localdomain";
            return connectionString;
        }
    }
}
