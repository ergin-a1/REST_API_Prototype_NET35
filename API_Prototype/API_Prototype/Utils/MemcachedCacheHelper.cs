using System;
using BeIT.MemCached;

namespace APIPrototype
{
    public class MemcachedCacheHelper
    {
        private static volatile MemcachedCacheHelper instance;
        private static object syncRoot = new object();


        private static string cacheName;
        private static string[] serverList;
        private static TimeSpan cacheExpiry;
        private static MemcachedClient cache;

        /// <summary>
        /// 
        /// </summary>
        private MemcachedCacheHelper() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheName"></param>
        private MemcachedCacheHelper(string cacheName)
        {
            if (string.IsNullOrEmpty(cacheName))
                throw new Exception();

            if (cache == null)
            {
                serverList = Utils.Utils.GetAppSetting(Constants.CONFIG_MEMCACHED_SERVERLIST).Split(Constants.ITEM_SEPERATOR.ToCharArray());
                initialize(cacheName, serverList);
            }
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static MemcachedCacheHelper getInstance(string cacheName)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new MemcachedCacheHelper(cacheName);
                }
            }
            return instance;
        }


   
        public void initialize(string cacheName, string[] serverList)
        {
            try
            {
                //set cache expiration from config
                double expiry;
                double.TryParse(Utils.Utils.GetAppSetting(Constants.CONFIG_MEMCACHED_EXPIRY),out expiry);
                cacheExpiry = TimeSpan.FromHours(expiry);
                //set client
                MemcachedClient.Setup(cacheName, serverList);
                cache = BeIT.MemCached.MemcachedClient.GetInstance(cacheName);
            }
            catch (Exception exc)
            {
                //TODO: Log the error
            }
        }

        /// <summary>
        /// do not use generic types in args
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string generateCacheKey(string methodName, params object[] args)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new Exception(Constants.ERR_CACHEKEY_GENERATION_EXCEPTION);

            string retVal = methodName;

            foreach (var item in args)
            {
                if(item!=null && !item.GetType().IsGenericType)
                {
                    retVal += "_" + item.ToString();
                }
                else
                {
                    retVal += Constants.NULL_CACHE_KEY_ITEM;
                }
            }

            return retVal;
        }

        public void setCacheValue<T>(string key, T value)
        {
            bool retVal = false;

            if (cache != null)
            {
                retVal =  cache.Set(key, value, cacheExpiry);
            }

            if (!retVal)
            {
                //TODO: Log the info here
            }
        }


        public T getCacheValue<T>(string key)
        {
             if (cache == null) 
                return default(T);

            return (T)cache.Get(key);
        }

    }
}
