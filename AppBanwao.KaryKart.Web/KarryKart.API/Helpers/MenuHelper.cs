using AppBanwao.KarryKart.Model;
using KarryKart.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace KarryKart.API.Helpers
{
    public class MenuHelper
    {
        karrykartEntities _dbContext = null;
        enum MenuType { CATEGORY = 1, SUBCATEGORY = 2 };
        string _url = ConfigurationManager.AppSettings["WebUIUrl"].ToString();
        public List<KeyValuePair<MenuModel, List<MenuModel>>> GetMenu()
        {
            using (_dbContext = new karrykartEntities())
            {
                var categories = _dbContext.Categories.ToList();
                var subcategories = _dbContext.Subcategories.ToList();
                List<KeyValuePair<MenuModel, List<MenuModel>>> menu = new List<KeyValuePair<MenuModel,List<MenuModel>>>();
                MenuModel menuitem = null;
                MenuModel submenuitem = null;
                foreach (var category in categories)
                {
                    
                    menuitem = new MenuModel() { Name=category.Name,
                                                 Value=category.CategoryID,
                                                 Type=(short)MenuType.CATEGORY ,
                                                 Link=_url+"Product/CategoryView/"+category.CategoryID,
                                                 ImageLink = null };
                    var menusubcategories = subcategories.Where(x => x.CategoryID == category.CategoryID).ToList();
                    List<MenuModel> lstSubmenu = new List<MenuModel>();
                    foreach (var item in menusubcategories)
                    {
                        submenuitem = new MenuModel()
                        {
                            Name = item.Name,
                            Type = (short)MenuType.SUBCATEGORY,
                            Value = item.SCategoryID,
                            Link = _url + "Product/SubCategoryView/" + item.SCategoryID,
                            ImageLink=null
                        };

                        lstSubmenu.Add(submenuitem);
                        submenuitem = null;
                    }
                    menu.Add(new KeyValuePair<MenuModel, List<MenuModel>>(menuitem, lstSubmenu));
                    lstSubmenu = null;
                    menuitem = null;
                }

                return menu;
            }
            return null;
        }
    }
}