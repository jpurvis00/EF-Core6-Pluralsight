
using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using System;

PubContext _context = new PubContext();

/* Commenting out since we know the db exists and don't need to do the
 * check everytime while we are testing 
 * This using statement checks to see if the db exists or not. If it 
 * doesn't, it creates it.
//using (PubContext context = new PubContext())
//{
//    context.Database.EnsureCreated();
//}
*/
//AddAuthors();

void AddAuthors()
{
    PubContext context = new PubContext();

    //context.Authors.Add(new Author { FirstName = "William", LastName = "Shakespeare", Books = new List<Book> { new Book { Title = "Hamlet", PublishDate = new DateTime(1600, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Mark", LastName = "Twain", Books = new List<Book> { new Book { Title = "The Adventures of Huckleberry Finn", PublishDate = new DateTime(1884, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Charles", LastName = "Dickens", Books = new List<Book> { new Book { Title = "A Tale of Two Cities", PublishDate = new DateTime(1859, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "John", LastName = "Steinbeck", Books = new List<Book> { new Book { Title = "The Grapes of Wrath", PublishDate = new DateTime(1939, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "John", LastName = "Grisham", Books = new List<Book> { new Book { Title = "The Firm", PublishDate = new DateTime(1991, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Stephen", LastName = "King", Books = new List<Book> { new Book { Title = "The Shining", PublishDate = new DateTime(1977, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Tom", LastName = "Clancy", Books = new List<Book> { new Book { Title = "The Hunt for Red October", PublishDate = new DateTime(1984, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Nora", LastName = "Roberts", Books = new List<Book> { new Book { Title = "The Witness", PublishDate = new DateTime(2012, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Jackie", LastName = "Collins", Books = new List<Book> { new Book { Title = "Hollywood Wives", PublishDate = new DateTime(1983, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Harold", LastName = "Robbins", Books = new List<Book> { new Book { Title = "The Carpetbaggers", PublishDate = new DateTime(1961, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Danielle", LastName = "Steel", Books = new List<Book> { new Book { Title = "The Promise", PublishDate = new DateTime(2005, 1, 1), BasePrice = 9.99m } } });
    //context.Authors.Add(new Author { FirstName = "Barbara", LastName = "Cartland", Books = new List<Book> { new Book { Title = "The Flame is Love", PublishDate = new DateTime(1978, 1, 1), BasePrice = 9.99m } } });

    //context.SaveChanges();

    /* We can accomplish the above with a bulk add using AddRange() */
    context.Authors.AddRange(new Author { FirstName = "Orson", LastName = "Card" },
                             new Author { FirstName = "Terry", LastName = "Brooks" },
                             new Author { FirstName = "Dr", LastName = "Suess" },
                             new Author { FirstName = "J.R.R.", LastName = "Tolkien" });

    context.SaveChanges();
}

//GetAuthors();
//GetAuthorsWithBook();
//QueryFilters();
//SkipAndTakeAuthors();
//ViewSortAuthors();
//GetAuthors();
//RetrieveAndUpdateAuthor();
Console.WriteLine();
//RetrieveAndUpdateMultipleAuthors();
//VariousOperations();
//DeleteAuthor();
GetAuthors();

void DeleteAuthor()
{
    var author = _context.Authors.Find(35);

    Console.WriteLine($"In delete: Found author: {author.AuthorId}: {author.FirstName} {author.LastName}");
    
    if (author != null)
    {
        _context.Authors.Remove(author);
        _context.SaveChanges();
    }
}

/* Demonstrating that EF Core does not care if you do multiple different operations
 * for 1 save/update.
 */
void VariousOperations()
{
    var author = _context.Authors.Find(22);
    author.FirstName = "Mark";

    var newAuthor = new Author { LastName = "Card", FirstName = "Orson" };
    _context.Authors.Add(newAuthor);

    _context.SaveChanges();

    Console.WriteLine($" {author.LastName}, {author.FirstName}");
}


