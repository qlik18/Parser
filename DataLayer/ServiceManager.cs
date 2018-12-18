using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.HPDataService;
using System.ServiceModel;

namespace DataLayer
{
    public sealed class ServiceManager
    {
        private EventHandler handler;
        private ChannelFactory<IDataService4HPChannel> factory;
        private IDataService4HPChannel clientHP;

        public ServiceManager(EventHandler onFaultEvent)
        {
            factory = new ChannelFactory<IDataService4HPChannel>("WSHttpBinding_IDataService4HP");
            clientHP = factory.CreateChannel();
            this.handler = onFaultEvent;
            clientHP.Faulted += new EventHandler(Channel_Fault);
        }

        private void Channel_Fault(object sender, EventArgs e)
        {
            clientHP = factory.CreateChannel();
            handler(sender, e);
        }
        public IDataService4HPChannel HPService
        {
            get
            {
                if (clientHP.State == CommunicationState.Faulted)
                    Channel_Fault(this, new EventArgs());
                return clientHP;
            }
        }
    }
}
