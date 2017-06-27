using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TzedakahFund.Data;

namespace TzedakahFund.Models
{
    public class AdminIndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}