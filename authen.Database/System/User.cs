using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace authen.Database.System
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // Để MongoDB tự hiểu Id là kiểu chuỗi ObjectId
        public string id { get; set; }
        public string Username { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] public DateTime? date_of_birth { get; set; }
        public int gender { get; set; }
        public string? fullName { get; set; }

        public string? email { get; set; }
        public string? phone_number { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
