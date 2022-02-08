using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace Data
{
    public class MyContext :DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
    
}
