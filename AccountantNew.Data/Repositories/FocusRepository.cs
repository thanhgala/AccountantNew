using AccountantNew.Data.Infrastructure;
using AccountantNew.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Repositories
{
    public interface IFocusRepository : IRepository<FocusNotification>
    {

    }
    public class FocusRepository : RepositoryBase<FocusNotification>, IFocusRepository
    {
        public FocusRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
