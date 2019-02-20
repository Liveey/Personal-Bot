using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Okami.Resources.Database
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Gold> Golds { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            string DBLocation = Assembly.GetEntryAssembly().Location.Replace(@"bin\Debug\netcoreapp2.1", @"Data"); //this is where my database is located
            Options.UseSqlite($"Data Source={DBLocation}Database.sqlite");
            Console.WriteLine($"{DBLocation}Database.sqlite");
        }
    }
}
