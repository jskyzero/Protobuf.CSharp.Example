using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Protobuf.Csharp.Example.Model;

namespace Protobuf.Csharp.Example.Controller {

  public class Controller : IDisposable {
    private static Users users = new Users();
    private static Books books = new Books();
    private static BorrowRecords records = new BorrowRecords();
    private static Dictionary<string, Tuple<Action<FileStream>, Action<FileStream>>> fileMapDict =
      new Dictionary<string, Tuple<Action<FileStream>, Action<FileStream>>> {
        {
          "user.dat",
          new Tuple<Action<FileStream>, Action<FileStream>>(
            new Action<FileStream>((input) => {
              users = Users.Parser.ParseFrom(input);
            }),
            new Action<FileStream>((output) => {
              users.WriteTo(output);
            }))
        },
        {
          "books.dat",
          new Tuple<Action<FileStream>, Action<FileStream>>(
            new Action<FileStream>((input) => {
              books = Books.Parser.ParseFrom(input);
            }),
            new Action<FileStream>((output) => {
              books.WriteTo(output);
            }))
        },
        {
          "records.dat",
          new Tuple<Action<FileStream>, Action<FileStream>>(
            new Action<FileStream>((input) => {
              records = BorrowRecords.Parser.ParseFrom(input);
            }),
            new Action<FileStream>((output) => {
              records.WriteTo(output);
            }))
        },
      };

    private static string kConfigFolderName = "./.config/";
    private static string kAdministratorAccountName = "Administrator";

    public bool ContainNoAdministratorAccount => users.Users_.Where(
      user => user.Name == kAdministratorAccountName).Count() == 0;

    public Controller() {
      LoadDataFile();
    }
    // Finalizers release resources
    ~Controller() {
      LogHelper.Log(LogType.DEBUG, "~Controller()");
    }
    // Explicit release of resources
    public void Dispose() {
      LogHelper.Log(LogType.DEBUG, "Controller Dispose");
      LogHelper.Log(LogType.DEBUG, "SaveDataFile Begin");
      SaveDataFile();
      LogHelper.Log(LogType.DEBUG, "SaveDataFile2 End");
    }

    public void AddUser(string name, string password) {
      var user = new User {
        Name = name,
        Password = password,
      };
      users.Users_.Add(user);
      LogHelper.Log(LogType.DEBUG, user.ToString());
    }

    public bool CheckUserPassword(string name, string password) {
      return users.Users_.Where(user => user.Name ==name && user.Password == password).Count() == 1;
    }

    public void AddAdministratorUser(string password) {
      AddUser(kAdministratorAccountName, password);
    }

    // Parsing 
    public void LoadDataFile() {
      foreach (var fileName in fileMapDict.Keys) {
        try {
          using(var input = File.OpenRead(kConfigFolderName + fileName)) {
            fileMapDict[fileName].Item1.Invoke(input);
          }
        } catch (Exception e) when(
          e is FileNotFoundException || e is DirectoryNotFoundException) {
          // file not exist
          LogHelper.Log(LogType.WARNING,
            String.Format("file:{0} not found, use default value", fileName));
        }
      }
    }

    // Serialization 
    public void SaveDataFile() {
      if (!Directory.Exists(kConfigFolderName))
        Directory.CreateDirectory(kConfigFolderName);
      foreach (var fileName in fileMapDict.Keys) {
        using(var input = File.OpenWrite(kConfigFolderName + fileName)) {
          // var valuePair = fileMapDict[fileName];
          // var objectType = valuePair.Item2.GetMembers()
          //   .Where(m => m.Name == "Parser").First();
          // var invoked = objectType.Invoke(valuePair.Item1, null);
          fileMapDict[fileName].Item2.Invoke(input);
        }
      }
    }
  }
}