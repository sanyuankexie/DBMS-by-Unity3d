using System.Data.SqlClient;
using DBMS.Data.Entity;
using IvyOrm;
using System.Data;
using System;
using System.Collections.Generic;



namespace DBMS.Data
{
    public class MsSqlService : IDataService
    {



        protected readonly MsSqlConnectPool ConPool;

        public MsSqlService()
        {
            ConPool = new MsSqlConnectPool();
        }

        /*
                public void DeleteWhereID<T>(T o) where T : class, IDataEntity, new()
                {
                    int id = o.ID;
                    SqlConnection con = null;
                    try
                    {
                        con = ConPool.GetConnection();
                        string sql = $"Delete * from {GetTableName(typeof(T))} where id={id};";
                        UnityEngine.Debug.Log(sql);
                        con.ExecuteNonQuery(sql);
                        ConPool.ReleaseContent(con);
                    }
                    catch (SqlException)
                    {
                        string sql = $"select * from [{GetTableName(typeof(T))}] where id={id};";
                        UnityEngine.Debug.Log(sql);
                        con.ExecuteNonQuery(sql);
                        ConPool.ReleaseContent(con);
                    }
                    catch
                    {
                        if (con != null)
                        {
                            con.Dispose();
                            ConPool.RemoveContent(con);
                        }
                        throw;
                    }
                }*/

        public void Insert(object entity)
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                con.RecordInsert(entity);
                ConPool.ReleaseContent(con);
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public T[] Query<T>(string s) where T : class, new()
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                string sql = $"select * from {GetTableName(typeof(T))} {s};";
                UnityEngine.Debug.Log(sql);
                var ret = con.RecordQuery<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (SqlException)
            {
                try
                {
                    string sql = $"select * from [{GetTableName(typeof(T))}] {s};";
                    UnityEngine.Debug.Log(sql);
                    var ret = con.RecordQuery<T>(sql);
                    ConPool.ReleaseContent(con);
                    return ret;
                }
                catch
                {
                    if (con != null)
                    {
                        con.Dispose();
                        ConPool.RemoveContent(con);
                    }
                    throw;
                }
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public T[] QueryWhere<T>(string where) where T : class, new()
        {
            return Query<T>($"where {where}");
        }

        public DataSet ExecuteDataSet(string sql)
        {            
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                ConPool.ReleaseContent(con);
                return ds;
            }
            catch (SqlException)
            {
                ConPool.ReleaseContent(con);
                throw;
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public int ExecuteSql(string sql)
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                int ret = con.ExecuteNonQuery(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (SqlException)
            {
                ConPool.ReleaseContent(con);
                throw;
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public void DeleteEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                con.RecordDelete(entity);
                ConPool.ReleaseContent(con);
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public void InsertEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                con.RecordInsert(entity);
                ConPool.ReleaseContent(con);
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public T LoadEntity<T>(int serial) where T : class, IDataEntity, new()
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                string sql = $"select * from {GetTableName(typeof(T))} where id = {serial}";
                var ret = con.RecordSingleOrDefault<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (SqlException)
            {
                try
                {
                    string sql = $"select * from [{GetTableName(typeof(T))}] where id = {serial}";
                    var ret = con.RecordSingleOrDefault<T>(sql);
                    ConPool.ReleaseContent(con);
                    return ret;
                }
                catch
                {
                    if (con != null)
                    {
                        con.Dispose();
                        ConPool.RemoveContent(con);
                    }
                    throw;
                }
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public T[] LoadEntitys<T>() where T : class, IDataEntity, new()
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection(); ;
                string sql = $"select * from {GetTableName(typeof(T))}";
                T[] ret = con.RecordQuery<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (SqlException)
            {
                try
                {
                    string sql = $"select * from [{GetTableName(typeof(T))}]";
                    T[] ret = con.RecordQuery<T>(sql);
                    ConPool.ReleaseContent(con);
                    return ret;
                }
                catch
                {
                    if (con != null)
                    {
                        con.Dispose();
                        ConPool.RemoveContent(con);
                    }
                    throw;
                }
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        public void UpdateEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            SqlConnection con = null;
            try
            {
                con = ConPool.GetConnection();
                con.RecordUpdate(entity);
                ConPool.ReleaseContent(con);
            }
            catch
            {
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        private readonly Dictionary<Type, string> tableMap = new Dictionary<Type, string>();

        /// <summary>
        /// 获得某个类型所对应的数据库表名
        /// 这里通过 Type与string 的字典做缓存
        /// 如果定义过 TableAttribute 则使用里面的Table去访问数据库
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetTableName(Type type)
        {
            string tableName;
            if (tableMap.TryGetValue(type, out tableName))
            {
                return tableName;
            }
            var tableAtt = type.GetCustomAttributes(typeof(TableAttribute), true);
            if (tableAtt.Length > 0)
            {
                tableName = ((TableAttribute)tableAtt[0]).TableName;
            }
            else
            {
                tableName = type.Name;
            }
            tableMap[type] = tableName;
            return tableName;
        }
    }
}