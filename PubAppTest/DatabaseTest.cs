using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using System.Diagnostics;

namespace PubAppTest
{
    [TestClass]
    public class DatabaseTest 
    {
        [TestMethod]
        public void CanInsertAuthorIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = PubTestData;");
            //builder.UseSqlServer("Server=JEFFP-SURFACE;Database=PubDatabase;Trusted_Connection=True;");

            using(var context = new PubContext(builder.Options))
            {
                /* Don't think I would want to use EnsureDeleted in a production environment.
                 * This is just for testing purposes.  I want to make sure that the database
                 * is clean before I start testing.  I want to make sure that the database is
                 * created and that the Author table is created.  I want to make sure that the
                 * Author table is empty.  I want to make sure that I can insert an Author into
                 * the database.  I want to make sure that the AuthorId is not 0 after the insert.
                 */
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                
                var author = new Author { FirstName = "a", LastName = "b"};
                
                context.Authors.Add(author);
                Debug.WriteLine($"Before Save: {author.AuthorId}");
                context.SaveChanges();
                Debug.WriteLine($"After Save: {author.AuthorId}");
                
                Assert.AreNotEqual(0, author.AuthorId);
            }
        }
    }
}