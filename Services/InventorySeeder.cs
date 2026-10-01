using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteca.Services
{
    public static class InventorySeeder
    {
        private static readonly string[] Books =
    {
        "Cien años de soledad",
        "El principito",
        "1984",
        "Rayuela",
        "Ficciones"
    };

        private static readonly string[] Branches =
        {
        "Sede Central",
        "Sucursal Norte",
        "Sucursal Sur"
    };

        private static readonly int[,] InitialStock =
        {
        { 12,  5,  8 },
        {  3, 10,  7 },
        {  6,  2,  9 },
        {  4,  1, 15 },
        {  8,  0,  3 }
    };

        public static IInventory Create()
        {
            MatrixInventory inventory = new MatrixInventory(Books, Branches);

            for (int book = 0; book < Books.Length; book++)
            {
                for (int branch = 0; branch < Branches.Length; branch++)
                {
                    inventory.SetStock(book, branch, InitialStock[book, branch]);
                }
            }
            return inventory;
        }
    }
}
