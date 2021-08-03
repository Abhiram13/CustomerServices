using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models;
using CRM;
using System;
using MongoDB.Driver;
using AuthenticationService;
using DatabaseManagement;

namespace CRM {
   public class Services<T> {
      public T requestBody;
      public Document<T> document;

      public Services(HttpRequest request) {
         requestBody = JSON.httpContextDeseriliser<T>(request).Result;         
      }

      public Services(HttpRequest request, string table) {
         requestBody = JSON.httpContextDeseriliser<T>(request).Result;
         document = new Document<T>(table);
      }

      public void Send<M>(int statusCode, M message) {
         
      }
   }
}