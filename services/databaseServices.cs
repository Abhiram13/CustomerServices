using System;
using MongoDB.Driver;
using CRM;
using Models;
using System.Collections.Generic;
using MongoDB.Driver.Linq;

namespace DataBase {
   #nullable enable
   public abstract class DatabaseOperations<DataType> {
		private DocumentStructure<DataType>? _document { get; set; }
		public IMongoCollection<DataType>? _collection { get; private set; }
		public DatabaseOperations(DocumentStructure<DataType> document) {
			_document = document;
         _collection = Mongo.database.GetCollection<DataType>(document.Collection);
		}

      public List<DataType> FetchOne() {
         return _collection.Find(_document?.filter).ToList();
      }

		private bool _isDocumentExist() {
			return FetchOne().Count > 0;
		}

      public ResponseModel Insert() {
         if (_document == null || _document.RequestBody == null) {
				return new ResponseModel(StatusCode.BadRequest);
			} else if (!_isDocumentExist()) {
				_collection?.InsertOne(_document.RequestBody);
				return new ResponseModel(StatusCode.Inserted);
			}

			return new ResponseModel(StatusCode.DocumentFound);
		}

      public ResponseModel UpdateOne() { 
         if (_collection != null && _document != null) {
				UpdateResult result = _collection.UpdateOne(_document.filter, _document.update); 
            if (result.MatchedCount > 0) {
					return new ResponseModel(StatusCode.OK);
				}

				return new ResponseModel(StatusCode.NotModified);
			}

			return new ResponseModel(StatusCode.BadRequest);
		}

      public ResponseModel DeleteOne() {
			if (_collection != null && _document != null) {
				DeleteResult delete = _collection.DeleteOne(_document.filter);            
				if (delete.DeletedCount > 0) {
					return new ResponseModel(StatusCode.OK, "Document successfully deleted");
				}

				return new ResponseModel(StatusCode.NotModified);
			}

			return new ResponseModel(StatusCode.BadRequest);
      }
   }
}