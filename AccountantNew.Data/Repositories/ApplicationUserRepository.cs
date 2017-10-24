using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId);
        IEnumerable<ApplicationUser> GetListUserByName(string nameGroup);
    }

    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<ApplicationUser> GetListUserByGroupId(int groupId)
        {
            var query = from u in DbContext.Users
                        join ug in DbContext.ApplicationUserGroups
                        on u.Id equals ug.UserId
                        join g in DbContext.ApplicationGroups
                        on ug.GroupId equals g.ID
                        where ug.GroupId == groupId
                        select u;
            return query;
        }

        public IEnumerable<ApplicationUser> GetListUserByName(string nameGroup)
        {
            var query = from u in DbContext.Users
                        join ug in DbContext.ApplicationUserGroups
                        on u.Id equals ug.UserId
                        join g in DbContext.ApplicationGroups
                        on ug.GroupId equals g.ID
                        where g.Name.Contains(nameGroup)
                        select u;
            return query;
        }
    }
}
