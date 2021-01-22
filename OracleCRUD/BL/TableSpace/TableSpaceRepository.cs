using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BL.TableSpace
{
    public class TableSpaceRepository : ITableSpaceRepository
    {
        private readonly IConfiguration _configuration;

        public TableSpaceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<TableSpace> GetAll()
        {
            using (IDbConnection db = new OracleConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                return db.Query<TableSpace>
                ("Select * From dotnetcore.tablespaces").ToList();
            }
        }
    }
}
