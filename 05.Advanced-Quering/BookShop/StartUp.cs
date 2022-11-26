namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //int input = int.Parse(Console.ReadLine());

            //string input = Console.ReadLine();


            int result = RemoveBooks(db);

            Console.WriteLine(result);
            
        }

        //AgeRestriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            string[] books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(e => e.Title)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine($"{item}");
            }

            return sb.ToString().TrimEnd();
        }

        //GoldenBooks
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            string command = "Gold";

            EditionType goldenBooks = Enum.Parse<EditionType>(command, true);

            var books = context
                .Books
                .Where(b => b.EditionType == goldenBooks)
                .Where(b => b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }

        //BooksByPrice
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Price >= 40)
                .Select(b => new
                {
                    Title = b.Title,
                    Price = b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} - ${item.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //NotReleasedIn
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.ReleaseDate.HasValue &&
                        b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }

        //BookTitleByCategory not result
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] categories = input.Split();

            string[] books = null;

            foreach (var category in categories)
            {
                books = context
                .BooksCategories
                .Where(b => b.Category.Name == category)
                .Select(b => b.Book.Title)
                .ToArray();

                foreach (var item in books)
                {
                    sb.AppendLine($"{item}");
                }

            }

            
            return sb.ToString().TrimEnd();
        }

        //ReleasedBeforeDate not result
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            DateTime newDate = DateTime.Parse(date);

            var books = context
                .Books
                .Where(b =>
                        b.ReleaseDate.Value <= newDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {

                    Title = b.Title,
                    EditionType = b.EditionType,
                    Price = b.Price
                })
                .ToArray();

            foreach (var item in books)
            {
                sb
                    .AppendLine($"{item.Title} - {item.EditionType} - ${item.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //AuthorNames
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context
                .Authors
                .ToArray()
                .Where(a => a.FirstName.ToLower().EndsWith(input.ToLower()))
                .Select(a => new
                {
                    FullNAme = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a.FullNAme)
                .ToArray();

            foreach (var item in authors)
            {
                sb.AppendLine(item.FullNAme);
            }

            return sb.ToString().TrimEnd();
        }

        //BookSearch
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine(item);
            }

            return sb.ToString().TrimEnd();
        }

        //BookSearchByAuthor
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    Title = b.Title,
                    AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToArray();

            foreach (var item in books)
            {
                sb
                    .AppendLine($"{item.Title} ({item.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }


        //CountBooks
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray();

            int sum = books.Count();

            return sum;
        }

        //TotalBookCopies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context
                .Authors
                .Select(a => new
                {
                    Name = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(e => e.Copies)
                .ToArray();

            foreach (var item in authors)
            {
                sb.AppendLine($"{item.Name} - {item.Copies}");
            }

            return sb.ToString().TrimEnd();
                
        }

        //ProfitByCategory
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Categories
                .Select(b => new
                {
                    BookCategory = b.Name,
                    Profit = b.CategoryBooks
                        .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(a => a.Profit)
                .ThenBy(a => a.BookCategory)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.BookCategory} ${item.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //MostRecentBooks
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context
                .Categories
                .Select(b => new
                {
                    Category = b.Name,
                    Books = b.CategoryBooks
                        .Select(cb => cb.Book)
                        .OrderByDescending(a => a.ReleaseDate)
                        .Select(a => new
                        {
                            Title = a.Title,
                            year = a.ReleaseDate.Value.Year
                        })
                        .Take(3)
                })
                .OrderBy(a => a.Category)
                .ToArray();

            foreach (var item in books)
            {
                sb.AppendLine($"--{item.Category}");

                foreach (var item2 in item.Books)
                {
                    sb.AppendLine($"{item2.Title} ({item2.year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //IncreasePrice
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.ReleaseDate.HasValue &
                            b.ReleaseDate.Value.Year < 2010);

            foreach (var item in books)
            {
                item.Price += 5;
            }

            context.SaveChanges();
        }

        //ReomoveBooks
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            int deletedBooks = 0;

            foreach (var book in books)
            {
                context.Books.BulkDelete(books, null);
                deletedBooks++;
            }

            return deletedBooks;
        }
    }
}
