//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppBanwao.KarryKart.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Panel
    {
        public Panel()
        {
            this.PanelItems = new HashSet<PanelItem>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public Nullable<int> Type { get; set; }
        public string For { get; set; }
        public Nullable<int> ItemOrder { get; set; }
        public Nullable<bool> Active { get; set; }
        public string html { get; set; }
    
        public virtual ICollection<PanelItem> PanelItems { get; set; }
    }
}
