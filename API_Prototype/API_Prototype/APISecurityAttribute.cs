using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace APIPrototype
{
    /// <summary>
    /// Add security layer to here - NOT IMPLEMENTED YET!
    /// </summary>
    public class APISecurityAttribute : Attribute, IOperationBehavior,IParameterInspector
    {
        public bool IsAuthenticationEnabled { get; set; }

        #region IOperationBehavior Members

        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        void IOperationBehavior.ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            if (IsAuthenticationEnabled)
            {
                dispatchOperation.ParameterInspectors.Add(this);
            }
        }

        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    
        #region IParameterInspector Members

        public void  AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        public object  BeforeCall(string operationName, object[] inputs)
        {
            //Object x = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
            return null;
        }

        #endregion
    }
}


