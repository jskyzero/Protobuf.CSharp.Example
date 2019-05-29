using System;

namespace Protobuf.Csharp.Example {
    class Program {
        static void Main(string[] args) {
            // var book = new Model.Book {
            //     Name= "123",
            // };
            // var user = new Model.User {
            //     Name = "jskyzero",
            //     Password = "123456",
            // };
            Controller.Controller controller = new Controller.Controller();
            controller.SaveDataFile();
            // Console.WriteLine(user.IsDefault);
        }
    }
}