using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_project.Model
{
    public class user
    {
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        [Key]
        public int Id { get; set; }
        
       // private ObservableCollection<image> _list_images;
       // public ObservableCollection<image> list_images
       // {
       //     get { return _list_images; }
       //     set { _list_images = value; }
       // }

    }
}
