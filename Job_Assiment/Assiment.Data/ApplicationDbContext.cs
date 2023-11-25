
using Assiment.core.Models;
using ECommerce.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assiment.EF
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext() : base()
        { }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyGlobalFilter(x => !x.IsDeleted);


        }
    }
}
