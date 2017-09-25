using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface INewCategoryRepository : IRepository<NewCategory>
    {
        NewCategory GetByAlias(string alias);

        IEnumerable<int> GetListCategoryByGroupId(int groupId);
    }

    public class NewCategoryRepository : RepositoryBase<NewCategory>, INewCategoryRepository
    {
        public NewCategoryRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<int> GetListCategoryByGroupId(int groupId)
        {
            var query = from n in DbContext.NewCategories
                        join cg in DbContext.ApplicationCateGroups
                        on n.ID equals cg.CategoryId
                        where cg.GroupId == groupId
                        select n.ID;
            return query;
        }

        public NewCategory GetByAlias(string alias)
        {
            return this.DbContext.NewCategories.SingleOrDefault(x=>x.Alias == alias);
        }
    }
}
