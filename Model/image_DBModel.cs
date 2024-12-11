using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_project.Model
{
    public class image_DBModel
    {
        [Key]
        public int ID { get; set; }
        public List<image> Images { get; set; }
    }
}
