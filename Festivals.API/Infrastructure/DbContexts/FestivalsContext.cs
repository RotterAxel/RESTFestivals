using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading;
using System.Threading.Tasks;
using Festivals.API.Infrastructure.Entities;
using Festivals.API.Service;

namespace Festivals.API.Infrastructure.DbContexts
{
    public class FestivalsContext : DbContext
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserInfoService _userInfoService;
        
        public FestivalsContext(DbContextOptions<FestivalsContext> options, 
            IDateTimeService dateTimeService,
            IUserInfoService userInfoService)
           : base(options)
        {
            this._dateTimeService = dateTimeService;
            _userInfoService = userInfoService ?? throw new ArgumentNullException(nameof(userInfoService));
        }

        public FestivalsContext(DbContextOptions<FestivalsContext> options)
            : base(options)
        {
        }
        
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Festival> Festivals { get; set; }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedOn = _dateTimeService.Now;
                        entry.Entity.RowVersion = _dateTimeService.Now;
                        entry.Entity.CreatedBy = _userInfoService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _userInfoService.UserId;
                        entry.Entity.RowVersion = _dateTimeService.Now;
                        break;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.CreatedOn = _dateTimeService.Now;
                        entry.Entity.RowVersion = _dateTimeService.Now;
                        entry.Entity.CreatedBy = _userInfoService.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _userInfoService.UserId;
                        entry.Entity.RowVersion = _dateTimeService.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

    public class MedievalFestivalsContextDesignFactory : IDesignTimeDbContextFactory<FestivalsContext>
    {
        public FestivalsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FestivalsContext>()
                           .UseMySql("Server=localhost;Port=3306;Database=Festivals;Uid=root;Pwd=root;");
    
            return new FestivalsContext(optionsBuilder.Options);
        }
        
    }
}
