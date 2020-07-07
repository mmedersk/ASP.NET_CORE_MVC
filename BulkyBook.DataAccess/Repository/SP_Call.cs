using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _dbContext;
        private static string ConnectionString = "";

        public SP_Call(ApplicationDbContext context)
        {
            _dbContext = context;
            ConnectionString = context.Database.GetDbConnection().ConnectionString;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                sql.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                return sql.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                var result = SqlMapper.QueryMultiple(sql, procedureName, param, commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }

                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
            }
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                var value = sql.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
                return (T) Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                return (T)Convert.ChangeType(sql.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure), typeof(T));
            }
        }
    }
}