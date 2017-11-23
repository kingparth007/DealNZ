using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DealsNZ.Models;

namespace RepoPattern.Models.RepositoryFiles
{
    public class Repository<T> : _IRepositoryList<T> where T : class
    {
        protected readonly DealsDB _DbContext;


        public Repository(DealsDB Data)
        {
            _DbContext = Data;
        }


        public void Delete(T Class)
        {
            _DbContext.Set<T>().Remove(Class);
        }

        public void DeleteRange(IEnumerable<T> Classes)
        {
            _DbContext.Set<T>().RemoveRange(Classes);
        }

        public IEnumerable<T> GetAll()
        {
            return _DbContext.Set<T>().ToList();
        }

        public T GetByID(int ClassID)
        {
            return _DbContext.Set<T>().Find(ClassID);
        }

        public void Insert(T Class)
        {
            _DbContext.Set<T>().Add(Class);

        }

        public void InsertRange(IEnumerable<T> Classes)
        {
            _DbContext.Set<T>().AddRange(Classes);


        }

        public void SaveChange()
        {
            _DbContext.SaveChanges();
        }


    }
}