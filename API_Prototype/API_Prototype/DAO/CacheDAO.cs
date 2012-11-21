using System;
using System.Collections.Generic;

namespace APIPrototype
{
    /// <summary>
    /// Implements RecommendationDAO for accessing Cache
    /// </summary>
    public class CacheDAO:RecommendationDAO
    {
        private string cacheName;
        private MemcachedCacheHelper cacheHelper;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CacheDAO()
        {
            this.cacheName = string.Concat(Constants.DEFAULT_CACHE_NAME, Utils.Utils.generateUniqueID());
            cacheHelper = MemcachedCacheHelper.getInstance(cacheName);
        }

        /// <summary>
        /// Overriden constructor
        /// </summary>
        /// <param name="cacheName"></param>
        public CacheDAO(string cacheName)
        {
            if(string.IsNullOrEmpty(cacheName)) //generate random name
            {
                this.cacheName = string.Concat(Constants.DEFAULT_CACHE_NAME, Utils.Utils.generateUniqueID());
            }
            else
            {
                this.cacheName = string.Concat(Constants.DEFAULT_CACHE_NAME, Utils.Utils.generateUniqueID());
            }

            cacheHelper = MemcachedCacheHelper.getInstance(this.cacheName);
        }
        

        #region RecommendationDAO Members

        public List<ProductRecommendation> getProductToProductRecommendation(long tenantId, long sourceId, string productKey, int limit)
        {
            string cacheKey = Constants.NA_CACHE_KEY;
            List<ProductRecommendation> cachedItem = null;
            try
            {
                cacheKey = cacheHelper.generateCacheKey("ProductToProductRecommendation", tenantId, sourceId, productKey);
                cachedItem =  cacheHelper.getCacheValue<List<ProductRecommendation>>(cacheKey);
            }
            catch (Exception exc)
            {
                //TODO: Log the error
                cachedItem = null;
            }

            return cachedItem;
        }

        public List<UserRecommendation> getUserToProductRecommendation(long tenantId, long sourceId, string userKey, int limit)
        {

            string cacheKey = Constants.NA_CACHE_KEY;
            List<UserRecommendation> cachedItem = null;
            try
            {
                cacheKey = cacheHelper.generateCacheKey("UserToProductRecommendation", tenantId, sourceId, userKey);
                cachedItem = cacheHelper.getCacheValue<List<UserRecommendation>>(cacheKey);
            }
            catch (Exception exc)
            {
                //TODO: Log the error
                cachedItem = null;
            }

            return cachedItem;

        }

        public void setProductToProductRecommendation(long tenantId, long sourceId, string productKey, int limit, List<ProductRecommendation> productRecommendations)
        {
            string cacheKey = Constants.NA_CACHE_KEY;
            try
            {
                cacheKey = cacheHelper.generateCacheKey("ProductToProductRecommendation", tenantId, sourceId, productKey);
                cacheHelper.setCacheValue<List<ProductRecommendation>>(cacheKey, productRecommendations);
            }
            catch (Exception exc)
            {
                //TODO: Log the error
            }
        }

        public void setUserToProductRecommendation(long tenantId, long sourceId, string userKey, int limit, List<UserRecommendation> userRecommendations)
        {
            string cacheKey = Constants.NA_CACHE_KEY;
            try
            {
                cacheKey = cacheHelper.generateCacheKey("UserToProductRecommendation", tenantId, sourceId, userKey);
                cacheHelper.setCacheValue<List<UserRecommendation>>(cacheKey, userRecommendations);
            }
            catch (Exception exc)
            {
                //TODO: Log the error
            }
        }

        #endregion
    }
}
