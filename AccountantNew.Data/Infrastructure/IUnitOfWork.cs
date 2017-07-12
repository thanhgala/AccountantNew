namespace AccountantNew.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}