void RetrieveAndUpdateMultipleAuthors()
{
    var authors = _context.Authors.Where(n => n.FirstName == "Jim").ToList();

    foreach (var auth in authors)
    {
        auth.FirstName = "John";
    }

    /* The following shows the objects that are being tracked and their status. They are 
     * not marked as modified until the DetectChanges is called. The SaveChanges calls it
     * so that we don't have to. This is just to show us what's going on behind the scenes.
     */
    Console.WriteLine($"Before: {_context.ChangeTracker.DebugView.ShortView}");
    _context.ChangeTracker.DetectChanges();
    Console.WriteLine($"After: {_context.ChangeTracker.DebugView.ShortView}");
    
    _context.SaveChanges();
}

void RetrieveAndUpdateAuthor()
{
    var author = _context.Authors.FirstOrDefault(a => a.FirstName == "Charlie" && a.LastName == "Dickens");

    if (author != null)
    {
        author.FirstName = "Charles";
        _context.SaveChanges();
    }
}

void ViewSortAuthors()
{
    //Orders by last name
    //List<Author> authors = _context.Authors.OrderBy(o => o.LastName).ToList();

    //Orders by first name. If multiple OrderBy are linked together, all are ignore but the first one.
    //List<Author> authors = _context.Authors.OrderBy(o => o.LastName)
    //                                       .OrderBy(o => o.FirstName).ToList();
   
    //To accomplish the above, we would use ThenBy
    //List<Author> authors = _context.Authors.OrderBy(o => o.LastName)
    //                                       .ThenBy(o => o.FirstName).ToList();

    //We can also call descending
    //List<Author> authors = _context.Authors.OrderByDescending(o => o.LastName)
    //                                       .ThenByDescending(o => o.FirstName).ToList();

    //We can also combine LINQ methods. Using Where to filter the results followed by a sorting/order by
    List<Author> authors = _context.Authors.Where(l => l.FirstName == "John")
                                           .OrderByDescending(o => o.LastName)
                                           .ThenByDescending(o => o.FirstName).ToList();

    DisplayAuthors(authors);
}

void DisplayAuthors(List<Author> authors)
{
    foreach (var author in authors)
    {
        Console.WriteLine($" {author.LastName}, {author.FirstName}");
    }
}

/* This is an example of a method that might limit the amount of results returned from
 * the query. We are using Skip and Take here. Maybe we only want to display the first 100
 * results returned from the query or if they click on page 3, only show results 300-400.
 */
void SkipAndTakeAuthors()
{
    int groupSize = 2;

    for (int i = 0; i < 6; i++)
    {
        List<Author> authors = _context.Authors.Skip(groupSize * i).Take(groupSize).ToList();

        Console.WriteLine($"Group {i}:");

        foreach (Author author in authors)
        {
            Console.WriteLine($" {author.FirstName} {author.LastName}");
        }
    }
}

void QueryFilters()
{
    //List<Author> authors = _context.Authors.Where(f => f.FirstName == "John").ToList();

    /* The following two statements are exactly the same. The do the same sql behind the scenes. */
    //List<Author> authors = _context.Authors.Where(f => f.FirstName.Contains("j")).ToList();
    List<Author> authors = _context.Authors.Where(f => EF.Functions.Like(f.FirstName, "j%")).ToList();


    foreach (Author author in authors)
    {
        Console.WriteLine(author.FirstName + " " + author.LastName);
    }
}

void GetAuthors()
{
    List<Author> authors = _context.Authors.ToList();

    foreach (Author author in authors)
    {
        Console.WriteLine($"{author.AuthorId}: {author.FirstName} {author.LastName}");
    }
}
void GetAuthorsWithBook()
{
    _context.Authors.Include(b => b.Books).ToList();

    foreach (Author author in _context.Authors)
    {
        Console.WriteLine($"{author.FirstName} {author.LastName}");

        foreach(Book book in author.Books)
        {
            Console.WriteLine($"    {book.Title}: {book.PublishDate}");
        }
    }
}
