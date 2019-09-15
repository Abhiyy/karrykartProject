using AppBanwao.KarryKart.Model;
using AppBanwao.KaryKart.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppBanwao.KaryKart.Web.Helpers
{
    public class ChartHelper
    {

        public static List<D3ChartModel.BarChart> GetSalesRevenueChart(string type = "months")
        {
            using (var context = new karrykartEntities())
            {
                return context.GetSalesRevenue(type).Select(x => new D3ChartModel.BarChart() { Category = x.Category, Value = x.Value.Value }).ToList();
            }
        }
    }
}