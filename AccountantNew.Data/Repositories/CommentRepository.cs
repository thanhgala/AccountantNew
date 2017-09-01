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
    public interface ICommentRepository : IRepository<Comment>
    {
        //IEnumerable<Comment> GetListCommentByPostID(int id);
    }

    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        //public IEnumerable<Comment> GetListCommentByPostID(int id)
        //{
        //    var query = from c in DbContext.Comments
        //                join p in DbContext.Posts
        //                on c.PostID equals p.ID
        //                where c.PostID == id
        //                select c;

        //    return query;
        //}
    }
}
