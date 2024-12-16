using sample_project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_project.Data_access
{
    public interface ISharedDataService
    {
        public int user_id { get; set; }

        public List<user> list_user { get; set; }

        public string path_histogram { get; set; }

    }

    public class shareddata_service : ISharedDataService
    {
        public int user_id { get; set; }

        public List<user> list_user { get; set; }

        public string path_histogram { get; set; }
    }
}
