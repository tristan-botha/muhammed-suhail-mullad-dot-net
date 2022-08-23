using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class Author
    {
        
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
