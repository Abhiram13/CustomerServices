using System;
using Models;
using CRM;
using Services.Security;
using DataBase;

namespace Services {
	namespace Authentication {
		public class Login : DatabaseOperations<Employee> {
			private LoginRequest _login { get; set; }
			private Employee employee { get; set; }
			public Login(DocumentStructure<Employee> document, LoginRequest login) : base(document) {
				_login = login;
				employee = FetchOne()[0];
			}

			private bool IsPasswordValid() {
				return Hash.Compare(employee.salt, _login.password, employee.password);
			}

			public ResponseModel Authenticate() {
				bool passwordCheck = this.IsPasswordValid();
				if (employee == null) {
					return new ResponseModel(StatusCode.NotFound);
				} else if (passwordCheck == false) {
					return new ResponseModel(StatusCode.Forbidden);
				}

				return new ResponseModel(StatusCode.OK, Text.Encode($"{_login.empid.ToString()}_{_login.password.ToString()}"));
			}
		}
	}
}