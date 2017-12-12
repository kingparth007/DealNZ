using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepoPattern.Models.RepositoryFiles
{
    public interface _IRepositoryList<T> where T : class
    {
        IEnumerable<T> GetAll();

        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);

        T GetByID(int ID);

        void Insert(T Class);
        void InsertRange(IEnumerable<T> Classes);

        void Delete(T Class);
        void DeleteRange(IEnumerable<T> Classes);

        //  void UpdateClass(T Class);
        void SaveChange();
        void Dispose();

    }
}
