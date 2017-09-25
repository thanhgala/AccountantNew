using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface IApplicationCateGroupRepository : IRepository<ApplicationCateGroup>
    {
        
    }

    public class ApplicationCateGroupRepository : RepositoryBase<ApplicationCateGroup>, IApplicationCateGroupRepository
    {
        public ApplicationCateGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
