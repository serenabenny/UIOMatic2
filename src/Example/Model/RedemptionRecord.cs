using System;
using System.Collections.Generic;
using UIOMatic.Attributes;
using UIOMatic.Enums;
using UIOMatic.Interfaces;
using Umbraco.Core.Persistence;

namespace Example.Model
{
    [UIOMaticAttribute("RedemptionRecord", "icon-users", "icon-user", RenderType = UIOMaticRenderType.List, ShowInTree = false)]
    [TableName("RedemptionRecord")]
    [PrimaryKey("Uid", autoIncrement = false)]
    [ExplicitColumns]
    public partial class RedemptionRecord : IUIOMaticModel
    {
        public const string TableName = "RedemptionRecord";

        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [Column]
        public Guid Uid { get; set; }
        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [Column]
        public Guid RedemptionID { get; set; }

        [Column("CreatedBy")]
        public int Winner { get; set; }
        [Column]
        [UIOMaticField("Confirmed DateTime", "", View = "datetime", IsCanEdit = false)]
        public DateTime? ConfirmedDateTime { get; set; }
        [Column]
        public DateTime? CollectedDateTime { get; set; }
        [Column]
        [UIOMaticField("Collection Expiry DateTime", "", View = "datetime", IsCanEdit = false)]
        public DateTime? CollectionExpiryDateTime { get; set; }
        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [Column]
        public DateTime CreatedDateTime { get; set; }

        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [Column]
        public DateTime UpdatedDateTime { get; set; }
        [UIOMaticIgnoreField]
        [UIOMaticIgnoreFromListView]
        [Column]
        public int UpdatedBy { get; set; }
        
        public override string ToString()
        {
            return Winner.ToString();
        }

        public IEnumerable<Exception> Validate()
        {
            var exs = new List<Exception>();
            
            return exs;
        }


        public void SetDefaultValue()
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.Now;
                Winner = 0;
            }
            UpdatedDateTime = DateTime.Now;
            UpdatedBy = 0;
        } 
    }
}