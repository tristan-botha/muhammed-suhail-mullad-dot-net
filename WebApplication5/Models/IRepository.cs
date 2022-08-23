using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllChangesAsync();
        Task<Author[]> GetAllAuthors();
        Task<Author> GetAuthorByGuid(Guid id);
        Task<Book[]> GetAllBooks();
        Task<Book> GetBookById(Guid id);
        Task<Book[]> GetBooksByAuthorId(Guid id);
        
    }
}
