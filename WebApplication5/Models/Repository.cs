using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class Repository: IRepository
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void Add<T>(T entity) where T : class
        {
            _appDbContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _appDbContext.Remove(entity);
        }
        public async Task<bool> SaveAllChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<Author[]> GetAllAuthors()
        {
            IQueryable<Author> authors = _appDbContext.Authors;
            return await authors.ToArrayAsync();
        }

        public async Task<Author> GetAuthorByGuid(Guid id)
        {
            IQueryable<Author> author = _appDbContext.Authors.Where(x => x.AuthorId == id).Include(a=>a.Books);
            return await author.FirstOrDefaultAsync();
        }

        public async Task<Book[]> GetAllBooks()
        {
            IQueryable<Book> books = _appDbContext.Books.Include(a=> a.Author);
            return await books.ToArrayAsync();
        }
        public async Task<Book> GetBookById(Guid id)
        {
            IQueryable<Book> book = _appDbContext.Books.Where(x => x.BookId == id);
            return await book.FirstOrDefaultAsync();
        }

        public async Task<Book[]> GetBooksByAuthorId(Guid id)
        {
            IQueryable<Book> book = _appDbContext.Books.Where(b => b.Author.AuthorId == id);
            return await book.ToArrayAsync();
        }
    }
}
