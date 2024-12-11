
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using sample_project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_project.Data_access
{
    public class user_dataaccess
    {
        public void readdata_user(List<user> users)
        {
            var db = new database_userr();
            foreach (var item in db.user_Model)
            {
                user tmp_user = new user();
                tmp_user.Id = item.Id;
                tmp_user.email = item.email;
                tmp_user.name = item.name;
                tmp_user.password = item.password;
                tmp_user.username = item.username;                
                users.Add(tmp_user);

            }
        }

        public void add_user(user item)
        {
            using (var db = new database_userr())
            {
                db.user_Model.Add(item);
                db.SaveChanges();
            }
        }

    }
}

