using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CRM {
   public abstract class Controller {
      private HttpContext context;

      public Controller(HttpContext Context) {
         this.context = Context;
      }

      /// <summary>
      /// Fetches all documents from the specified table
      /// </summary>
      /// <typeparam name="Table">Table Type</typeparam>
      /// <param name="tableName">Name of the Table from where documents need to be fetched</param>
      /// <returns>Array of Documents from the specified table</returns>
      public async static Task<Table[]> FetchAll<Table>(string tableName) {
         string documents = await new Database<Table>(tableName).FetchAll();
         return JSONObject.DeserializeObject<Table[]>(documents);
      }

      public string Add<Table>(bool isExist, string table, Table doc) {
         if (!isExist) {
            new Database<Table>(table).Insert(doc);
            return "Successfully Added";
         }

         return "Already Existed";
      }
   }
}