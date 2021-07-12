using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raimun.Data
{
    public class ContextFactory : DesignTimeDbContextFactoryBase<AppDb>
    {
        protected override AppDb CreateNewInstance(DbContextOptions<AppDb> options)
        {
            return new AppDb(options);
        }
    }
}
