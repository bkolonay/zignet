using System;

// fix issue outlined here: https://stackoverflow.com/a/29743758

namespace ZigNet.Database.EntityFramework
{
    public static class Fix
    {
        static void FixIssue()
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, ensures static reference to System.Data.Entity.SqlServer");
        }
    }
}
