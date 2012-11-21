using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

#if Mock
using WebOperationContext = System.ServiceModel.Web.MockedWebOperationContext;
#endif 

namespace APIPrototype
{
    // NOTE: If you change the class name "APIPrototypeService" here, you must also update the reference to "APIPrototypeService" in Web.config.
    public class APIPrototypeService : IAPIPrototypeService
    {

        private bool _cacheSupport;

        public bool cacheSupport
        {
            get
            {
                bool.TryParse(Utils.Utils.GetAppSetting(Constants.CONFIG_ENABLE_CACHE), out _cacheSupport);
                return _cacheSupport;
            }

        }

        /// <summary>
        /// Gets product to product recommendation no data, caching or etc - use IRecommendations for business layer
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="productKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Result productToProductRecommendation(string tenantId, string sourceId, string productKey)
        {
            #region shared variables

            Result result = new Result();

            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            List<ProductRecommendation> lstProductRecommendations = null;
            long _tenantId = 0;
            long _sourceId = 0;
 
            #endregion shared variables

            try
            {
                #region param validation

                tenantId.isValidId("Tenant ID");
                sourceId.isValidId("Source ID");
                productKey.requireNotNullOrEmpty("Product Key");

                _tenantId = long.Parse(tenantId);
                _sourceId = long.Parse(sourceId);

                #endregion param validation

                #region access to business logic layer

                AbstractRecommendations productRecommendations = new Recommendations(cacheSupport, null);
                lstProductRecommendations = productRecommendations.productToProductRecommendation(_tenantId, _sourceId, productKey, Constants.NUMBER_OF_RECOMMENDATIONS);

                #endregion access to business logic layer


                #region set return value

                //get status code - if no data not founds
                statusCode = RestUtils.getStatusCodeForContent<ProductRecommendation>(lstProductRecommendations);

                //Assign tenantId to result object
                result.tenantId = _tenantId;
                result.resultSet = lstProductRecommendations;
   
                #endregion set return value
            }
            catch (TenantNotFoundException txc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(txc.message, Constants.ERR_TENANT_NOT_FOUND);
                statusCode = txc.statusCode;
            }
            catch (NotFoundException nxc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(nxc.message, Constants.ERR_ITEM_NOT_FOUND);
                statusCode = nxc.statusCode;
            }
            catch (ArgumentException axc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(axc.Message, Constants.ERR_ARGUMENT_EXCEPTION);
                statusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception exc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(exc.Message, Constants.ERR_GENERIC_EXCEPTION);
                statusCode = HttpStatusCode.BadRequest;
            }
            return RestUtils.RestResponse<Result>(WebOperationContext.Current, statusCode, result);
        }

        /// <summary>
        /// Gets user to product recommendation no data, caching or etc - use IRecommendations for business layer
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="sourceId"></param>
        /// <param name="userKey"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Result userToProductRecommendation(string tenantId, string sourceId, string userKey)
        {
            #region shared variables

            Result result = new Result();

            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            List<UserRecommendation> lstuserRecommendations = null;
            long _tenantId = 0;
            long _sourceId = 0;
            

            #endregion shared variables

            try
            {
                #region param validation

                tenantId.isValidId("Tenant ID");
                sourceId.isValidId("Source ID");
                userKey.requireNotNullOrEmpty("User Key");

                _tenantId = long.Parse(tenantId);
                _sourceId = long.Parse(sourceId);
                

                #endregion param validation

                #region access to business logic layer

                AbstractRecommendations userRecommendations = new Recommendations(cacheSupport, null);
                lstuserRecommendations = userRecommendations.userToProductRecommendation(_tenantId, _sourceId, userKey, Constants.NUMBER_OF_RECOMMENDATIONS);

                #endregion access to business logic layer

                #region set return value

                //get status code - if no data not founds
                statusCode = RestUtils.getStatusCodeForContent<UserRecommendation>(lstuserRecommendations);

                //Assign tenantId to result object
                result.tenantId = _tenantId;
                result.resultSet = lstuserRecommendations;
                #endregion set return value
            }
            catch (TenantNotFoundException txc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(txc.message, Constants.ERR_TENANT_NOT_FOUND);
                statusCode = txc.statusCode;
            }
            catch (NotFoundException nxc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(nxc.message, Constants.ERR_ITEM_NOT_FOUND);
                statusCode = nxc.statusCode;
            }
            catch (ArgumentException axc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(axc.Message, Constants.ERR_ARGUMENT_EXCEPTION);
                statusCode = HttpStatusCode.BadRequest;
            }
            catch (Exception exc)
            {
                //TODO: Log the error
                long.TryParse(tenantId, out _tenantId);
                result.tenantId = _tenantId;
                result.error = new RestError(exc.Message, Constants.ERR_GENERIC_EXCEPTION);
                statusCode = HttpStatusCode.BadRequest;
            }
            return RestUtils.RestResponse<Result>(WebOperationContext.Current, statusCode, result);
        }
    }
}
