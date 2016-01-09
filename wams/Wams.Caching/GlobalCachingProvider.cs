using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Caching
{
    public class GlobalCachingProvider : CachingProviderBase, IGlobalCachingProvider
    {
        #region Singleton

        protected GlobalCachingProvider()
        {
        }

        public static GlobalCachingProvider Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            public static readonly GlobalCachingProvider instance = new GlobalCachingProvider();
        }

        #endregion

        #region ICachingProvider

        public virtual new void AddItem(string key, object value)
        {
            base.AddItem(key, value);
        }

        public virtual object GetItem(string key)
        {
            return base.GetItem(key, true);//Remove default is true because it's Global Cache!
        }

        public virtual new bool ItemExist(string key)
        {
            return base.ItemExist(key);
        }

        public string GetCacheKey(params string[] str)
        {
            var key = string.Empty;

            if (str.Length == 1)
            {
                return str[0];
            }

            for (var i = 0; i < str.Length; i++)
            {
                if (i == str.Length - 1)
                {
                    key += str[i];
                }
                else
                {
                    key += string.Format("{0}:", str[i]);
                }
            }

            return key;
        }

        #endregion

    } 
}
