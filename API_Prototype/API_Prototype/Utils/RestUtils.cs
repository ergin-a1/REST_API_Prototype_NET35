using System.ServiceModel.Web;
using System.Net;
using System.Collections.Generic;
using System.ServiceModel;

namespace APIPrototype
{
    /// <summary>
    /// All the utilities related to the RESTful API
    /// </summary>
    public class RestUtils
    {
        /// <summary>
        /// Returns response from rest service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="retVal"></param>
        /// <returns></returns>
        public static T RestResponse<T>(IWebOperationContext context , HttpStatusCode statusCode,object retVal)
        {
            context.OutgoingResponse.StatusCode = statusCode;
            return (T)retVal;
        }

        /// <summary>
        /// get the status code for returning object - basically check whether there is content available or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static HttpStatusCode getStatusCodeForContent<T>(object obj)
        {
            HttpStatusCode statusCode = HttpStatusCode.NotFound;
            if (obj != null)
            {
                //it's Collection Type - get size 
                if (obj.GetType().IsGenericType && obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
                {
                    IList<T> listObject = (IList<T>)obj;
                    if (listObject.Count > 0)
                    {
                        statusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        throw new NotFoundException("No item was found");
                    }
                }
                else //it's not Collection Type - return OK.
                {
                    statusCode = HttpStatusCode.OK;
                }
            }
            else
            {
                throw new NotFoundException("No item was found");
            }

            return statusCode;
        }

    }
}
