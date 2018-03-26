using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesSingleton
    {
        ZigNetEntities GetInstance();
    }
}
