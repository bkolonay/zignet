using System;


namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntitiesWrapper : IZigNetEntitiesWrapper, IDisposable
    {
        private ZigNetEntities _zigNetEntities = new ZigNetEntities();

        public ZigNetEntities Get()
        {
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
