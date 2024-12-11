using Microsoft.EntityFrameworkCore;
using sample_project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_project
{
    
        public class database_userr : DbContext
        {
            public database_userr()
            {

                Database.EnsureCreated();

            }
            public DbSet<user> user_Model { get; set; }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlite("Data Source= database__user.db");

            }

        }
    }

