using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raimun.Core.Common.Interfaces;
using Raimun.Domain;
using Raimun.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Raimun.Domain.Entities.Common;

namespace Raimun.Data
{
    public class AppDb : IdentityDbContext<User, IdentityRole, string>, IAppDb
    {
        public AppDb(DbContextOptions<AppDb> options)
            : base(options)
        {

            Db = this.Database;
        }



        public DatabaseFacade Db { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDb).Assembly);
            base.OnModelCreating(builder);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity> Add<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IBaseEntity
        {
            var entry = await base.Set<TEntity>().AddAsync(entity, cancellationToken);
            return entry.Entity;
        }

        public Task<TEntity> Update<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IBaseEntity
        {
            return Task.FromResult(base.Set<TEntity>().Update(entity).Entity);
        }

        public Task Delete<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class, IBaseEntity
        {
            return Task.FromResult(base.Set<TEntity>().Remove(entity));
        }

        public DbSet<T> DbSet<T>() where T : class, IBaseEntity
        {
            return base.Set<T>();
        }

    }
}
