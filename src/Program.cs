using System;

namespace protobuf.csharp.example {
    class Program {
        static void Main(string[] args) {
            var book = new Book {
                Name= "123",
                Author = "123"
            };
            Console.WriteLine(book.Author);
        }
    }
}