using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DealsNZ.Models.DealsModels;

namespace DealsNZ.Repository.Interface
{
    interface IDeal: _IRepositoryList<Deal>
    {
        int CreateDeal(Deal deal);
        void RemoveDealbyId(int id);
        void UpdateDeal(Deal deal);
    //    IEnumerable<Deal> Alldeal();
   IEnumerable< DealsModels.DealViewModel> AllDeal();

        ViewSingleDeal GetSingleDeal(int Id);
        


    }
}
