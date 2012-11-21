using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DamianBlog
{
    public class ServiceHostFactory2Ex : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, 
                                                         Uri[] baseAddresses)
        {


            WebServiceHost2Ex webServiceHost2Ex = 
                new WebServiceHost2Ex(serviceType, baseAddresses);

            //new code
            Uri[] defaultAddresses = new Uri[1];
            defaultAddresses[0] = baseAddresses[0];

            // Bind up the JSONP extension            
            CustomBinding cb = new CustomBinding(new WebHttpBinding());
            cb.Name = "JSONPBinding";

            // Replace the current MessageEncodingBindingElement with the JSONP element
            var currentEncoder = cb.Elements.Find<MessageEncodingBindingElement>();
            if (currentEncoder != default(MessageEncodingBindingElement))
            {
                cb.Elements.Remove(currentEncoder);
                cb.Elements.Insert(0, new JSONPBindingElement());
            }

            webServiceHost2Ex.AddServiceEndpoint(serviceType.GetInterfaces()[0], cb, defaultAddresses[0]);


            return webServiceHost2Ex;
        }
    }
}
