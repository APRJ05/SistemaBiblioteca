using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Services
{
    public interface IInventory
    {
        int BookCount { get; }
        int BranchCount { get; }

        string GetBookTitle (int book);
        string GetBranchName (int branch);

        int GetStock(int branch, int book);
        OperationResult SetStock(int branch, int book, int quantity);

        int GetBookTotal(int book);
        int GetBranchTotal(int branch);
        IReadOnlyList<StockPosition> GetLowStock(int threshold);
        int GetBranchWithMostStock(int book);
    }
}
