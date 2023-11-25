using Assiment.core.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Assiment.core.Interfaces;
using ECommerce.Data.UnitOfWork;

namespace Assiment.EF.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : BaseModel
    {
        ApplicationDbContext Context;
        public GenericRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        
        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }


        public T GetByID(int id)
        {
            return Context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public T Add(T entity)
        {

            Context.Set<T>().Add(entity);
            return entity;
        }

        public void Update(T entity)
        {
            Context.Update(entity);
        }

        public void Update(T entity, params string[] properties)
        {
            
            var localEntity = Context.Set<T>().Local.Where(x => x.Id==entity.Id).FirstOrDefault();

            EntityEntry entityEntry;

           if (localEntity is null)
           {
                entityEntry = Context.Set<T>().Entry(entity);
            }
            else
            {
                entityEntry =
                    Context.ChangeTracker.Entries<T>()
                    .Where(x => x.Entity.Id == entity.Id).FirstOrDefault();
            }
            foreach (var property in entityEntry.Properties)
            {
                if (properties.Contains(property.Metadata.Name))
                {
                    property.CurrentValue = entity.GetType().GetProperty(property.Metadata.Name).GetValue(entity);
                    property.IsModified = true;
                }
            }

        }

        public void Delete(T entity)
        {
            Update(entity, nameof(entity.IsDeleted));

        }

        public int GetCount()
        {
            int count=Context.Set<T>().Count();
            return count;
        }
    }
}
