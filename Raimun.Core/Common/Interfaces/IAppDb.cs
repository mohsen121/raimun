using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Raimun.Domain.Entities;
using Raimun.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raimun.Core.Common.Interfaces
{
    public interface IAppDb
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<TEntity> Add<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IBaseEntity;
        Task<TEntity> Update<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IBaseEntity;
        Task Delete<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IBaseEntity;
        DbSet<T> DbSet<T>() where T : class, IBaseEntity;
        public DatabaseFacade Db { get; set; }
    }
}
