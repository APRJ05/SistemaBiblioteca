using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.UI
{
    public static class ConsoleInput
    {
        public static int ReadInt(string message)
        {
            int value;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Valor inválido. " + message);
            }
            return value;
        }

        public static int ReadIntInRange(string message, int min, int max)
        {
            int value = ReadInt(message);
            while (value < min || value > max)
            {
                Console.Write($"Debe estar entre {min} y {max}. ");
                value = ReadInt(message);
            }
            return value;
        }

        //public static string ReadRequiredText(string message)
        //{
        //    string? text;
        //    do
        //    {
        //        Console.Write(message);
        //        text = Console.ReadLine();
        //    } while (string.IsNullOrWhiteSpace(text));
        //    return text.Trim();
        //}

        //public static string? ReadOptionalText(string message)
        //{
        //    Console.Write(message);
        //    string? text = Console.ReadLine();
        //    return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
        //}

        //public static int? ReadOptionalInt(string message)
        //{
        //    Console.Write(message);
        //    string? text = Console.ReadLine();
        //    return int.TryParse(text, out int value) ? value : null;
        //}
    }
}
