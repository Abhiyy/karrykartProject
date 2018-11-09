using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KarryKart.API.Models
{
    public class PanelModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public List<PanelItems> Items{get;set;}
    }

    public class PanelItems {
        public string Title { get; set; }
        public string ImageLink { get; set; }
        public string ItemID { get; set; }
        public int ItemType { get; set; }
        public string Link { get; set; }
        public int PanelID { get; set; }
    }

}