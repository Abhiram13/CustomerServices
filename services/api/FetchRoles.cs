using Models;
using System;
using DatabaseManagement;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Services {
   namespace ApiManagement {
      public sealed partial class Api {
         public static string[] fetchRoles() {
            Database<Roles> db = new Database<Roles>(Table.roles);
            List<Roles> roles = db.collection.Find(new BsonDocument()).ToList();
            return roles[0].roles;
         }
      }
   }
}