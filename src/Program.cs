using System;

namespace Protobuf.Csharp.Example {
    class Program {
        static void Main(string[] args) {
            var book = new Model.Book {
                Name= "123",
                Author = "123"
            };
            var user = new Model.User {
                Name = "jskyzero",
                Password = "123456",
            };
            user.IsAdminstrator = true;
            Console.WriteLine(user.IsDefault);
        }
    }
}