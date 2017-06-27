using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TzedakahFund.Data;

namespace TzedakahFund.Models
{
    public class ApplicationViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public User User { get; set; }
    }
}