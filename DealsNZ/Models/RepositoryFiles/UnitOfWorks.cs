using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DealsNZ.Models;

namespace RepoPattern.Models.RepositoryFiles
{
    public class UnitOfWorks<T> : _IRepositoryList<T> where T : class
    {

        private DealsDB _DbContaxt;
        private IDbSet<T> _DBClass;

        
        public UnitOfWorks()
        {
            _DbContaxt = new DealsDB();
            _DBClass = _DbContaxt.Set<T>();

        }

        public void DeleteClass(int ClassID)
        {
            T Class = _DBClass.Find(ClassID);
            _DBClass.Remove(Class);
        }

        public IEnumerable<T> GetClass()
        {
            
            return _DBClass.ToList();
        }

        public T GetClassByID(int ClassID)
        {
            return _DBClass.Find(ClassID);
        }

        public void InsertClass(T Class)
        {
            _DBClass.Add(Class);
        }

        public void SaveChange()
        {
            _DbContaxt.SaveChanges();
        }

        public void UpdateClass(T Class)
        {
            _DbContaxt.Entry(Class).State = EntityState.Modified;
        }

        public void List(String ClassName) {

        //    IDbSet<T> Class = ClassName
        //    _DBClass =  ClassName;
        }
    }
}