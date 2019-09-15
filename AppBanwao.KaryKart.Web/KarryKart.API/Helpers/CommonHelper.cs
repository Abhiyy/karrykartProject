using AppBanwao.KarryKart.Model;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class APICommonHelper
    {
        static string _WebPortalLink = ConfigurationManager.AppSettings["WebPortalLink"].ToString();
        public static List<PanelModel> GetPanels(bool includeDeActivate=false, string ForPlatfrom="Mobile")
        {
            AddPageHit();
            using (karrykartEntities dbContext = new karrykartEntities())
            {
                List<PanelModel> panels = new List<PanelModel>();

                if (includeDeActivate)
                {
                    panels = dbContext.Panels.Where(x => x.For.ToLower() == ForPlatfrom.ToLower()).OrderBy(x=>x.ItemOrder).Select(x => new PanelModel()
                    {
                        ID = x.Id,
                        Title = x.Title,
                        Type = x.Type.Value,
                        Items = x.PanelItems.Select(p=> new PanelItems(){ ImageLink =p.ImageLink.Replace("~",_WebPortalLink),
                                                                          ItemID = p.ItemID,
                                                                          ItemType =p.ItemType.Value,
                                                                          Link = p.Link,
                                                                          PanelID = p.PanelID.Value,
                                                                          Title = p.Title
                                                    }).ToList()
                    }).ToList();

                }
                else {
                    panels = dbContext.Panels.Where(x => x.For.ToLower() == ForPlatfrom.ToLower() && x.Active.Value).OrderBy(x => x.ItemOrder).Select(x => new PanelModel()
                    {
                        ID = x.Id,
                        Title = x.Title,
                        Type = x.Type.Value,
                        Items = x.PanelItems.Select(p=> new PanelItems(){ ImageLink = p.ImageLink.Replace("~",_WebPortalLink),
                                                                          ItemID = p.ItemID,
                                                                          ItemType =p.ItemType.Value,
                                                                          Link = p.Link,
                                                                          PanelID = p.PanelID.Value,
                                                                          Title = p.Title
                                                    }).ToList()
                    }).ToList();
                }

                return panels;
              
            }
        }

        public static string GetImageLink(string url)
        {
        return url.Replace("~",_WebPortalLink);
        }

        public static void AddPageHit()
        {
            using (karrykartEntities _context = new karrykartEntities())
            {
                _context.UpdatePageHit();
            }
        }

        public static void AddUserAlert(Guid UserID, string message)
        {
            using (karrykartEntities context = new karrykartEntities())
            { 
            context.UserAlerts.Add(new UserAlert(){
                UserID = UserID,
                Message = message,
                CreatedAt = DateTime.Now
            });
            context.SaveChanges();
            }
        }

        public enum OrderStatus { 
            Placed = 1,
            Confirmed = 2,
            Dispatched = 3,
            InTransit = 4,
            Delievered = 5,
            Cancel = 6
        }
    }
}