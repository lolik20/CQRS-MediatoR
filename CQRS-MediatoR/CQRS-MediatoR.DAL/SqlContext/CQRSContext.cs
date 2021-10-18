using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CQRS_MediatoR.Common.Entities.Domain;
using CQRS_MediatoR.Common.Entities.Domain.Enums;

namespace CQRS_MediatoR.DAL.SqlContext
{
    public class CQRSContext : DbContext
    {
        public CQRSContext(DbContextOptions<CQRSContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");


        }

    }
}
