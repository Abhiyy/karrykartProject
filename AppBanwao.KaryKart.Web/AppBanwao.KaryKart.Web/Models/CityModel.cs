using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Models
{
    public class CityModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}