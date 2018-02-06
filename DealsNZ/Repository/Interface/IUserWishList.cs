using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealsNZ.Models;
using RepoPattern.Models.RepositoryFiles;

namespace DealsNZ.Repository.Interface
{
    interface IUserWishList: _IRepositoryList<WishList>
    {
        IEnumerable<DealsModels.DealViewModel> ViewWishListByUser(int UserID);
        bool wishlistCheck(int DealId, int UserID);
    }
}
