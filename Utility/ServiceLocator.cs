using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Utility
{
    public sealed class ServiceLocator
    {
        static ServiceLocator instance = null;

        private readonly UnityContainer _unityContainer;

        private ServiceLocator() {
            _unityContainer = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Containers["mainContainer"].Configure(_unityContainer);
        }

        public static ServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceLocator();
                }
                return instance;
            }
        }

        public T Retrieve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        //public T Retrieve<T>(string key)
        //{
        //    return _unityContainer.Resolve<T>(key);
        //}
    }
}
