using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface IApplicationRoleGroupRepository : IRepository<ApplicationRoleGroup>
    {
        ApplicationRoleGroup GetRoleGroup(int groupId,string roleID);
    }

    public class ApplicationRoleGroupRepository : RepositoryBase<ApplicationRoleGroup>, IApplicationRoleGroupRepository
    {
        public ApplicationRoleGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public ApplicationRoleGroup GetRoleGroup(int groupId, string roleID)
        {
            return this.DbContext.ApplicationRoleGroups.SingleOrDefault(x => x.GroupId == groupId && x.RoleId == roleID);
        }
    }
}
