using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading;
namespace DBMS.Utils
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> where T : new()
    {

        #region zh-CHS 私有成员变量 | en Private Member Variables

        /// <summary>
        /// 内存池的容量
        /// </summary>
        private readonly long m_InitialCapacity;

        /// <summary>
        /// 最大持有容量
        /// 如果发生miss，仍然会按照初始容量去扩容
        /// 但是，一旦发生回收，池里的数量大于最大容量，则不会再往池里丢数据
        /// 会直接抛弃掉
        /// </summary>
        public int MaxCapacity { get; set; }

        /// <summary>
        /// 内存池
        /// </summary>
        protected ConcurrentQueue<T> m_FreePool = new ConcurrentQueue<T>();

        /// <summary>
        /// 内存池的容量不足时再次请求数据的次数
        /// </summary>
        private long m_Misses;

        #endregion

        #region zh-CHS 构造和初始化和清理 | en Constructors and Initializers and Dispose

        /// <summary>
        /// 初始化内存池
        /// </summary>
        /// <param name="iInitialCapacity">初始化内存池对象的数量</param>
        /// <param name="maxCapacity">最大容量</param>
        public ObjectPool(long iInitialCapacity = 64, int maxCapacity = int.MaxValue)
        {
            m_InitialCapacity = iInitialCapacity;
            MaxCapacity = maxCapacity;

            for (int iIndex = 0; iIndex < iInitialCapacity; ++iIndex)
                m_FreePool.Enqueue(new T());
        }

        /// <summary>
        /// 扩展数据
        /// </summary>
        private void Extend()
        {
            for (int iIndex = 0; iIndex < m_InitialCapacity; ++iIndex)
            {
                newCount++;
                m_FreePool.Enqueue(new T());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ~ObjectPool()
        {
            Debug.Log(ToString());
            Debug.Log(ToString());
        }

        /// <summary>
        /// 输出对象池的一些状态
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(512);
            ret.AppendFormat("FreeCount:{0}\r\n", m_FreePool.Count);
            ret.AppendFormat("InitialCapacity:{0}\r\n", m_InitialCapacity);
            ret.AppendFormat("NewCount:{0}\r\n", newCount);
            ret.AppendFormat("AcquireCount:{0}\r\n", acquireCount);
            ret.AppendFormat("ReleaseCount:{0}\r\n", releaseCount);

            return ret.ToString();
        }

        #endregion

        private long acquireCount;

        private long releaseCount;

        private int newCount;

        #region zh-CHS 共有方法 | en Public Methods

        /// <summary>
        /// 内存池请求数据
        /// </summary>
        /// <returns></returns>
        public T AcquireContent()
        {
            //lock (this)
            {
                T returnT;

                do
                {
                    if (m_FreePool.TryDequeue(out returnT))
                        break;

#if !UNITY_IPHONE
                    lock (m_FreePool)
#endif
                    {
                        m_Misses++;
                        Extend();
                    }

                } while (true);

                //Interlocked.Increment(ref acquireCount);
#if UNITY_IPHONE
                acquireCount++;
#else
                Interlocked.Increment(ref acquireCount);
#endif


#if NET40 && DEBUG
                checkPool.Add(returnT);
#endif
                return returnT;
            }
        }


        /// <summary>
        /// 内存池释放数据
        /// </summary>
        /// <param name="content"></param>
        public void ReleaseContent(T content)
        {
            if (content == null)
                throw new ArgumentNullException("content",
                    "MemoryPool.ReleasePoolContent(...) - contentT == null error!");
            Interlocked.Increment(ref releaseCount);
            if (m_FreePool.Count < MaxCapacity)
                m_FreePool.Enqueue(content);
        }

        /// <summary>
        /// 释放内存池内全部的数据
        /// </summary>
        public void Free()
        {
            m_FreePool = new ConcurrentQueue<T>();
        }
        #endregion
    }
}
