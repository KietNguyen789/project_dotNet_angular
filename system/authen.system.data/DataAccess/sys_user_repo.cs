using authen.Database.Mongodb.Collection;
using authen.Database.System;
using authen.system.data.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace authen.system.data.DataAccess
{
    public class sys_user_repo
    {
        public readonly MongoDBContext _context;
        public sys_user_repo(MongoDBContext context)
            {
                _context = context;
        }
        public User getElementById(string Email)
        {
            // Implementation to get a User by Id
            return _context.sys_user_col.AsQueryable().Where(d=> d.email == Email).FirstOrDefault();
        }

        public async Task<int> insert(sys_user_model model)
        {

            await _context.sys_user_col.InsertOneAsync(model.db);

            return 1;
        }
    }
}
