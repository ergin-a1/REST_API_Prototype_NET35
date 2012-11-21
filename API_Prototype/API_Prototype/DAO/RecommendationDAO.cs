using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIPrototype
{
    /// <summary>
    /// Recommendation layer DAO - strictly for data access
    /// </summary>
    public interface RecommendationDAO
    {
        List<ProductRecommendation> getProductToProductRecommendation(long tenantId, long sourceId, string productKey, int limit);

        List<UserRecommendation> getUserToProductRecommendation(long tenantId, long sourceId, string userKey, int limit);

        void setProductToProductRecommendation(long tenantId, long sourceId, string productKey, int limit, List<ProductRecommendation> productRecommendations);

        void setUserToProductRecommendation(long tenantId, long sourceId, string userKey, int limit, List<UserRecommendation> userRecommendations);
    }
}
