using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AccountantNew.Data.Repositories
{
    public interface IApplicationRoleRepository : IRepository<ApplicationRole>
    {
        IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId);

        Task<ApplicationRole> ExamPleAsync();

        IEnumerable<ApplicationRole> GetListRoleByGroupRoleId(int groupId,string roleId);
    }

    public class ApplicationRoleRepository : RepositoryBase<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<ApplicationRole> ExamPleAsync()
        { 
            var querry = await DbContext.ApplicationRoles
                        .Where(x => x.Id == "dada")
                        .FirstOrDefaultAsync();
            return querry;
        }

        public IEnumerable<ApplicationRole> GetListRoleByGroupId(int groupId)
        {
            var query = from g in DbContext.ApplicationRoles
                        join ug in DbContext.ApplicationRoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;
            return query;
        }

        public IEnumerable<ApplicationRole> GetListRoleByGroupRoleId(int groupId,string roleId)
        {
            var query = from g in DbContext.ApplicationRoles
                        join ug in DbContext.ApplicationRoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId && ug.RoleId == roleId
                        select g;
            return query;
        }
    }
}
