﻿using Entities.Configuration;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new PlaneConfiguration());
            modelBuilder.ApplyConfiguration(new PilorConfiguration());
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Pilor> Pilors { get; set; }
    }
}
