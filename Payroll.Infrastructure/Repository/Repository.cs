
using Payroll.Application.Interfaces;
using Payroll.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;


namespace Payroll.Infrastructure.Repository
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly AppDbContext context;
        internal DbSet<T> dbset;

        public Repository(AppDbContext context)
        {
            this.context = context;
            dbset = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeprop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }


    }
}

