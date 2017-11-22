using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoPattern.Models.RepositoryFiles
{
    public interface _IRepositoryList<T> where T : class
    {
        IEnumerable<T> GetClass();

        T GetClassByID(int ClassID);

        void InsertClass(T Class);
        void InsertRange(IEnumerable<T> Classes);

        void DeleteClass(T Class);
        void DeleteRange(IEnumerable<T> Classes);

      //  void UpdateClass(T Class);
        void SaveChange();


    }
}
