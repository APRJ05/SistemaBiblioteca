//using Biblioteca.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Biblioteca.Services
//{
//    public interface IBookCatalog
//    {
//        int Count { get; }
//        int Capacity { get; }
//        bool IsFull { get; }

//        OperationResult Add(Book book);
//        IReadOnlyList<Book> GetAll();
//        Book? FindById(int id);
//        OperationResult Update(int id, string? title, string? author, int? year);
//        OperationResult Remove (int id);
//    }
//}
