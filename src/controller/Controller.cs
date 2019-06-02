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
    private static BorrowRecords _records = new BorrowRecords();
    private RepeatedField<User> users => _users.Users_;
    private RepeatedField<Book> books => _books.Books_;
    private RepeatedField<BorrowRecord> records => _records.BorrowRecords_;

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
              _records = BorrowRecords.Parser.ParseFrom(input);
            }),
            new Action<FileStream>((output) => {
              _records.WriteTo(output);
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

    public bool CheckBookNameExists(string bookname) {
      return books.Where(book => book.Name == bookname).Count() > 0;
    }

    public void AddBook(string bookname, string details, uint amount) {
      var newBook = new Book {
        Name = bookname,
        Details = details,
        Amount = amount,
      };
      books.Add(newBook);
    }

    public string FindBooks(string bookname) {
      var list = books.Where(
        book => book.Name == bookname).Select(
        book => {
          return String.Format(
            "Name: {0}\ndetails: {1}\namount: {2}\n",
            book.Name, book.Details, book.Amount);
        });
      return String.Join("\n", list);
    }

    public bool BorrowBook(string bookname, string username) {
      if (books.Where(book =>
          book.Name == bookname &&
          book.Amount > 0).Count() != 1)
        return false;
      foreach (var book in books) {
        if (book.Name == bookname) book.Amount -= 1;
      }
      var record = new BorrowRecord() {
        UserName = username,
        BookName = bookname,
        BorrowTime = DateTime.Now.ToString(),
      };
      records.Add(record);
      return true;
    }

    public bool ReturnBook(string bookname, string username) {
      if (books.Where(book => book.Name == bookname).Count() != 1)
        return false;
      if (records.Where(record =>
          record.BookName == bookname &&
          record.UserName == username &&
          record.ReturnTime == string.Empty).Count() == 0) return false;
      foreach (var book in books) {
        if (book.Name == bookname) book.Amount += 1;
      }
      foreach (var record in records) {
        if (record.BookName == bookname &&
          record.UserName == username &&
          record.ReturnTime == string.Empty) {
            record.ReturnTime = DateTime.Now.ToString();
            break;
          }
      }
      return true;
    }

    public string FindRecords(string username) {
      return String.Join("\n",
        records.Where(record =>
          record.UserName == username).Select(
          record =>
          String.Format(
            "BookName: {0}\nBorrowDate: {1}\nReturnDate: {2}\n",
            record.BookName, record.BorrowTime,
            record.ReturnTime == String.Empty ? "Unturened" : record.ReturnTime)
        ));
    }

    public string FindRecords() {
      return String.Join("\n",
        records.Select(
          record =>
          String.Format(
            "UserName: {3}\nBookName: {0}\nBorrowDate: {1}\nReturnDate: {2}\n",
            record.BookName, record.BorrowTime,
            record.ReturnTime == String.Empty ? "Unturened" : record.ReturnTime,
            record.UserName)
        ));
    }

    public bool CheckUserNameExist(string name) {
      return users.Where(user => user.Name == name).Count() != 0;
    }

    public void AddUser(string name, string password, User.Types.RoleType type = User.Types.RoleType.Defalult) {
      var user = new User {
      Name = name,
      Password = password,
      Roletype = type
      };
      users.Add(user);
      LogHelper.Log(LogType.DEBUG, user.ToString());
    }

    public void DeleteUser(string name) {
      users.Remove(users.Where(user => user.Name == name).First());
    }

    public bool CheckUserPassword(string name, string password) {
      return users.Where(user => user.Name == name &&
        user.Password == password).Count() == 1;
    }

    public bool UpdateUserPassword(string name, string password, string newPassword) {
      if (users.Where(user => user.Name == name &&
          user.Password == password).Count() != 1)
        return false;
      foreach (var user in users) {
        if (user.Name == name &&
          user.Password == password) {
          user.Password = newPassword;
        }
      }
      return true;
    }

    public bool CheckIsAdministratorUser(string name, string password) {
      return users.Where(user => user.Name == name &&
        user.Password == password &&
        user.Roletype == User.Types.RoleType.Administrator).Count() == 1;
    }

    public void AddAdministratorUser(string password) {
      AddUser(kAdministratorAccountName, password, User.Types.RoleType.Administrator);
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