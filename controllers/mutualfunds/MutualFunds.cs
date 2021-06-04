using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Models;
using CRM;
using Models.TransactionsRequestBody;
using CustomerManagement;
using EmployeeManagement;

namespace TransactionManagement {
   public class MutualFundsTransactionController : TransactionController {
      private Task<MutualFundsBody> transaction;

      public MutualFundsTransactionController(HttpContext context) {
         this.transaction = JSONObject.Deserilise<MutualFundsBody>(context);
      }

      public async Task<string> Add() {
         MutualFundsBody trans = await this.transaction;
         TransactionVerification<MutualFundsBody> details = new TransactionVerification<MutualFundsBody>() {
            document = trans,
            isCustomerExist = await CustomerController.IsCustomerExist(trans.MOBILE),
            isEmployeeExist = await EmployeeController.IsEmployeeExist(trans.MANAGER),
            table = Table.mutualFunds,
         };

         return AddTransaction<MutualFundsBody>(details);
      }
   }
}