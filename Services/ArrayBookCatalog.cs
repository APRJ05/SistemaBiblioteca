using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Services
{
    public class ArrayBookCatalog : IBookCatalog
    {
        private readonly Book[] _books;
        private int _count;

        public ArrayBookCatalog(int capacity = 20)
        {
            _books = new Book[capacity];
        }

        public int Count => _count;
        public int Capacity => _books.Length;
        public bool IsFull => _count == _books.Length;

        public OperationResult Add(Book book)
        {
            if (IsFull)
                return OperationResult.CatalogFull;

            if (IndexOf(book.Id) != -1)
                return OperationResult.DuplicateId;

            _books[_count] = book;
            _count++;
            return OperationResult.Success;
        }

        public IReadOnlyList<Book> GetAll()
        {
            Book[] snapshot = new Book[_count];
            for (int i = 0; i < _count; i++)
            {
                snapshot [i] = _books[i];
            }
            return snapshot;
        }

        public Book? FindById(int id)
        {
            int index = IndexOf(id);
            return index == -1 ? null : _books[index];
        }

        public OperationResult Update(int id, string? title, string? author, int? year)
        {
            Book? book = FindById(id);
            if (book == null)
                return OperationResult.NotFound;

            if (title != null) book.Title = title;
            if (author != null) book.Author = author;
            if (year.HasValue) book.Year = year.Value;

            return OperationResult.Success;
        }

        public OperationResult Remove(int id)
        {
            int index = IndexOf(id);
            if (index == -1)
                return OperationResult.NotFound;

            for (int i = index; i < _count - 1; i++)
            {
                _books[i] = _books[i + 1];
            }

            _books[_count - 1] = null!;
            _count--;
            return OperationResult.Success;
        }

        private int IndexOf(int id)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_books[i].Id == id)
                    return i;
            }
            return -1;
        }
    }
}
