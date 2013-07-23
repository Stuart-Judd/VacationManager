﻿using System.Data.Entity;
using Persistence.Configurations;
using Persistence.Model;

namespace Persistence
{
    public class VacationManagerContext : DbContext
    {
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<VacationRequestEntity> Requests { get; set; }
        public DbSet<EmployeeSituationEntity> Situations { get; set; }

        //public VacationManagerContext()
        //{
        //    // Initialize EF Profiler. Because of this you will be able to check SQL generated by EF.
        //    HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new VacationRequestConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}