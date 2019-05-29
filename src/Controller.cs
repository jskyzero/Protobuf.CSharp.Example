using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Protobuf.Csharp.Example.Model;

namespace Protobuf.Csharp.Example.Controller {

  public class Controller {
    private static Users users = new Users();
    private static Books books = new Books();
    private static BorrowRecords records = new BorrowRecords();
    private static Dictionary<string, Action<FileStream>> readFileMapDict =
      new Dictionary<string, Action<FileStream>> {
        {
          "user.dat",
          new Action<FileStream>((input) => {
            users = Users.Parser.ParseFrom(input);
          })
        },
        {
          "books.dat",
          new Action<FileStream>((input) => {
            books = Books.Parser.ParseFrom(input);
          })
        },
        {
          "records.dat",
          new Action<FileStream>((input) => {
            records = BorrowRecords.Parser.ParseFrom(input);
          })
        },
      };

    private static Dictionary<string, Action<FileStream>> writeFileMapDict =
      new Dictionary<string, Action<FileStream>> {
        {
          "user.dat",
          new Action<FileStream>((output) => {
            users.WriteTo(output);
          })
        },
        {
          "books.dat",
          new Action<FileStream>((output) => {
            books.WriteTo(output);
          })
        },
        {
          "records.dat",
          new Action<FileStream>((output) => {
            records.WriteTo(output);
          })
        },
      };

    private static string ConfigFolderName = "./.config/";

    public Controller() {
      LoadDataFile();
    }

    // Parsing 
    public void LoadDataFile() {
      foreach (var fileName in readFileMapDict.Keys) {
        try {
          using(var input = File.OpenRead(ConfigFolderName + fileName)) {
            // var valuePair = fileMapDict[fileName];
            // var objectType = valuePair.Item2.GetMembers()
            //   .Where(m => m.Name == "Parser").First();
            // var invoked = objectType.Invoke(valuePair.Item1, null);
            readFileMapDict[fileName].Invoke(input);
          }
        } catch (Exception e) when(
          e is FileNotFoundException || e is DirectoryNotFoundException) {
          // file not exist
          LogHelper.Log(LogHelper.LogType.WARNING,
            String.Format("file:{0} not found, use default value", fileName));
        }
      }
    }

    // Serialization 
    public void SaveDataFile() {
      if (!Directory.Exists(ConfigFolderName))
        Directory.CreateDirectory(ConfigFolderName);
      foreach (var fileName in writeFileMapDict.Keys) {
        using(var input = File.OpenWrite(ConfigFolderName + fileName)) {
          // var valuePair = fileMapDict[fileName];
          // var objectType = valuePair.Item2.GetMembers()
          //   .Where(m => m.Name == "Parser").First();
          // var invoked = objectType.Invoke(valuePair.Item1, null);
          writeFileMapDict[fileName].Invoke(input);
        }
      }
    }
  }

}