using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Biblioteka
{
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
    }

    class Program
    {
        static string databasePath = "/Users/jakubkoprowski/Desktop/wsb/pppdz/biblioteka/biblioteka/biblioteka.txt";
        static List<Book> library = new List<Book>();

        static void Main(string[] args)
        {
            LoadLibraryFromDatabase();

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("Witaj w bibliotece.");
                Console.WriteLine("1. Dodaj ksiązkę.");
                Console.WriteLine("2. Usuń ksiązkę.");
                Console.WriteLine("3. Wypozycz ksiązkę.");
                Console.WriteLine("4. Wyszukaj ksiązkę.");
                Console.WriteLine("5. Edytuj szczegóły ksiązki.");
                Console.WriteLine("6. Wyświetl listę ksiązek.");
                Console.WriteLine("7. Zakończ.");
                Console.WriteLine("Wpisz numer operacji: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddBook();
                            break;
                        case 2:
                            DeleteBook();
                            break;
                        case 3:
                            IssueBook();
                            break;
                        case 4:
                            SearchBook();
                            break;
                        case 5:
                            EditBook();
                            break;
                        case 6:
                            DisplayAllBooks();
                            break;
                        case 7:
                            isRunning = false;
                            break;
                        default:
                            Console.WriteLine("Błąd. Spróbój ponownie.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Błąd. Wpisz numer.");
                }
            }

            SaveLibraryToDatabase();
        }

        static void LoadLibraryFromDatabase()
        {
            if (File.Exists(databasePath))
            {
                var lines = File.ReadAllLines(databasePath);
                foreach (var line in lines)
                {
                    var bookInfo = line.Split(',');
                    if (bookInfo.Length == 4)
                    {
                        var book = new Book
                        {
                            Title = bookInfo[0],
                            Author = bookInfo[1],
                            Year = int.Parse(bookInfo[2]),
                            IsAvailable = bool.Parse(bookInfo[3])
                        };
                        library.Add(book);
                    }
                }
            }
        }

        static void SaveLibraryToDatabase()
        {
            using (StreamWriter writer = new StreamWriter(databasePath))
            {
                foreach (var book in library)
                {
                    writer.WriteLine($"{book.Title},{book.Author},{book.Year},{book.IsAvailable}");
                }
            }
        }

        static void AddBook()
        {
            Console.WriteLine("Wpisz tytuł:");
            string title = Console.ReadLine();

            Console.WriteLine("Wpisz autora:");
            string author = Console.ReadLine();

            Console.WriteLine("Wpisz rok wydania:");
            int year;
            if (!int.TryParse(Console.ReadLine(), out year))
            {
                Console.WriteLine("Zły format roku.");
                return;
            }

            var book = new Book
            {
                Title = title,
                Author = author,
                Year = year,
                IsAvailable = true
            };

            library.Add(book);
            Console.WriteLine("Ksiązka dodana.");
        }

        static void DeleteBook()
        {
            Console.WriteLine("Wpisz tytuł ksiązki którą chcesz usunąć:");
            string titleToDelete = Console.ReadLine();

            var bookToDelete = library.FirstOrDefault(b => b.Title.Equals(titleToDelete, StringComparison.OrdinalIgnoreCase));

            if (bookToDelete != null)
            {
                library.Remove(bookToDelete);
                Console.WriteLine("Ksiązka usunięta z biblioteki.");
            }
            else
            {
                Console.WriteLine("Nie znaleziono ksiązki w bibliotece.");
            }
        }

        static void IssueBook()
        {
            Console.WriteLine("Wpisz tytuł ksiązki którą chcesz wypozyczyć:");
            string titleToIssue = Console.ReadLine();

            var bookToIssue = library.FirstOrDefault(b => b.Title.Equals(titleToIssue, StringComparison.OrdinalIgnoreCase));

            if (bookToIssue != null)
            {
                if (bookToIssue.IsAvailable)
                {
                    bookToIssue.IsAvailable = false;
                    Console.WriteLine("Ksiązka wypozyczona.");
                }
                else
                {
                    Console.WriteLine("Błąd. Ksiązka jest juz wypozyczona.");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono ksiązki w bibliotece.");
            }
        }

        static void SearchBook()
        {
            Console.WriteLine("Znajdź ksiązkę po:\n1. Tytule.\n2. Autorze.\n3. Roku wydania.");
            int searchChoice;
            if (int.TryParse(Console.ReadLine(), out searchChoice))
            {
                switch (searchChoice)
                {
                    case 1:
                        Console.WriteLine("Wpisz tytuł:");
                        string title = Console.ReadLine();
                        var booksByTitle = library.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
                        DisplayBooks(booksByTitle);
                        break;
                    case 2:
                        Console.WriteLine("Wpisz autora:");
                        string author = Console.ReadLine();
                        var booksByAuthor = library.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
                        DisplayBooks(booksByAuthor);
                        break;
                    case 3:
                        Console.WriteLine("Wpisz rok wydania:");
                        if (int.TryParse(Console.ReadLine(), out int year))
                        {
                            var booksByYear = library.Where(b => b.Year == year);
                            DisplayBooks(booksByYear);
                        }
                        else
                        {
                            Console.WriteLine("Błąd. Zły format roku.");
                        }
                        break;
                    default:
                        Console.WriteLine("Błąd wyboru.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Błąd. Wpisz liczbę.");
            }
        }
static void EditBook()
{
    Console.WriteLine("Wpisz tytuł ksiązki której szczegóły chcesz edytować:");
    string titleToEdit = Console.ReadLine();

    var bookToEdit = library.FirstOrDefault(b => b.Title.Equals(titleToEdit, StringComparison.OrdinalIgnoreCase));

    if (bookToEdit != null)
    {
        Console.WriteLine("Wybierz co chcesz edytować:");
        Console.WriteLine("1. Tytuł.");
        Console.WriteLine("2. Autora.");
        Console.WriteLine("3. Rok wydania.");

        int fieldChoice;
        if (int.TryParse(Console.ReadLine(), out fieldChoice))
        {
            switch (fieldChoice)
            {
                case 1:
                    Console.WriteLine("Wpisz nowy tytuł:");
                    string newTitle = Console.ReadLine();
                    bookToEdit.Title = newTitle;
                    break;
                case 2:
                    Console.WriteLine("Wpisz nowego autora:");
                    string newAuthor = Console.ReadLine();
                    bookToEdit.Author = newAuthor;
                    break;
                case 3:
                    Console.WriteLine("Wpisz nowy rok wydania:");
                    if (int.TryParse(Console.ReadLine(), out int newYear))
                    {
                        bookToEdit.Year = newYear;
                    }
                    else
                    {
                        Console.WriteLine("Błąd. Zły format roku.");
                    }
                    break;
                default:
                    Console.WriteLine("Błąd wyboru.");
                    break;
            }
            Console.WriteLine("Szczegóły ksiązki zostały zaaktualizowane.");
        }
        else
        {
            Console.WriteLine("Błąd. Wpisz liczbę.");
        }
    }
    else
    {
        Console.WriteLine("Nie znaleziono ksiązki w bibliotece.");
    }
}
static void DisplayAllBooks()
{
    if (library.Any())
    {
        Console.WriteLine("Lista wszystkich ksiązek:");
        foreach (var book in library)
        {
            string status = book.IsAvailable ? "Dostępna" : "Wypozyczona";
            Console.WriteLine($"Tytuł: {book.Title}, Autor: {book.Author}, Rok wydania: {book.Year}, Status: {status}");
        }
    }
    else
    {
        Console.WriteLine("Bibloteka jest pusta.");
    }
}

        static void DisplayBooks(IEnumerable<Book> books)
        {
            if (books.Any())
            {
                foreach (var book in books)
                {
                    string status = book.IsAvailable ? "Dostępna" : "Wypozyczona";
                    Console.WriteLine($"Tytuł: {book.Title}, Autor: {book.Author}, Rok: {book.Year}, Status: {status}");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono.");
            }
        }
    }
}
