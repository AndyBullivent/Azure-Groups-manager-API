using cumbria.services.storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cumbria.services.groups.api.Models
{
    public class VmWhiteListItem:Group
    {
        public bool IsWhitelisted { get; set; }
    }
}