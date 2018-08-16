namespace ZigNet.Database.EntityFramework
{
    public interface IDbContext
    {
        ZigNetEntities Get();
    }
}
