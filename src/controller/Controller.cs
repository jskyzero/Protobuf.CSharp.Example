using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Protobuf.Csharp.Example.Model;

namespace Protobuf.Csharp.Example.Controller {

  public class Controller : IDisposable {
    private static Users _users = new Users();
    private static Books _books = new Books();
    private RepeatedField<User> users => _users.Users_;
    private RepeatedField<Book> books => _books.Books_;
    private static BorrowRecords records = new BorrowRecords();
    private static Dictionary<string, Tuple<Action<FileStream>, Action<FileStream>>> fileMapDict =
      new Dictionary<string, Tuple<Action<FileStream>, Action<FileStream>>> {
        {
          "user.dat",
          new Tuple<Action<FileStream>, Action<FileStream>>(
            new Action<FileStream>((input) => {
              _users = Users.Parser.ParseFrom(input);
            }),
            new Action<FileStream>((output) => {
              _users.WriteTo(output);
            }))
        },
        {
          "books.dat",
          new Tuple<Action<FileStream>, Action<FileStream>>(
            new Action<FileStream>((input) => {
              _books = Books.Parser.ParseFrom(input);
            }),
            new Action<FileStream>((output) => {
              _books.WriteTo(output);
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

    public bool ContainNoAdministratorAccount => _users.Users_.Where(
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

    public bool isUserNameExist(string name) {
      return users.Where(user => user.Name == name).Count() != 0;
    }

    public void AddUser(string name, string password) {
      var user = new User {
        Name = name,
        Password = password,
      };
      users.Add(user);
      LogHelper.Log(LogType.DEBUG, user.ToString());
    }

    public bool BorrowBook(string name) {
      return false;
    }

    public bool ReturnBook(string name) {
      return false;
    }

    public string FindBooks(string name) {
      var list = books.Where(book => book.Name == name).Select(book => {
        return String.Format("Name: {0}\ndetails: {1}\namount: {2}\n", 
        book.Name, book.Details, book.Amount);
      });
      return String.Join("\n", list);
    }

    public bool CheckUserPassword(string name, string password) {
      return users.Where(user => user.Name == name && user.Password == password).Count() == 1;
    }

    public bool UpdateUserPassword(string name, string password, string newPassword) {
      return users.Where(user => user.Name == name && user.Password == password).Count() == 1;
    }

    public bool CheckIsAdministratorUser(string name, string password) {
      return users.Where(user => user.Name == name && user.Password == password && user.Roletype == User.Types.RoleType.Administrator).Count() == 1;
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