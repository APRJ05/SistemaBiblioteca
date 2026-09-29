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
        private readonly IBookCatalog _catalog;

        public MenuController(IBookCatalog catalog)
        {
            _catalog = catalog;
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
                    case 1: AddBook(); break;
                    case 2: ListBooks(); break;
                    case 3: SearchBook(); break;
                    case 4: UpdateBook(); break;
                    case 5: DeleteBook(); break;
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
            Console.WriteLine("====== SISTEMA DE BIBLIOTECA ======\n");
            Console.WriteLine("1. Agregar libro");
            Console.WriteLine("2. Listar libros");
            Console.WriteLine("3. Buscar libro por ID");
            Console.WriteLine("4. Actualizar libro");
            Console.WriteLine("5. Eliminar libro");
            Console.WriteLine("0. Salir\n");
        }

        private void AddBook()
        {
            if (_catalog.IsFull)
            {
                Console.WriteLine($"El catálogo está lleno ({_catalog.Capacity} libros). No se pueden agregar más.");
                return;
            }

            int id = ConsoleInput.ReadInt("ID: ");
            string title = ConsoleInput.ReadRequiredText("Título: ");
            string author = ConsoleInput.ReadRequiredText("Autor: ");
            int year = ConsoleInput.ReadInt("Año: ");

            OperationResult result = _catalog.Add(new Book(id, title, author, year));

            Console.WriteLine(result switch
            {
                OperationResult.Success => "Libro agregado correctamente.",
                OperationResult.DuplicateId => "Ya existe un libro con ese ID.",
                OperationResult.CatalogFull => "El catálogo está lleno.",
                _ => "No se pudo agregar el libro."
            });
        }

        private void ListBooks()
        {
            IReadOnlyList<Book> books = _catalog.GetAll();

            if (books.Count == 0)
            {
                Console.WriteLine("No hay libros registrados.");
                return;
            }

            Console.WriteLine("{0,-5} {1,-30} {2,-28} {3,-6}", "ID", "Título", "Autor", "Año");
            Console.WriteLine(new string('-', 72));
            foreach (Book b in books)
            {
                Console.WriteLine("{0,-5} {1,-30} {2,-28} {3,-6}", b.Id, b.Title, b.Author, b.Year);
            }
            Console.WriteLine($"\nTotal: {_catalog.Count} de {_catalog.Capacity}");
        }

        private void SearchBook()
        {
            int id = ConsoleInput.ReadInt("ID del libro a buscar: ");
            Book? book = _catalog.FindById(id);

            if (book == null)
            {
                Console.WriteLine("El libro no existe.");
                return;
            }

            Console.WriteLine("\nLibro encontrado:");
            Console.WriteLine("ID: " + book.Id);
            Console.WriteLine("Título: " + book.Title);
            Console.WriteLine("Autor: " + book.Author);
            Console.WriteLine("Año: " + book.Year);
        }

        private void UpdateBook()
        {
            int id = ConsoleInput.ReadInt("ID del libro a actualizar: ");
            Book? book = _catalog.FindById(id);

            if (book == null)
            {
                Console.WriteLine("El libro no existe.");
                return;
            }

            Console.WriteLine("Deje vacío para mantener el valor actual.");
            string? title = ConsoleInput.ReadOptionalText($"Nuevo título ({book.Title}): ");
            string? author = ConsoleInput.ReadOptionalText($"Nuevo autor ({book.Author}): ");
            int? year = ConsoleInput.ReadOptionalInt($"Nuevo año ({book.Year}): ");

            OperationResult result = _catalog.Update(id, title, author, year);

            Console.WriteLine(result == OperationResult.Success
                ? "Libro actualizado correctamente."
                : "No se pudo actualizar el libro.");
        }

        private void DeleteBook()
        {
            int id = ConsoleInput.ReadInt("ID del libro a eliminar: ");
            OperationResult result = _catalog.Remove(id);

            Console.WriteLine(result == OperationResult.Success
                ? "Libro eliminado correctamente."
                : "El libro no existe.");
        }
    }
}