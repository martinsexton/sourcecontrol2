using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using StructureMap;

namespace BabySitterClubService.InstanceProvider
{
    public class StructureMapInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        public StructureMapInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return ObjectFactory.GetInstance(_serviceType);
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance)
        {
            throw new NotImplementedException();
        }
    }
}