using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using BabySitterClubService.InstanceProvider;

namespace BabySitterClubService.ServiceBehaviours
{
    public class StructureMapServiceBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceProvider =
                            new StructureMapInstanceProvider(serviceDescription.ServiceType);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            throw new NotImplementedException();
        }
    }
}