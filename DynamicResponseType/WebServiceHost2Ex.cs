using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace DamianBlog
{
    class WebServiceHost2Ex : WebServiceHost
    {
        public WebServiceHost2Ex(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) {}

        protected override void OnOpening()
        {
            base.OnOpening();

            if (base.Description != null)
            {
                foreach (ServiceEndpoint endpoint in base.Description.Endpoints)
                {
                    if ((endpoint.Binding != null) && (endpoint.Binding.CreateBindingElements().Find<JSONPBindingElement>() != null))
                    {
                        if (endpoint.Behaviors.Find<WebHttpBehavior>() == null)
                        {
                            endpoint.Behaviors.Add(new WebHttpBehavior2Ex());
                        }
                    }
                }
            }

            //end
            foreach (var ep in this.Description.Endpoints)
            {
                if (ep.Behaviors.Find<WebHttpBehavior>() != null)
                {
                    ep.Behaviors.Remove<WebHttpBehavior>();
                    ep.Behaviors.Add(new WebHttpBehavior2Ex());
                }
            }
  
        }
    }
}
