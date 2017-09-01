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
    public interface IFileRepository : IRepository<File>
    {
    }

    public class FileRepository : RepositoryBase<File>, IFileRepository
    {
        public FileRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
