using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsNZ.Repository.Interface
{
    interface ICompany: _IRepositoryList<Company>
    {
        //for company
       
        Company GetCompanyById(int id);
        bool CreateCompany(Company _company);
        void RemoveCompanybyId(int id);
        void UpdateCompany(Company _company);
    }
}
