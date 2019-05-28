using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using Protobuf.Csharp.Example.Model; 


namespace Protobuf.Csharp.Example.Controller {

  class Controller {
    private static Users users;
    private static Books books;
    private static BorrowRecord records;
    static Dictionary<string, Tuple<Object, Type>> fileMapDict = 
      new Dictionary<string, Tuple<object, Type>> {
       { "user.dat", new Tuple<object, Type>(users, typeof(Users))},
    };


    // Parsing 
    public void LoadDataFile() {
      foreach(var fileName in fileMapDict.Keys) {
        using (var input = File.OpenRead(fileName)) {
          var valuePair = fileMapDict[fileName];
          var objectType = valuePair.Item2;
          // var valueWithType = (objectType)valuePair.Item1;
        }
      }
    }

    // Serialization 
    public void SaveDataFile() {
      
    }
  }

}