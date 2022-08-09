

namespace Luck.DDD.Domain.Repositories
{
    public interface IWriteRepository<in TEntity, TKey> where TEntity : IEntity
    {

        /// <summary>
        /// 附加实体，将当前实体设置为 Unchanged 状态
        /// </summary>
        /// <param name="entity"></param>
        void Attach(TEntity entity);



        void Add(TEntity entity);

        /// <summary>
        /// 更新实体，将当前实体设置为 Modfied 状态
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);


        void Remove(TEntity entity);
        
    }
}
