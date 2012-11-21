using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using DamianBlog;

namespace APIPrototype
{
    // NOTE: If you change the interface name "APIPrototype" here, you must also update the reference to "APIPrototype" in Web.config.
    /// <summary>
    /// DO NOT forget to add types to service for result set requirements
    /// </summary>
    [ServiceContract]
    [ServiceKnownType(typeof(RestError))]
    [ServiceKnownType(typeof(List<ProductRecommendation>))]
    [ServiceKnownType(typeof(List<UserRecommendation>))]
    [ServiceKnownType(typeof(Result))]
    public interface IAPIPrototypeService
    {
        [OperationContract]
        [WebInvoke(Method ="GET",
            UriTemplate = "/10/tenant/{tenantId}/product/{sourceId}/{productKey}/recommendations/crosssell")]
        [DynamicResponseType]
        [JSONPBehavior(callback = "callback")]
        [APISecurity(IsAuthenticationEnabled=false)]
        Result productToProductRecommendation(string tenantId, string sourceId, string productKey);


        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "/10/tenant/{tenantId}/user/{sourceId}/{userKey}/recommendations/product")]
        [DynamicResponseType]
        [JSONPBehavior(callback = "callback")]
        [APISecurity(IsAuthenticationEnabled = false)]
        Result userToProductRecommendation(string tenantId, string sourceId, string userKey);

    }
}
