using Microsoft.EntityFrameworkCore;
using sample_project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_project
{
    public class database_image
    {
        public class database_img : DbContext
        {
            public database_img()
            {

                Database.EnsureCreated();

            }
            public DbSet<image_DBModel> image_model { get; set; }


            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlite("Data Source= database_image.db");

            }
        }
    }
}
