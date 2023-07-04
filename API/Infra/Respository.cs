using API.Entities;
using API.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Infra
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private DbSet<T> DbSet => _dataContext.Set<T>();

        public T Create(T entity)
        {
            _dataContext.Add(entity);
            _dataContext.Entry(entity).State = EntityState.Added;
            _dataContext.SaveChanges();

            return entity;
        }

        public Result<T> Get(int page, int qtd)
        {
            qtd = qtd == 0 ? 10 : qtd;
            var result = new Result<T>
            {
                Page = page,
                Qtd = qtd,
                Data = DbSet.Where(x => x.Deleted == 0).Skip((page - 1) * qtd).Take(qtd).AsNoTracking().ToList()
            };

            result.Total = result.Data.Count;

            return result;
        }

        public T Get(string id) => DbSet.Where(x => x.Deleted == 0 && x.Id == id).AsNoTracking().FirstOrDefault();


        public void Remove(string id)
        {
            var entity = Get(id);
            entity.Deleted = 1;
            _dataContext.Update(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public void Update(string id, T entity)
        {
            _dataContext.Update(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }
    }
}
