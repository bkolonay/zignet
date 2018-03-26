using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntitiesSingleton : IZigNetEntitiesSingleton, IDisposable
    {
        private ZigNetEntities _zigNetEntities;

        // todo: unit test this
        public ZigNetEntities GetInstance()
        {
            if (_zigNetEntities == null)
            {
                _zigNetEntities = new ZigNetEntities();
#if DEBUG
                _zigNetEntities.Database.Log = s => Debug.WriteLine(s);
#endif
            }
            return _zigNetEntities;
        }

        // disposal code copied directly from: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/dependency-injection
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_zigNetEntities != null)
                {
                    _zigNetEntities.Dispose();
                    _zigNetEntities = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
