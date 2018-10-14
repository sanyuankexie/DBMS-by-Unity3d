using DBMS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;
namespace DBMS.Data
{
    public class MsSqlConnectPool : ObjectPool<SqlConnection>
    {

        /// <summary>
        /// 只创建10个就OK了
        /// </summary>
        public MsSqlConnectPool()
            : base(10)
        {
            m_connectStr = @"server=.;uid=sa;pwd=123456;database=人事档案信息管理数据库";
        }

        /// <summary>
        /// 清理所有连接，释放对应的资源
        /// </summary>
        public void ClearConnection()
        {
            timeMap.Clear();

            while (!m_FreePool.IsEmpty)
            {
                SqlConnection con;
                if (m_FreePool.TryDequeue(out con))
                {
                    con.Dispose();
                }
            }

            GC.Collect();
        }

        private Dictionary<SqlConnection, DateTime> timeMap = new Dictionary<SqlConnection, DateTime>();

        /// <summary>
        /// 获得一个连接对象，注意，使用完后要返回连接池
        /// 方法内部会初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            var con = AcquireContent();

            DateTime dropTime;
            if (timeMap.TryGetValue(con, out dropTime))
            {
                if (DateTime.Now > dropTime)
                {
                    //  超过一天了，这个sql连接需要抛弃
                    try
                    {
                        Debug.Log($"drop mysql connect. {con.GetHashCode()}");
                        con.Close();
                        con.Dispose();
                        timeMap.Remove(con);
                    }
                    catch
                    {
                    }

                    con = new SqlConnection();
                    timeMap[con] = DateTime.Now.AddDays(1);
                }
            }
            else
            {
                timeMap[con] = DateTime.Now.AddDays(1);
            }

            if (con.State == ConnectionState.Open)
            {
                return con;
            }
            //Debug.Log($"set connestring {m_connectStr}");
            con.ConnectionString = m_connectStr;
            con.Open();
            return con;
        }

        /// <summary>
        /// 移除一个错误的对象
        /// </summary>
        /// <param name="con"></param>
        public void RemoveContent(SqlConnection con)
        {
            timeMap.Remove(con);
        }


        string m_connectStr = @"server=.;uid=sa;pwd=123456;database=人员管理数据库";

        /// <summary>
        /// 数据库的连接字符串
        /// </summary>
        public string ConnectString
        {
            get { return m_connectStr; }
        }
    }
}