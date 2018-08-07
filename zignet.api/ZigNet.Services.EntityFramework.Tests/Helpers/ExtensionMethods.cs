using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ZigNet.Services.EntityFramework.Tests.Helpers
{
    public static class ExtensionMethods
    {
        public static Mock<DbSet<T>> ToDbSetMock<T>(this IEnumerable<T> list) where T : class
        {
            var listAsQueryable = list.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(listAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(listAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(listAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(listAsQueryable.GetEnumerator());

            return dbSetMock;

            // code taken from:
            // https://www.jankowskimichal.pl/en/2016/01/mocking-dbcontext-and-dbset-with-moq/
            // https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx
        }
    }
}
