using Biblioteca.Services;
using Biblioteca.UI;

IInventory inventory = InventorySeeder.Create();

MenuController menu = new MenuController(inventory);
menu.Run();