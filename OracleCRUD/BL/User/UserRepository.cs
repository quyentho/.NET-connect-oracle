using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BL.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<ApplicationUser> GetAll()
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return db.Query<ApplicationUser>("Select * From dotnetcore.users").ToList();
            }
        }

        public void Create(ApplicationUser newUser)
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                CreateOracleUser(newUser, db);

                InsertToUsersTable(newUser, db);
            }
        }

        private static void CreateOracleUser(ApplicationUser newUser, IDbConnection db)
        {
            string sqlQuery = $@"CREATE USER {newUser.Username} 
            IDENTIFIED BY ""{newUser.PasswordHash}"" 
            DEFAULT TABLESPACE space300 
            QUOTA {newUser.Quota} ON {newUser.TableSpaceName} 
            PROFILE profile1";

            db.Execute(sqlQuery);

            sqlQuery = $"grant connect to {newUser.Username}";
            db.Execute(sqlQuery);

            sqlQuery = $"grant select on dotnetcore.users to {newUser.Username}";
            db.Execute(sqlQuery);
        }

        private static void InsertToUsersTable(ApplicationUser newUser, IDbConnection db)
        {
            string sqlQuery = @"Insert into dotnetcore.users(USERNAME,FirstName,LastName,EMAIL,PHONE,PasswordHash,QUOTA,AccountStatus,TableSpaceName,ProfileName) 
    values(:Username,:FirstName,:LastName,:Email,:Phone,:PasswordHash,:Quota,1,:TableSpaceName,:ProfileName)";

            db.Execute(sqlQuery, newUser);
        }

        public void Update(double id, ApplicationUser user)
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var userFromDb = FindById(id);

                if (id == user.Id && userFromDb != null )
                {
                    AlterOracleUser(user, db, userFromDb);

                    // TODO: Update only properties that have been changed.
                    UpdateInUsersTable(user, db);
                }
            }
        }

              public void Delete(double id)
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var userFromDb = FindById(id);

                if (userFromDb != null)
                {
                    DropOracleUser(db, userFromDb);

                    DeleteFromUsersTable(id, db);
                }

            }
        }
        public void Delete(double id)
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var userFromDb = FindById(id);

                if (userFromDb != null)
                {
                    DropOracleUser(db, userFromDb);

                    DeleteFromUsersTable(id, db);
                }

            }
        }

        private static void DropOracleUser(IDbConnection db, ApplicationUser userFromDb)
        {
            string sqlQuery = $@"drop user {userFromDb.Username} CASCADE";
            db.Execute(sqlQuery);
        }

        private static void DeleteFromUsersTable(double id, IDbConnection db)
        {
            string sqlQuery = @"delete from dotnetcore.users where id = :id";

            db.Execute(sqlQuery, new { id });
        }

        private static void AlterOracleUser(ApplicationUser user, IDbConnection db, ApplicationUser userFromDb)
        {
            string sqlQuery = $@"Alter USER {user.Username} 
                        IDENTIFIED BY {user.PasswordHash} 
                        DEFAULT TABLESPACE space300 
                        QUOTA 0 ON {userFromDb.TableSpaceName}
                        QUOTA {user.Quota} ON {user.TableSpaceName}
                        PROFILE profile1";

            db.Execute(sqlQuery);
        }

        private static void UpdateInUsersTable(ApplicationUser user, IDbConnection db)
        {
            string sqlQuery = @"UPDATE dotnetcore.users
                    SET USERNAME = :Username,FirstName = :FirstName,LastName = :LastName,EMAIL = :Email,PHONE = :Phone
                    ,PasswordHash = :PasswordHash,QUOTA = :Quota,AccountStatus = :AccountStatus
                    ,TableSpaceName = :TableSpaceName,ProfileName = :ProfileName
                    WHERE ID = :Id";
            db.Execute(sqlQuery, user);
        }

        public ApplicationUser FindById(double id)
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                string sqlQuery = @"Select * from dotnetcore.users where ID = :id";

                return db.QuerySingle<ApplicationUser>(sqlQuery, new { id });
            }
        }
    }
}
