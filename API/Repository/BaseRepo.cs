using API.Base;
using API.Context;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class BaseRepo<TEntity, TContext> : IRepo<TEntity>
        where TEntity : class, BaseModel
        where TContext : MyContext
    {
        MyContext _context;
        public BaseRepo(MyContext context)
        {
            _context = context;
        }
        public async Task<int> Create(TEntity entity)
        {
            entity.CreateData = DateTimeOffset.Now;
            entity.isDelete = false;
            await _context.Set<TEntity>().AddAsync(entity);
            var createItem = await _context.SaveChangesAsync();
            return createItem;
        }

        public async Task<int> Delete(int Id)
        {
            var data = await GetID(Id);
            if (data == null)
            {
                return 0;
            }
            data.DeleteData = DateTimeOffset.Now;
            data.isDelete = true;
            _context.Entry(data).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            var data = await _context.Set<TEntity>().Where(x => x.isDelete == false).ToListAsync();
            if (!data.Count.Equals(0))
            {
                return data;
            }
            return null;
        }

        public virtual async Task<TEntity> GetID(int Id)
        {
            var data = await _context.Set<TEntity>().SingleOrDefaultAsync(x => x.Id == Id && x.isDelete == false);
            if (!data.Equals(0))
            {
                return data;
            }
            return null;
        }

        public async Task<int> Update(TEntity entity)
        {
            entity.UpdateDate = DateTimeOffset.Now;
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
