using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealsNZ.Repository.Interface
{
    interface IDealImage: _IRepositoryList<DealImage>
    {
        int CreateDealImage(DealImage dealImage);
        IEnumerable<DealImage> GetDealImageByDealId(int id);
    }
   
}
