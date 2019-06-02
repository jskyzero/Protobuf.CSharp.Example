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
      if (controller.ReturnBook(bookname, username)) {
        Console.Out.WriteLine("Return success");
      } else {
        Console.Out.WriteLine("Return failed, please check your record");
      };
    }

    private void ListBorrowRecord() {
      string findResult = controller.FindRecords(username);
      Console.WriteLine(
        findResult == string.Empty ?
        "No borrow record" :
        findResult);
    }

    private void ChangePassword() {
      Console.Out.WriteLine("Enter old password");
      PrintInputCharacter();
      string oldpassword = PasswordHash(Console.In.ReadLine(), kSaltString);
      Console.Out.WriteLine("Enter new password");
      PrintInputCharacter();
      string newpassword = PasswordHash(Console.In.ReadLine(), kSaltString);
      if (controller.UpdateUserPassword(username, oldpassword, newpassword)) {
        Console.WriteLine("Update user password success");
        Console.WriteLine("Please Login agagin");
        isPasswordRight = false;
      } else {
        Console.WriteLine("Update user password failed, please check your oldpassword");
      }
    }

    private void AddUser() {
      Console.Out.WriteLine("Enter user name");
      PrintInputCharacter();
      string newUsername = Console.In.ReadLine();
      Console.Out.WriteLine("Enter user default password");
      PrintInputCharacter();
      string newPassword = PasswordHash(Console.In.ReadLine(), kSaltString);
      if (controller.CheckUserNameExist(newUsername)) {
        Console.Out.WriteLine("Username exists, please change a new username");
      } else {
        controller.AddUser(newUsername, newPassword);
        Console.Out.WriteLine("Add user success");
      }
    }

    private void AddBooks() {
      Console.Out.WriteLine("Enter book name");
      PrintInputCharacter();
      string bookname = Console.In.ReadLine();
      Console.Out.WriteLine("Enter book details");
      PrintInputCharacter();
      string detail = Console.In.ReadLine();
      Console.Out.WriteLine("Enter book amount");
      PrintInputCharacter();
      uint amount = UInt32.Parse(Console.In.ReadLine());

      if (controller.CheckBookNameExists(bookname)) {
        Console.Out.WriteLine("Bookname exists, please change a new bookname");
      } else {
        controller.AddBook(bookname, detail, amount);
        Console.Out.WriteLine("Add book success");
      }
    }

    private void ShowAllRecord() {
     Console.Out.WriteLine(controller.FindRecords());
    }

    private void DeleteUser() {
      Console.Out.WriteLine("Enter user name");
      PrintInputCharacter();
      string username = Console.In.ReadLine();
      if (controller.CheckUserNameExist(username)) {
        controller.DeleteUser(username);
        Console.Out.WriteLine("Delete user success");
      } else {
        Console.Out.WriteLine("Username unexist, please input correct username");
      }
    }

    private void MainLoop() {
      PrintInputCharacter();
      string userChoice = Console.In.ReadLine();
      if (!isAdministratorAccount) {
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
            ReturnBook();
            break;
          case "4": // 4: List Borrow Record
            ListBorrowRecord();
            break;
          case "5": // 5: Change Password");
            ChangePassword();
            break;
          case "6":
          case "7":
          case "8":
          case "9":
          default:
            Console.Out.WriteLine("Your input is {0}", userChoice);
            Console.Out.WriteLine("Can't find match methods, please check your input");
            break;
        }
      } else {
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
            ReturnBook();
            break;
          case "4": // 4: List Borrow Record
            ListBorrowRecord();
            break;
          case "5": // 5: Change Password");
            ChangePassword();
            break;
          case "6": // 6: (Administrator only) Add User
            AddUser();
            break;
          case "7": // 7: (Administrator only) Add Books
            AddBooks();
            break;
          case "8": // 8: (Administrator only) Show All Record
            ShowAllRecord();
            break;
          case "9": // 9: (Administrator only) Delete User
            DeleteUser();
            break;
          default:
            Console.Out.WriteLine("Your input is {0}", userChoice);
            Console.Out.WriteLine("Can't find match methods, please check your input");
            break;
        }
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