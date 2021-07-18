using System;
using CRM;
using Models;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Authentication {
   [Route("")]
   public class AuthenticationController : Microsoft.AspNetCore.Mvc.Controller {

      [HttpGet]
      public ResponseBody<string> Home() {
         Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000/");
         return new ResponseBody<string> {
            body = "Hello World",
            statusCode = 200,
         };
      }

      [HttpPost]
      [Route("login")]
      public async Task<string> Login() {
         LoginRequest request = await JSONN.httpContextDeseriliser<LoginRequest>(Request);
         ResponseBody<string> response = new Authenticate.Login(request).authenticate();
         CookieOptions options = new CookieOptions() {
            SameSite = SameSiteMode.None,
            Domain = "localhost",
            Secure = true,
         };
         Response.Headers.Add("Access-Control-Allow-Credentials", "true");
         Response.StatusCode = response.statusCode;
         Response.Cookies.Append("auth", response.body, options);
         return response.body;
      }

      [HttpGet]
      [Route("/demo")]
      public string Demo() {
         Response.Headers.Add("Access-Control-Allow-Credentials", "true");
         Console.WriteLine(Request.Headers["Cookie"]);
         string password = "123";

         // generate a 128-bit salt using a secure PRNG
         byte[] salt = new byte[128 / 8];
         using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
         }
         Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

         // hash::: ZmDSZq0sUL54xT7lda3TcX+njxHnm+cB81K1FBT9LWc=
         // salt::: yZCSv8JvkE5XFJ1jmokKlw==

         // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
         string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: password,
             salt: Encoding.ASCII.GetBytes("yZCSv8JvkE5XFJ1jmokKlw=="),
             prf: KeyDerivationPrf.HMACSHA1,
             iterationCount: 10000,
             numBytesRequested: 256 / 8));
         return $"Hashed: {hashed}";
      }

      // private LoginRequest request;
      // public Authentication(LoginRequest req) {
      //    request = req;
      // }

      // private List<Employee> employeeId() {
      //    DB<Employee> db = new DB<Employee>(Table.employee);
      //    IMongoCollection<Employee> collection = db.collection;
      //    FilterDefinitionBuilder<Employee> builder = db.builders;
      //    FilterDefinition<Employee> filter = builder.Eq("ID", request.id);
      //    return collection.Find(filter).ToList();
      // }

      // private List<Employee> password() {
      //    DB<Employee> db = new DB<Employee>(Table.employee);
      //    IMongoCollection<Employee> collection = db.collection;
      //    FilterDefinitionBuilder<Employee> builder = db.builders;
      //    FilterDefinition<Employee> filter = builder.Eq("ID", request.id) & builder.Eq("PASSWORD", request.password);
      //    return collection.Find(filter).ToList();
      // }

      // public string authenticate() {
      //    int id = this.employeeId().Count;
      //    int password = this.password().Count;

      //    if (id > 0 && password > 0) {
      //       return "Ok";
      //    } else if (id == 0) {
      //       return "Employee does not exist";
      //    }

      //    return "password is incorrect";
      // }
   }
}