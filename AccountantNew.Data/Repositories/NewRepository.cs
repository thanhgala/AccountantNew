using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface INewRepository : IRepository<New>
    {
        IEnumerable<New> GetHotNew();
        IEnumerable<New> GetLastestNew();
    }

    public class NewRepository : RepositoryBase<New>, INewRepository
    {
        public NewRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<New> GetHotNew()
        {
            var querry = (from n in DbContext.News
                         join nc in DbContext.NewCategories
                         on n.NewCategoryID equals nc.ID
                         where n.Status == true && n.HotFlag == true
                         select n).Include("NewCategory");
            return querry;
        }

        public IEnumerable<New> GetLastestNew()
        {
            var querry = (from n in DbContext.News
                          join nc in DbContext.NewCategories
                          on n.NewCategoryID equals nc.ID
                          where n.Status == true && n.HomeFlag == true
                          select n).Include("NewCategory");
            return querry;
        }
    }
}
