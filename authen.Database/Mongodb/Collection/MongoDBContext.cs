using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using authen.Database.System;

namespace authen.Database.Mongodb.Collection
{
    public partial class MongoDBContext
    {
        public IMongoDatabase _database { get; }

        public MongoDBContext(IMongoDatabase database)
        {
            // take database inject from program.cs
            // when the app start, this table will create when the insertOneAsync is performed
            _database = database;
            sys_user_col = database.GetCollection<User>("Users");
        }

        public readonly IMongoCollection<User> sys_user_col;
    }
}
