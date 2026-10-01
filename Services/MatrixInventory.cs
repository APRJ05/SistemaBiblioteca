using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Services
{
    public class MatrixInventory : IInventory
    {
        private readonly string[] _books;
        private readonly string[] _branches;
        private readonly int[,] _stock;

        public MatrixInventory(string[] books, string[] branches)
        {
            _books = books;
            _branches = branches;
            _stock = new int[books.Length, branches.Length];
        }

        public int BookCount => _books.Length;
        public int BranchCount => _branches.Length;

        public string GetBookTitle(int book)
        {
            ValidateBook(book);
            return _books[book];
        }

        public string GetBranchName(int branch)
        {
            ValidateBranch(branch);
            return _branches[branch];
        }

        public int GetStock(int book, int branch)
        {
            ValidateBook(book);
            ValidateBranch(branch);
            return _stock[book, branch];
        }

        public OperationResult SetStock(int book, int branch, int quantity)
        {
            if (!IsValidBook(book))
                return OperationResult.InvalidBook;

            if (!IsValidBranch(branch))
                return OperationResult.InvalidBranch;

            if (quantity < 0)
                return OperationResult.InvalidQuantity;

            _stock[book, branch] = quantity;
            return OperationResult.Success;
        }

        public int GetBookTotal(int book)
        {
            ValidateBook(book);

            int total = 0;
            for (int branch = 0; branch < BranchCount; branch++)
            {
                total += _stock[book, branch];
            }
            return total;
        }

        public int GetBranchTotal(int branch)
        {
            ValidateBranch(branch);

            int total = 0;
            for (int book = 0; book < BookCount; book++)
            {
                total += _stock[book, branch];
            }
            return total;
        }

        public IReadOnlyList<StockPosition> GetLowStock(int threshold)
        {
            List<StockPosition> result = new List<StockPosition>();

            for (int book = 0; book < BookCount; book++)
            {
                for (int branch = 0; branch < BranchCount; branch++)
                {
                    if (_stock[book, branch] <= threshold)
                    {
                        result.Add(new StockPosition(book, branch, _stock[book, branch]));
                    }
                }
            }
            return result;
        }

        public int GetBranchWithMostStock(int book)
        {
            ValidateBook(book);

            int best = 0;
            for (int branch = 1; branch < BranchCount; branch++)
            {
                if (_stock[book, branch] > _stock[book, best])
                {
                    best = branch;
                }
            }
            return best;
        }

        private bool IsValidBook(int book) => book >= 0 && book < _books.Length;

        private bool IsValidBranch(int branch) => branch >= 0 && branch < _branches.Length;

        private void ValidateBook(int book)
        {
            if (!IsValidBook(book))
                throw new ArgumentOutOfRangeException(nameof(book), "El libro indicado no existe.");
        }

        private void ValidateBranch(int branch)
        {
            if (!IsValidBranch(branch))
                throw new ArgumentOutOfRangeException(nameof(branch), "La sucursal indicada no existe.");
        }
    }
}
