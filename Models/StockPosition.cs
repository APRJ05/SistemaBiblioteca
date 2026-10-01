using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Models
{
    public record StockPosition(int BookIndex, int BranchIndex, int Quantity);
}
