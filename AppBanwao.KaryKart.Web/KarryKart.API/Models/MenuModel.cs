using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class MenuModel
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public string Link { get; set; }

        public string ImageLink { get; set; }

        public short Type { get; set; }
    }
}