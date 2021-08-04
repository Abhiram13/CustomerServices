using System;
using Models;
using CRM;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Services.Security;

// Take login credentials from user input
// search db with that id
// if found, compare the password
// if not, throw error

namespace Services {
   namespace Authentication {
      public class Login : Services<Employee> {
         private Employee emp;
         public Login(HttpRequest request) : base(request, Table.employee) { }

         private Employee employee() {
            FilterDefinition<Employee> filter = document.builders.Eq("empid", requestBody.empid);
            return document.FetchOne(filter);
         }

         private bool IsPasswordValid() {
            return Hash.Compare(emp.salt, requestBody.password, emp.password);            
         }

         public ResponseBody<string> Authenticate() {
            ResponseBody<string> response = new ResponseBody<string>();
            emp = this.employee();

            if (emp == null) {
               response.body = "Employee does not exist";
               response.statusCode = 404;
               return response;
            }

            bool passwordCheck = this.IsPasswordValid();

            if (passwordCheck == false) {
               response.body = "password is incorrect";
               response.statusCode = 401;
               return response;
            }

            response.body = Text.Encode($"{requestBody.empid.ToString()}_{requestBody.password.ToString()}");
            response.statusCode = 200;

            return response;
         }
      }
   }
}