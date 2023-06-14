using Bookstore.Api.Common.Dtos;
using Bookstore.Api.Common.Models;
using Bookstore.Api.Services.Interfaces;
using Dapr.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Api.Services
{
    public class BookService : IBookService
    {
        private static string STORE_NAME = "statestore";
        private readonly DaprClient _daprClient;
        private readonly ILogger<BookService> _logger;

        public BookService(DaprClient daprClient, ILogger<BookService> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        public async Task<string> CreateBook(BookDto bookDto)
        {
            try
            {
                var book = new Book
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = bookDto.Name,
                    Price = bookDto.Price,
                    Category = bookDto.Category,
                    Author = bookDto.Author
                };

                _logger.LogInformation($"Saving a new Book with Id: {book.Id} to state store");
                await _daprClient.SaveStateAsync<Book>(STORE_NAME, book.Id, book);
                return book.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(CreateBook)}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteBook(string id)
        {
            try
            {
                _logger.LogInformation($"Deleting Book with Id: {id}");
                await _daprClient.DeleteStateAsync(STORE_NAME, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(DeleteBook)}: {ex.Message}");
                throw;
            }
        }

        public async Task<Book?> GetBookById(string id)
        {
            try
            {
                _logger.LogError($"Getting Book with Id: {id}");
                var book = await _daprClient.GetStateAsync<Book>(STORE_NAME, id);
                return book;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetBookById)}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Book>> GetBooksByCategory(string category)
        {
            try
            {
                var query = "{" +
                    "\"filter\": {" +
                        "\"EQ\": { \"category\": \"" + category + "\" }" +
                    "}}";

                var queryResponse = await _daprClient.QueryStateAsync<Book>(STORE_NAME, query);

                var bookList = queryResponse.Results.Select(q => q.Data).OrderByDescending(q => q.Category).ToList();

                return bookList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(GetBooksByCategory)}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateBook(string id, BookDto bookDto)
        {
            try
            {
                _logger.LogInformation($"Updating book with Id: {id}");
                var currentBook = await _daprClient.GetStateAsync<Book>(STORE_NAME, id);
                if (currentBook is not null)
                {
                    currentBook.Name = bookDto.Name;
                    currentBook.Price = bookDto.Price;
                    currentBook.Category = bookDto.Category;
                    currentBook.Author = bookDto.Author;
                    await _daprClient.SaveStateAsync<Book>(STORE_NAME, currentBook.Id, currentBook);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown in {nameof(UpdateBook)}: {ex.Message}");
                throw;
            }
        }
    }
}
