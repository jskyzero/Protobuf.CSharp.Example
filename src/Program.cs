using System;

namespace Protobuf.Csharp.Example {
    class Program {
        static void Main(string[] args) {
            var book = new Model.Book {
                Name= "123",
                Author = "123"
            };
            Console.WriteLine(book.Author);
        }
    }
}