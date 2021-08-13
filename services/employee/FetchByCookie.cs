using System;
using CRM;
using Microsoft.AspNetCore.Http;
using Models;
using Services.EmployeeManagement;

namespace Services {
   namespace EmployeeManagement {
      public partial class EmployeeService {
         public static Employee fetchByCookie(HttpRequest request) {
            string cookie = request.Headers["Cookie"];
            string str = cookie.Split("=")[1];
            string replaced = str.Replace("%3D", "=");
            int id = Convert.ToInt32(Text.Decode(replaced).Split("_")[0]);

            return EmployeeService.FetchById(id);
         }
      }
   }
}