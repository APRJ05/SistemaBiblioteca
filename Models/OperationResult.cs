using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public enum OperationResult
    {
        Success,
        CatalogFull,
        DuplicateId,
        NotFound
    }
}
