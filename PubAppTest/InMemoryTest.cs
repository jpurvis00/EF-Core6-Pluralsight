using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubAppTest
{
    [TestClass]
    public class InMemoryTest
    {
        [TestMethod]
        public void CanInsertAuthorIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();
            builder.UseInMemoryDatabase("CanInsertAuthorIntoDatabase");

            using (var context = new PubContext(builder.Options))
            {
                /* We don't need the EnsureDeleted/EnsureCreated method 
                 * here because we are using an in-memory database.
                 */
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();

                var author = new Author { FirstName = "a", LastName = "b" };

                context.Authors.Add(author);
                Debug.WriteLine($"Before Save: {author.AuthorId}");
                context.SaveChanges();
                Debug.WriteLine($"After Save: {author.AuthorId}");

                Assert.AreNotEqual(0, author.AuthorId);
            }
        }
    }
}
