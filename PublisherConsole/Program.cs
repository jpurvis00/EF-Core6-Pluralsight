
using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using System;
using System.Net.Http.Headers;

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
//GetAuthors();
//FilterUsingRelatedData();
//ModifyingRelatedDataWhenTracked();
//ConnectExistingArtistAndCoverObjects();
//CreateNewCoverWithExistingArtist();
//CreateNewCoverAndArtistTogether();
//RetrieveAnArtistWithTheirCovers();
//RetrieveACoverWithItsArtists();
//GetAllBooksWithTheirCovers();
//MultiLevelInclude();
//SimpleRawSQL();
//TwoWaysToCallStoredProceduresUsingRawSql();
DeleteCover(4);

/* The following runs the DeleteCover stored procedure. It deletes the cover id passed to it. This delete
 * also has a cascading delete and deletes any data related to that cover in any of the other tables.
 */
void DeleteCover(int coverId)
{
    var rowCount = _context.Database.ExecuteSqlRaw("DeleteCover {0}", coverId);
    Console.WriteLine(rowCount);
}

void TwoWaysToCallStoredProceduresUsingRawSql()
{
    //Raw sql call
    var authors = _context.Authors
        .FromSqlRaw("AuthorsPublishedInYearRange {0}, {1}", 2010, 2015)
        .ToList();

    foreach (var author in authors)
    {
        Console.WriteLine($"Author: {author.FirstName}, {author.LastName}");
    }

    //Interpolated sql call
    int start = 1960;
    int end = 2000;

    var authors2 = _context.Authors
        .FromSqlInterpolated($"AuthorsPublishedInYearRange {start}, {end}")
        .ToList();

    Console.WriteLine();
    foreach (var author in authors)
    {
        Console.WriteLine($"Author: {author.FirstName}, {author.LastName}");
    }
}

void SimpleRawSQL()
{
    var authors = _context.Authors.FromSqlRaw("SELECT * FROM Authors").ToList();

    foreach (var author in authors)
    {
        Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
    }

    /* Building onto our raw sql with Linq methods */
    var authorsLinq = _context.Authors.FromSqlRaw("SELECT * FROM Authors")
                        .Include(a => a.Books).ToList();

    foreach (var author in authorsLinq)
    {
        Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
        Console.Write($"Books: ");
        author.Books.ForEach(b => Console.Write(b.Title + " "));
        Console.WriteLine();
        Console.WriteLine();
    }
}

/* This shows getting data from multiple tables. The tables must have navigation properties or foreign
 * keys to get this to work though. The Authors table has a navigation property(List<Books>) which 
 * points to the Books table. The Books table has a nav prop(Cover Cover) that points to the Cover
 * table. The Cover table has a nav prop(List<Artist> that points to the Artists table.
 */
void MultiLevelInclude()
{
    var authorGraph = _context.Authors.AsNoTracking()
        .Include(a => a.Books)
        .ThenInclude(b => b.Cover)
        .ThenInclude(c => c.Artists)
        .FirstOrDefault(a => a.AuthorId == 40);

    Console.WriteLine(authorGraph?.FirstName + " " + authorGraph?.LastName);

    foreach (var book in authorGraph.Books)
    {
        Console.WriteLine("Book: " + book.Title);

        if (book.Cover != null)
        {
            Console.WriteLine("Design Ideas: " + book.Cover.DesignIdeas);
            Console.Write("Artist(s): ");
            book.Cover.Artists.ForEach(a => Console.Write(a.LastName + " "));
        }
        Console.WriteLine();
        Console.WriteLine();
    }
}

/* Shows querying a 1-1 relationship. We added navigation properties to the Book/Cover classes along
 * with adding the BookId column to the Covers table. We also added a FK(FK_Covers_Books_BookId) to the Covers 
 * table along with an index
 */
void GetAllBooksWithTheirCovers()
{
    var booksAndCovers = _context.Books.Include(b => b.Cover).ToList();

    /* Bc there are books with no cover, we need to handle this with code. They will be returned as
     * null so we display "No cover yet".
     *:w
     */
    booksAndCovers.ForEach(book =>
        Console.WriteLine(
            book.Title +
            (book.Cover == null ? ": No cover yet" : ": " + book.Cover.DesignIdeas)));

    Console.WriteLine("\n\n");

    //This creates an anonymous object type with only 2 properties and assigns them values.
    var booksAndCovers2 = _context.Books.Where(b => b.Cover != null)
                            .Select(b => new {b.Title, b.Cover.DesignIdeas }).ToList(); 

    for (int i = 0; i < booksAndCovers2.Count; i++)
    {
        Console.WriteLine($"Title: {booksAndCovers2[i].Title}   DesignIdeas: {booksAndCovers2[i].DesignIdeas}");
    }
}

void RetrieveACoverWithItsArtists()
{
    var coverWithArtists = _context.Covers.Include(c => c.Artists)
                            .FirstOrDefault(c => c.CoverID == 5);
}

void RetrieveAnArtistWithTheirCovers()
{
    var artistWithCovers = _context.Artists.Include(a => a.Covers)
                            .FirstOrDefault(a => a.ArtistID == 1);
}

void CreateNewCoverAndArtistTogether()
{
    var newArtist = new Artist { FirstName = "Kir", LastName = "Talmage" };
    var newCover = new Cover { DesignIdeas = "We like birds!" };
    newArtist.Covers.Add(newCover);
    _context.Artists.Add(newArtist);
    _context.SaveChanges();
}

void CreateNewCoverWithExistingArtist()
{
    var artistA = _context.Artists.Find(1);
    var cover = new Cover { DesignIdeas = "Author has provided a photo" };
    cover.Artists.Add(artistA);
    _context.Covers.Add(cover);
    _context.SaveChanges();
}

void ConnectExistingArtistAndCoverObjects()
{
    var artistA = _context.Artists.Find(1);
    var artistB = _context.Artists.Find(2);
    var coverA = _context.Covers.Find(1);

    coverA.Artists.Add(artistA);
    coverA.Artists.Add(artistB);
    _context.SaveChanges();
}

void ModifyingRelatedDataWhenTracked()
{
    var author = _context.Authors.Include(a => a.Books)
        .FirstOrDefault(a => a.AuthorId == 25);
    author.Books[0].BasePrice = (decimal)11.99;
    _context.ChangeTracker.DetectChanges();
    var state = _context.ChangeTracker.DebugView.ShortView;
    _context.SaveChanges();
}

void FilterUsingRelatedData()
{
    //Returns only the author info even though we are using related data(Books) to filter the data on
    var recentAuthors = _context.Authors
            .Where(a => a.Books.Any(b => b.PublishDate.Year >= 2015))
            .ToList();
}

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

    //We can also combine LINQ methods. Using Where to filter the results followed by a sorting/ order by
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
    PubContext context2 = new PubContext();
    
    int groupSize = 2;

    for (int i = 0; i < 6; i++)
    {
        //List<Author> authors = _context.Authors.Skip(groupSize * i).Take(groupSize).ToList();
        List<Author> authors = context2.Authors.Skip(groupSize * i).Take(groupSize).ToList();

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
