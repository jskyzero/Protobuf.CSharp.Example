using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Protobuf.Csharp.Example.Controller;

namespace Protobuf.Csharp.Example.Viewer {
  internal class Viewer : IDisposable {
    private static string kSaltString = "jskyzero";
    private string username = "";
    private string password = "";
    private bool isPasswordRight = false;
    private bool isAdministratorAccount = false;
    private bool isLogout => isPasswordRight;

    private Controller.Controller controller = new Controller.Controller();

    public void Dispose() {
      controller.Dispose();
    }

    public void Main() {
      // controller.SaveDataFile();
      if (controller.ContainNoAdministratorAccount)
        InitialAdministratorAccount();
      // ask Login
      while (!isPasswordRight)
        UserLogin();
      // main function
      while (isLogout) {
        PrintMenu();
        PrintInputCharacter();
        MainLoop();
      }
    }

    private void ExitSystem() {
      Console.Out.WriteLine("Exit System");
      password = string.Empty;
      isPasswordRight = false;
    }

    private void CheckBooks() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      string findResult = controller.FindBooks(bookname);
      Console.WriteLine(
        findResult == string.Empty ?
        "No such name book" :
        findResult);
    }

    private void BorrowBook() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      if (controller.BorrowBook(bookname, username)) {
        Console.Out.WriteLine("Borrow success");
      } else {
        Console.Out.WriteLine("Borrow failed, please check book amount");
      };
    }

    private void ReturnBook() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      if (controller.BorrowBook(bookname, username)) {
        Console.Out.WriteLine("Return success");
      } else {
        Console.Out.WriteLine("Return failed, please check your record");
      };
    }

    private void ListBorrowRecord() {
      // Console.Out.WriteLine("Enter book name");
      // PrintInputCharacter();
      // string bookname = Console.In.ReadLine();
      string findResult = controller.FindRecords(username);
      Console.WriteLine(
        findResult == string.Empty ?
        "No borrow record" :
        findResult);
    }

    private void ChangePassword() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      string findResult = controller.FindBooks(bookname);
      Console.WriteLine(findResult == string.Empty ? "No Such Name Book" : findResult);
    }

    private void AddUser() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      string findResult = controller.FindBooks(bookname);
      Console.WriteLine(findResult == string.Empty ? "No Such Name Book" : findResult);
    }

    private void AddBooks() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      string findResult = controller.FindBooks(bookname);
      Console.WriteLine(findResult == string.Empty ? "No Such Name Book" : findResult);
    }

    private void ShowAllRecord() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      string findResult = controller.FindBooks(bookname);
      Console.WriteLine(findResult == string.Empty ? "No Such Name Book" : findResult);
    }

    private void DeleteUser() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      string findResult = controller.FindBooks(bookname);
      Console.WriteLine(findResult == string.Empty ? "No Such Name Book" : findResult);
    }

    private void MainLoop() {
      PrintInputCharacter();
      string userChoice = Console.In.ReadLine();
      switch (userChoice) {
        case "0":
          ExitSystem();
          break;
        case "1": // 1: Check Books
          CheckBooks();
          break;
        case "2": // 2: Borrow Books
          BorrowBook();
          break;
        case "3": // 3: Return Books
          break;
        case "4": // 4: List Borrow Record
          break;
        case "5": // 5: Change Password");
          break;
        case "6": // 6: (Administrator only) Add User
          break;
        case "7": // 7: (Administrator only) Add Books
          break;
        case "8": // 8: (Administrator only) Show All Record
          break;
        case "9": // 9: (Administrator only) Delete User
          break;
        default:
          Console.Out.WriteLine("Your input is {0}", userChoice);
          Console.Out.WriteLine("Can't find match methods, please check your input");
          break;
      }
    }

    private void PrintInputCharacter() {
      Console.Out.Write("$> ");
    }

    private void PrintMenu() {
      Console.Out.WriteLine(@"
      1: Check Books
      2: Borrow Books
      3: Return Books
      4: List Borrow Record
      5: Change Password");
      if (isAdministratorAccount)
        Console.Out.WriteLine(@"
      6: (Administrator only) Add User
      7: (Administrator only) Add Books
      8: (Administrator only) Show All Record
      9: (Administrator only) Delete User");
      Console.Out.WriteLine(@"
      0: Exit System
      Enter your choices(use number)");
    }

    private void InitialAdministratorAccount() {
      Console.Out.WriteLine("First time running, please set password for default administrator account");
      PrintInputCharacter();
      password = PasswordHash(Console.In.ReadLine(), kSaltString);
      controller.AddAdministratorUser(password);
    }

    private void UserLogin() {
      Console.Out.WriteLine("[User Login]");
      Console.Out.WriteLine("Input your user name");
      PrintInputCharacter();
      username = Console.In.ReadLine();
      Console.Out.WriteLine("Input your password");
      PrintInputCharacter();
      password = PasswordHash(Console.In.ReadLine(), kSaltString);
      isPasswordRight = controller.CheckUserPassword(username, password);
      isAdministratorAccount = controller.CheckIsAdministratorUser(
        username, password);
    }

    private static string PasswordHash(string password, string salt) {
      byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
      byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
      byte[] saltedValue = passwordBytes.Concat(saltBytes).ToArray();
      byte[] hashedValue = new SHA256Managed().ComputeHash(saltedValue);
      return Encoding.ASCII.GetString(hashedValue);
    }
  }
}