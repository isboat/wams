using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wams.Caching
{
    using System.Runtime.Caching;

    public abstract class CachingProviderBase
    {
        protected CachingProviderBase()
        {
            this.DeleteLog();
        }

        protected MemoryCache Cache = new MemoryCache("CachingProvider");

        static readonly object Padlock = new object();

        protected virtual void AddItem(string key, object value)
        {
            lock (Padlock)
            {
                this.Cache.Add(key, value, DateTimeOffset.MaxValue);
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (Padlock)
            {
                this.Cache.Remove(key);
            }
        }

        protected virtual object GetItem(string key, bool remove)
        {
            lock (Padlock)
            {
                var res = this.Cache[key];

                if (res != null)
                {
                    if (remove)
                        this.Cache.Remove(key);
                }
                else
                {
                    this.WriteToLog("CachingProvider-GetItem: Don't contains key: " + key);
                }

                return res;
            }
        }

        protected virtual bool ItemExist(string key)
        {
            lock (Padlock)
            {
                return this.Cache.Contains(key);
            }
        }

        #region Error Logs

        readonly string logPath = System.Environment.GetEnvironmentVariable("TEMP");

        protected void DeleteLog()
        {
            System.IO.File.Delete(string.Format("{0}\\CachingProvider_Errors.txt", this.logPath));
        }

        protected void WriteToLog(string text)
        {
            using (System.IO.TextWriter tw = System.IO.File.AppendText(string.Format("{0}\\CachingProvider_Errors.txt", this.logPath)))
            {
                tw.WriteLine(text);
                tw.Close();
            }
        }

        #endregion
    } 
}
