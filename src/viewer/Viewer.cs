using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Protobuf.Csharp.Example.Controller;

namespace Protobuf.Csharp.Example.Viewer {
  internal class Viewer : IDisposable{
    private static string kSaltString = "jskyzero";
    private string name = "";
    private string password = "";
    private bool isPasswordRight = false;


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
      while (isPasswordRight) {

      }
    }

    private void PrintCharacter() {
      Console.Out.Write("$> ");
    }

    private void PrintMenu() {
      Console.Out.WriteLine(@"
      1: Check Books
      2: Borrow Books
      3: Return Books
      4: List Borrow Record
      5: Change Password
      6: (Administrator only) Add User
      7: (Administrator only) Add Books
      8: (Administrator only) Show All Record
      9: (Administrator only) Delete User

      Enter your choices(use number)");
    }

    private void InitialAdministratorAccount() {
      Console.Out.WriteLine("First time running, please set password for default administrator account");
      PrintCharacter();
      password = PasswordHash(Console.In.ReadLine(), kSaltString);
      controller.AddAdministratorUser(password);
    }

    private void UserLogin() {
      Console.Out.WriteLine("[User Login]");
      Console.Out.WriteLine("Input your user name");
      PrintCharacter();
      name = Console.In.ReadLine();
      Console.Out.WriteLine("Input your password");
      PrintCharacter();
      password = PasswordHash(Console.In.ReadLine(), kSaltString);
      isPasswordRight = controller.CheckUserPassword(name, password);
    }

    private static string PasswordHash(string password, string salt) {
      byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
      byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
      byte[] saltedValue = passwordBytes.Concat(saltBytes).ToArray();
      byte[] hashedValue  = new SHA256Managed().ComputeHash(saltedValue);
      return Encoding.ASCII.GetString(hashedValue);
    }
  }
}