using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Common
{
    public sealed class Ioc
    {
        private static Ioc inst = null;
        private static readonly object padlock = new object();

        private readonly IUnityContainer container;

        private Ioc()
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container = new UnityContainer().LoadConfiguration(section);
        }

        public static Ioc Instance { 
            get {
                lock (padlock)
                {
                    if (inst == null)
                    {
                        inst = new Ioc();
                    }
                }

                return inst;
            } 
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
