using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIPrototype
{
    /// <summary>
    /// Interface for Business Logic Layer - implemented by Recommendations class
    /// </summary>
    public abstract class AbstractRecommendations
    {
        /// <summary>
        /// CacheManager
        /// </summary>
        private CacheDAO cacheDAO;

        /// <summary>
        /// indicates if cacheEnable
        /// </summary>
        protected bool isCacheEnabled { get; set; }

        protected void setCacheManager(string cacheName)
        {
            cacheDAO = new CacheDAO(cacheName);
        }


        /// <summary>
        /// productToProductRecommendation - BLL
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="productKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public virtual List<ProductRecommendation> productToProductRecommendation(long tenantId, long sourceId, string productKey, int limit)
        {
            List<ProductRecommendation> productRecos = null;
            DAOFactory daoObject = DAOFactory.getDAOFactory();
            RecommendationDAO recommendationDAO = daoObject.getRecommendationDAO(tenantId);

            if (this.isCacheEnabled) //Check Cache first
            {
                productRecos = cacheDAO.getProductToProductRecommendation(tenantId, sourceId, productKey, limit);
             
                //if no result - 
                if (productRecos==null)
                {
                    //get data from persistent
                    productRecos = recommendationDAO.getProductToProductRecommendation(tenantId, sourceId, productKey, limit);
               
                    //TODO: Check this section for performance
                    cacheDAO.setProductToProductRecommendation(tenantId, sourceId, productKey, limit, productRecos);


                }
            }
            else //Go to persistent
            {
                productRecos = recommendationDAO.getProductToProductRecommendation(tenantId, sourceId, productKey, limit);

            }
            return productRecos;
        }

        /// <summary>
        /// userToProductRecommendation - BLL
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="userKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public virtual List<UserRecommendation> userToProductRecommendation(long tenantId, long sourceId, string userKey, int limit)
        {
            List<UserRecommendation> userRecos = null;
            DAOFactory daoObject = DAOFactory.getDAOFactory();
            RecommendationDAO recommendationDAO = daoObject.getRecommendationDAO(tenantId);

            if (this.isCacheEnabled) //Check Cache first
            {
                userRecos = cacheDAO.getUserToProductRecommendation(tenantId, sourceId, userKey, limit);

                //if no result - 
                if (userRecos == null)
                {
                    //get data from persistent
                    userRecos = recommendationDAO.getUserToProductRecommendation(tenantId, sourceId, userKey, limit);

                    cacheDAO.setUserToProductRecommendation(tenantId, sourceId, userKey, limit, userRecos);
                }
            }
            else //Go to persistent
            {
                userRecos = recommendationDAO.getUserToProductRecommendation(tenantId, sourceId, userKey, limit);

            }

            return userRecos;
        }

    }
}
