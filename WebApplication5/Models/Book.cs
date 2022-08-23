using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public DateTime DatePublished { get; set; }
        public int CopiesSold { get; set; }
        public virtual Author Author { get; set; }
        public Guid CreatedBy { get; set; }
        //public virtual MyProperty { get; set; }
    }
}
