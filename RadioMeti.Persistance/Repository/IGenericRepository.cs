using RadioMeti.Domain.Entities.Base;
namespace RadioMeti.Persistance.Repository
{
    public interface IGenericRepository<TEntity> where TEntity :BaseEntity
    {
        Task AddEntity(TEntity entity);
        void EditEntity(TEntity entity);
        Task<TEntity> GetEntityById(long entityId);
        void DeleteEntity(TEntity entity);
        Task DeleteEntity(long entityId);
        void DeletePermanet(TEntity entity);
        Task DeletePermanet(long entityId);
        Task SaveChangesAsync();
        IQueryable<TEntity> GetQuery();
    }
}
