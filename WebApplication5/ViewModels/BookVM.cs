using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.ViewModels
{
    public class BookVM
    {
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public DateTime DatePublished { get; set; }
        public int CopiesSold { get; set; }
    }
}
