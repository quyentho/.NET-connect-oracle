using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace DbConnect
{
    public class DBConnect
    {
        //Create a connection to Oracle			
        private string conString = "User Id=sys;Password=123456;" +
            "Data Source=DESKTOP-UPOOFN0:1521/orclpdb.localdomain;DBA Privilege=SYSDBA";

        private OracleCommand _command = null;
        private OracleConnection _connection = null;
        private OracleDataAdapter _dataAdapter= null;

        public DBConnect()
        {
            _connection = new OracleConnection(conString);
            _command = _connection.CreateCommand();
        }

        public DataSet ExecuteQueryDataSet(string SQLQuery, CommandType commandType)
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();

            _connection.Open();

            _command.CommandText = SQLQuery;
            _command.CommandType = commandType;
            _dataAdapter = new OracleDataAdapter(_command);
            OracleDataReader reader = _command.ExecuteReader();

            DataSet ds = new DataSet();
            
            _dataAdapter.Fill(ds);
            return ds;
        }

        public bool ExecuteNonQuery(string SQLQuery, CommandType CommandType, ref string error)
        {
            bool isSucess = false;
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Open();
            _command.CommandText = SQLQuery;
            _command.CommandType = CommandType;
            try
            {
                _command.ExecuteNonQuery();
                isSucess = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                _connection.Close();
            }
            return isSucess;
        }
    }
}
