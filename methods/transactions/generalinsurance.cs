using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM {
   public class IGeneralInsurance : IMongoObject {
      public string COMPANY { get; set; }
      public string PRODUCT { get; set; }
      public long GROSS { get; set; }
      public long NET { get; set; }      
      public string POLICY_NUMBER { get; set; }
      public int POLICY_TENOUR { get; set; }
      public string POLICY_TYPE { get; set; }
      public DateTime POLICY_LOGIN_DATE { get; set; }
      public string INSURANCE_TYPE { get; set; }
      public string BANK { get; set; }
      public int PAYMENT_TERM { get; set; }
      public DateTime ENTRY_DATE { get; set; }
      public long REVENUE { get; set; }
      public long MOBILE { get; set; }
      public long AADHAAR { get; set; }
      public int MANAGER { get; set; }
   }

   public class GeneralZonalReport : IGeneralInsurance {
      public string FIRSTNAME { get; set; }
      public string LASTNAME { get; set; }
      public string EMAIL { get; set; }
      public DateTime BIRTHDATE { get; set; }
      public string LOCATION { get; set; }
      public string BRANCH { get; set; }
   }

   public static class GeneralInsurance {
      public async static Task<IGeneralInsurance[]> Report(ICustomer[] customers, Zonal report) {
         IGeneralInsurance[] transactions = await new Filter().zonalTransactions<IGeneralInsurance>(report, "general_insurance");
         List<GeneralZonalReport> reports = new List<GeneralZonalReport>();

         for (int cIndex = 0; cIndex < customers.Length; cIndex++) {
            ICustomer customer = customers[cIndex];
            long? MOBILE = customer.MOBILE;

            for (int tIndex = 0; tIndex < transactions.Length; tIndex++) {
               IGeneralInsurance transaction = transactions[tIndex];
               long tMobile = transaction.MOBILE;

               if (tMobile == MOBILE && customer.LOCATION == report.LOCATION) {
                  reports.Add(new GeneralZonalReport() {
                     AADHAAR = (long)customer.AADHAAR,
                     INSURANCE_TYPE = transaction.INSURANCE_TYPE,
                     MANAGER = transaction.MANAGER,
                     PAYMENT_TERM = transaction.PAYMENT_TERM,
                     PRODUCT = transaction.PRODUCT,                     
                     BANK = transaction.BANK,
                     BIRTHDATE = (DateTime)customer.BIRTHDATE,
                     BRANCH = customer.BRANCH,
                     COMPANY = transaction.COMPANY,
                     EMAIL = customer.EMAIL,
                     ENTRY_DATE = transaction.ENTRY_DATE,
                     FIRSTNAME = customer.FIRSTNAME,
                     GROSS = transaction.GROSS,
                     LASTNAME = customer.LASTNAME,
                     LOCATION = customer.LOCATION,
                     MOBILE = (long)customer.MOBILE,
                     NET = transaction.NET,
                     REVENUE = transaction.REVENUE,
                  });
               }
            }
         }

         return reports.ToArray();
      }
   }
}
