using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface IOrganizationalRepository : IRepository<Organizational>
    {

    }

    public class OrganizationalRepository : RepositoryBase<Organizational>, IOrganizationalRepository
    {
        public OrganizationalRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
