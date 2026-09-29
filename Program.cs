using Biblioteca.Services;
using Biblioteca.UI;

IBookCatalog catalog = new ArrayBookCatalog(20);
BookSeeder.Load(catalog);

MenuController menu = new MenuController(catalog);
menu.Run();