
using DealsNZ.Models;
using DealsNZ.Repository.Interface;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealsNZ.Repository.ClassServices
{
    public class CompanyService : Repository<Company>,ICompany

    {
        protected readonly DealsDB DealDb;
        public CompanyService(DealsDB Data) : base(Data)
        {
            DealDb = Data;
        }

        public bool CreateCompany(Company _company)
        {
            Insert(_company);
            SaveChange();
            return true;
        }

       

        public Company GetCompanyById(int id)
        {
            return GetByID(id);
        }
        public void RemoveCompanybyId(int id)
        {
            Delete(GetByID(id));
        }

        public void UpdateCompany(Company _company)
        {
            DealDb.Entry(_company).State = System.Data.Entity.EntityState.Modified;
            SaveChange();
        }

    }
}