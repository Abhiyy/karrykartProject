using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class LocationHelper
    {
        karrykartEntities _dbContext = new karrykartEntities();

        public List<CityModel> GetCity(int StateID = -1)
        {
            List<CityModel> cities = null;
            if (StateID == -1)
            {
                cities = new List<CityModel>();
                cities = _dbContext.refCities.Select(x => new CityModel() { ID = x.CityID, Name = x.Name }).ToList();
            }
            else {
                cities = new List<CityModel>();
                cities = _dbContext.refCities.Where(x=>x.StateID==StateID).Select(x => new CityModel() { ID = x.CityID, Name = x.Name }).ToList();
            }
            return cities;
        }
    }
}