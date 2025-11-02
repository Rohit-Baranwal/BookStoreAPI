using Xunit;
using BookStoreAPI.Controllers;
using BookStoreAPI.Data;
using BookStoreAPI.Models;
using BookStoreAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Test
{
    public class BorrowControllerTests
    {
        private BookStoreContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<BookStoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                // Suppress transaction warnings since InMemory doesn't support them
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new BookStoreContext(options);
        }

        [Fact]
        public async Task Borrow_Decrements_AvailableCopies_And_Creates_Record()
        {
            // Arrange
            var context = GetInMemoryContext();

            var book = new Book
            {
                Title = "Test",
                Author = "Unknown",
                ISBN = "111",
                PublishedYear = 2020,
                AvailableCopies = 2
            };
            var member = new Member
            {
                Name = "Test Member",
                Email = "t@test.com",
                Phone = "1234567890"
            };

            context.Books.Add(book);
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new BorrowController(context);
            var dto = new BorrowRequestDto
            {
                BookId = book.BookId,
                MemberId = member.MemberId
            };

            // Act
            var result = await controller.Borrow(dto);

            // Assert
            Assert.NotNull(result);

            var updatedBook = await context.Books.FindAsync(book.BookId);
            Assert.Equal(1, updatedBook.AvailableCopies);

            var borrowRecord = await context.BorrowRecords
                .FirstOrDefaultAsync(br => br.BookId == book.BookId && br.MemberId == member.MemberId);

            Assert.NotNull(borrowRecord);
            Assert.False(borrowRecord.IsReturned);
        }
    }
}
