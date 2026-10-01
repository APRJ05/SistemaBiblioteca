using Biblioteca.Models;
using Biblioteca.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.UI
{
    public class MenuController
    {
        private const int LowStockThreshold = 3;

        private readonly IInventory _inventory;

        public MenuController(IInventory inventory)
        {
            _inventory = inventory;
        }

        public void Run()
        {
            int option;

            do
            {
                ShowMenu();
                option = ConsoleInput.ReadInt("Seleccione una opción: ");
                Console.WriteLine();

                switch (option)
                {
                    case 1: ShowInventory(); break;
                    case 2: QueryStock(); break;
                    case 3: UpdateStock(); break;
                    case 4: ShowBookTotal(); break;
                    case 5: ShowBranchTotal(); break;
                    case 6: ShowLowStock(); break;
                    case 7: ShowBranchWithMostStock(); break;
                    case 0: Console.WriteLine("Saliendo del sistema..."); break;
                    default: Console.WriteLine("Opción inválida."); break;
                }

                if (option != 0)
                {
                    Console.WriteLine("\nPresione una tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            } while (option != 0);
        }

        private static void ShowMenu()
        {
            Console.WriteLine("====== INVENTARIO POR SUCURSAL ======\n");
            Console.WriteLine("1. Mostrar inventario completo");
            Console.WriteLine("2. Consultar disponibilidad");
            Console.WriteLine("3. Actualizar disponibilidad");
            Console.WriteLine("4. Total disponible de un libro");
            Console.WriteLine("5. Total de inventario por sucursal");
            Console.WriteLine("6. Mostrar libros con bajo inventario");
            Console.WriteLine("7. Sucursal con mayor disponibilidad");
            Console.WriteLine("0. Salir\n");
        }

        private int ReadBook()
        {
            Console.WriteLine("Libros:");
            for (int book = 0; book < _inventory.BookCount; book++)
            {
                Console.WriteLine($"  {book + 1}. {_inventory.GetBookTitle(book)}");
            }

            int number = ConsoleInput.ReadIntInRange("Número del libro: ", 1, _inventory.BookCount);
            return number - 1;
        }

        private int ReadBranch()
        {
            Console.WriteLine("Sucursales:");
            for (int branch = 0; branch < _inventory.BranchCount; branch++)
            {
                Console.WriteLine($"  {branch + 1}. {_inventory.GetBranchName(branch)}");
            }

            int number = ConsoleInput.ReadIntInRange("Número de la sucursal: ", 1, _inventory.BranchCount);
            return number - 1;
        }

        private void ShowInventory()
        {
            Console.Write("{0,-28}", "Libro");
            for (int branch = 0; branch < _inventory.BranchCount; branch++)
            {
                Console.Write("{0,-18}", _inventory.GetBranchName(branch));
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 28 + 18 * _inventory.BranchCount));

            for (int book = 0; book < _inventory.BookCount; book++)
            {
                Console.Write("{0,-28}", $"{book + 1}. {_inventory.GetBookTitle(book)}");
                for (int branch = 0; branch < _inventory.BranchCount; branch++)
                {
                    Console.Write("{0,-18}", _inventory.GetStock(book, branch));
                }
                Console.WriteLine();
            }
        }

        private void QueryStock()
        {
            int book = ReadBook();
            Console.WriteLine();
            int branch = ReadBranch();

            int quantity = _inventory.GetStock(book, branch);

            Console.WriteLine($"\n\"{_inventory.GetBookTitle(book)}\" tiene {quantity} unidades disponibles en {_inventory.GetBranchName(branch)}.");
        }

        private void UpdateStock()
        {
            int book = ReadBook();
            Console.WriteLine();
            int branch = ReadBranch();

            int current = _inventory.GetStock(book, branch);
            Console.WriteLine($"\nCantidad actual: {current}");
            int quantity = ConsoleInput.ReadInt("Nueva cantidad: ");

            OperationResult result = _inventory.SetStock(book, branch, quantity);

            Console.WriteLine(result switch
            {
                OperationResult.Success => "Disponibilidad actualizada correctamente.",
                OperationResult.InvalidQuantity => "La cantidad no puede ser negativa.",
                OperationResult.InvalidBook => "El libro indicado no existe.",
                OperationResult.InvalidBranch => "La sucursal indicada no existe.",
                _ => "No se pudo actualizar la disponibilidad."
            });
        }

        private void ShowBookTotal()
        {
            int book = ReadBook();

            int total = _inventory.GetBookTotal(book);

            Console.WriteLine($"\n\"{_inventory.GetBookTitle(book)}\" tiene {total} unidades en total entre todas las sucursales.");
        }

        private void ShowBranchTotal()
        {
            int branch = ReadBranch();

            int total = _inventory.GetBranchTotal(branch);

            Console.WriteLine($"\n{_inventory.GetBranchName(branch)} tiene {total} unidades en total entre todos los libros.");
        }

        private void ShowLowStock()
        {
            IReadOnlyList<StockPosition> lowStock = _inventory.GetLowStock(LowStockThreshold);

            if (lowStock.Count == 0)
            {
                Console.WriteLine($"No hay libros con {LowStockThreshold} unidades o menos.");
                return;
            }

            Console.WriteLine($"Libros con {LowStockThreshold} unidades o menos:\n");
            Console.WriteLine("{0,-28} {1,-18} {2,-10} {3}", "Libro", "Sucursal", "Posición", "Cantidad");
            Console.WriteLine(new string('-', 70));

            foreach (StockPosition position in lowStock)
            {
                Console.WriteLine("{0,-28} {1,-18} {2,-10} {3}",
                    _inventory.GetBookTitle(position.BookIndex),
                    _inventory.GetBranchName(position.BranchIndex),
                    $"[{position.BookIndex},{position.BranchIndex}]",
                    position.Quantity);
            }
        }

        private void ShowBranchWithMostStock()
        {
            int book = ReadBook();

            int branch = _inventory.GetBranchWithMostStock(book);
            int quantity = _inventory.GetStock(book, branch);

            Console.WriteLine($"\nLa sucursal con mayor disponibilidad de \"{_inventory.GetBookTitle(book)}\" es {_inventory.GetBranchName(branch)} con {quantity} unidades.");
        }
    }


    //private static void ShowMenu()
    //{
    //    Console.WriteLine("====== SISTEMA DE BIBLIOTECA ======\n");
    //    Console.WriteLine("1. Agregar libro");
    //    Console.WriteLine("2. Listar libros");
    //    Console.WriteLine("3. Buscar libro por ID");
    //    Console.WriteLine("4. Actualizar libro");
    //    Console.WriteLine("5. Eliminar libro");
    //    Console.WriteLine("0. Salir\n");
    //}

    //private void AddBook()
    //{
    //    if (_catalog.IsFull)
    //    {
    //        Console.WriteLine($"El catálogo está lleno ({_catalog.Capacity} libros). No se pueden agregar más.");
    //        return;
    //    }

    //    int id = ConsoleInput.ReadInt("ID: ");
    //    string title = ConsoleInput.ReadRequiredText("Título: ");
    //    string author = ConsoleInput.ReadRequiredText("Autor: ");
    //    int year = ConsoleInput.ReadInt("Año: ");

    //    OperationResult result = _catalog.Add(new Book(id, title, author, year));

    //    Console.WriteLine(result switch
    //    {
    //        OperationResult.Success => "Libro agregado correctamente.",
    //        OperationResult.DuplicateId => "Ya existe un libro con ese ID.",
    //        OperationResult.CatalogFull => "El catálogo está lleno.",
    //        _ => "No se pudo agregar el libro."
    //    });
    //}

    //private void ListBooks()
    //{
    //    IReadOnlyList<Book> books = _catalog.GetAll();

    //    if (books.Count == 0)
    //    {
    //        Console.WriteLine("No hay libros registrados.");
    //        return;
    //    }

    //    Console.WriteLine("{0,-5} {1,-30} {2,-28} {3,-6}", "ID", "Título", "Autor", "Año");
    //    Console.WriteLine(new string('-', 72));
    //    foreach (Book b in books)
    //    {
    //        Console.WriteLine("{0,-5} {1,-30} {2,-28} {3,-6}", b.Id, b.Title, b.Author, b.Year);
    //    }
    //    Console.WriteLine($"\nTotal: {_catalog.Count} de {_catalog.Capacity}");
    //}

    //private void SearchBook()
    //{
    //    int id = ConsoleInput.ReadInt("ID del libro a buscar: ");
    //    Book? book = _catalog.FindById(id);

    //    if (book == null)
    //    {
    //        Console.WriteLine("El libro no existe.");
    //        return;
    //    }

    //    Console.WriteLine("\nLibro encontrado:");
    //    Console.WriteLine("ID: " + book.Id);
    //    Console.WriteLine("Título: " + book.Title);
    //    Console.WriteLine("Autor: " + book.Author);
    //    Console.WriteLine("Año: " + book.Year);
    //}

    //private void UpdateBook()
    //{
    //    int id = ConsoleInput.ReadInt("ID del libro a actualizar: ");
    //    Book? book = _catalog.FindById(id);

    //    if (book == null)
    //    {
    //        Console.WriteLine("El libro no existe.");
    //        return;
    //    }

    //    Console.WriteLine("Deje vacío para mantener el valor actual.");
    //    string? title = ConsoleInput.ReadOptionalText($"Nuevo título ({book.Title}): ");
    //    string? author = ConsoleInput.ReadOptionalText($"Nuevo autor ({book.Author}): ");
    //    int? year = ConsoleInput.ReadOptionalInt($"Nuevo año ({book.Year}): ");

    //    OperationResult result = _catalog.Update(id, title, author, year);

    //    Console.WriteLine(result == OperationResult.Success
    //        ? "Libro actualizado correctamente."
    //        : "No se pudo actualizar el libro.");
    //}

    //private void DeleteBook()
    //{
    //    int id = ConsoleInput.ReadInt("ID del libro a eliminar: ");
    //    OperationResult result = _catalog.Remove(id);

    //    Console.WriteLine(result == OperationResult.Success
    //        ? "Libro eliminado correctamente."
    //        : "El libro no existe.");
    //}
}
