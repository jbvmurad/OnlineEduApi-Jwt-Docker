using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEdu.Data.Abstract
{
    public interface IGeneric <T> where T : class
    {
        List<T> GetList();
        T GetFilthered(Expression<Func<T, bool>> filter);
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        int Count();
        int FiltheredCount(Expression<Func<T, bool>> filter);
        List<T> GetListFilthered(Expression<Func<T, bool>> filter);
    }
}
