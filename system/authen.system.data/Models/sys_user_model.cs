using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using authen.Database.System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace authen.system.data.Models
{
    public class sys_user_model
    {
        public sys_user_model()
        {
            db = new User();
          
        }
        public User db { get; set; }

        [BsonId]
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string? fullName { get; set; }

        public string? email { get; set; }


    }
}
