using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikkiDBLib.Modells;

namespace WikkiDBLib
{
    public class AppDbContext : DbContext
    {
        private readonly string _dbConString = @"Server=(LocalDb)\MSSQLLocalDB;Database=WikiDB;Integrated Security=true;TrustServerCertificate=true";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbConString);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Person> Person { get; set; }

        public DbSet<Stadt> Stadt { get; set; }
    }
}
