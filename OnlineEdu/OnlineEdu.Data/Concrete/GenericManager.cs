using Microsoft.EntityFrameworkCore;
using OnlineEdu.Data.Abstract;
using OnlineEdu.Data.EduContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Data.Concrete
{
    public class GenericManager<T>(EduDbContext _context) : IGeneric<T> where T : class
    {
        public DbSet<T> Table { get => _context.Set<T>(); }
        public void Add(T entity)
        {
            Table.Add(entity);
            _context.SaveChanges();
        }

        public int Count()
        {
            return Table.Count();
        }

        public void Delete(int id)
        {
            var entity = Table.Find(id);
            if (entity != null)
            {
                Table.Remove(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Entity not found");
            }
        }

        public int FiltheredCount(Expression<Func<T, bool>> filter)
        {
            return Table.Where(filter).Count();
        }

        public T GetById(int id)
        {
            var entity = Table.Find(id);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }
            return entity;
        }

        public T GetFilthered(Expression<Func<T, bool>> filter)
        {
            return Table.Where(filter).FirstOrDefault();
        }

        public List<T> GetList()
        {
            return Table.ToList();
        }

        public List<T> GetListFilthered(Expression<Func<T, bool>> filter)
        {
            return Table.Where(filter).ToList();
        }

        public void Update(T entity)
        {
            Table.Update(entity);
            _context.SaveChanges();
        }
    }
}
