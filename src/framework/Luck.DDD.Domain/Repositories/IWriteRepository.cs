

namespace Luck.DDD.Domain.Repositories
{
    public interface IWriteRepository<TEntity, TKey> where TEntity : IEntity
    {

        /// <summary>
        /// 附加实体，将当前实体设置为 Unchanged 状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Attach(TEntity entity);

        void AttachRange(params object[] entities);

        void Add(TEntity entity);
        void AddRange(params object[] entities);
        void BulkInsert(params object[] entities);
        /// <summary>
        /// 更新实体，将当前实体设置为 Modfied 状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        void UpdateRange(params object[] entities);

        void Remove(TEntity entity);
        void RemoveRange(params object[] entities);
    }
}
