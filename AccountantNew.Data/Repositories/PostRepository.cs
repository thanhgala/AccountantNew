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
    public interface IPostRepository : IRepository<Post>
    {
        Post GetByAlias(string alias);
        Post GetPostWithAppUser(int id);
        IEnumerable<Post> GetAllWithAppUserNewCate();
    }

    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public Post GetByAlias(string alias)
        {
            return this.DbContext.Posts.SingleOrDefault(x => x.Alias == alias);
        }

        public Post GetPostWithAppUser(int id)
        {
            var query = (from p in DbContext.Posts
                         join u in DbContext.Users
                         on p.ApplicationUserId equals u.Id
                         where p.ID == id
                         select p).Include("ApplicationUser").SingleOrDefault();
            return query;
        }

        public IEnumerable<Post> GetAllWithAppUserNewCate()
        {
            var query = (from p in DbContext.Posts
                         join u in DbContext.Users
                         on p.ApplicationUserId equals u.Id
                         join n in DbContext.NewCategories
                         on p.NewCategoryID equals n.ID
                         select p).Include("ApplicationUser").Include("NewCategory");
            return query;
        }
    }
}
