using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountantNew.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private AccountantNewDbContext dbContext;

        public AccountantNewDbContext Init()
        {
            return dbContext ?? (dbContext = new AccountantNewDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
