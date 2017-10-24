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
        New GetNewWithAppUser(int newID);
    }

    public class NewRepository : RepositoryBase<New>, INewRepository
    {
        public NewRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<New> GetHotNew()
        {
            var querry = (from n in DbContext.News
                          where n.Status == true && n.HotFlag == true && n.NewCategoryID == Common.CommonConstants.NewID1
                          select n).Include("NewCategory");
            return querry;
        }

        public IEnumerable<New> GetLastestNew()
        {
            var querry = (from n in DbContext.News
                              //join nc in DbContext.NewCategories
                              //on n.NewCategoryID equals nc.ID
                          where n.Status == true && n.HomeFlag == true && n.NewCategoryID == Common.CommonConstants.NewID1
                          select n).Include("NewCategory");
            return querry;
        }

        public New GetNewWithAppUser(int newID)
        {
            var querry = (from n in DbContext.News
                          join u in DbContext.Users
                          on n.ApplicationUserId equals u.Id
                          where n.ID == newID
                          select n).Include("ApplicationUser").SingleOrDefault();
            return querry;
        }
    }
}
