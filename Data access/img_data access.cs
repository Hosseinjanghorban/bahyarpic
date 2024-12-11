
using sample_project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static sample_project.database_image;

namespace sample_project.Data_access
{
    public class img_data_access
    {

      
        public void readdata_img(List<image> list_Image,int user_id)
        {   
           /* var db = new database_img();
            int i = 0;
            foreach (var item in db.image_model)
            {
                if (i == user_id)
                {
                    list_Image = item.Images;
                }
                i++;
            }*/

        }
        //update database or add ?
        public void add_image(List<image> item,int user_id)
        {
           /* image_DBModel image_dbModel = new image_DBModel();
            image_dbModel.Images = item;
            image_dbModel.ID = user_id;
            using (var db = new database_img())
            {
                db.image_model.Update(image_dbModel);
                db.SaveChanges();
            }*/
        }
    }
}
