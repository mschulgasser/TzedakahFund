using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TzedakahFund.Data;

namespace TzedakahFund.Models
{
    public class CategoriesViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}