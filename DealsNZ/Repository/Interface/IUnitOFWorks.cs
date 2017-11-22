using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoPattern.Models.RepositoryFiles
{
    interface IUnitOFWorks : IDisposable
    {
       // ICarDetail CarDetails { get; }

        int Complete();
    }
}
