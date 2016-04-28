using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UIOMatic.Attributes;
using UIOMatic.Enums;
using UIOMatic.Interfaces;
using Umbraco.Core.Persistence;

namespace Example.Model
{
    [UIOMaticAttribute("Redemption", "icon-users", "icon-user",
        RenderType = UIOMaticRenderType.List, IsCanExport = true, ReadOnly = false)]
    [TableName("Redemption")]
    [PrimaryKey("Uid", autoIncrement = false)]
    [ExplicitColumns]
    public class Redemption : IUIOMaticModel
    {
        public const string TableName = "Redemption";
        [UIOMaticIgnoreField]
        [UIOMaticField("Uid", "", IsCanEdit = false)]
        [Column]
        public Guid Uid { get; set; }
        [UIOMaticNameField]
        [Column]
        public string ProductID { get; set; }
        [UIOMaticField("Start Date", "Enter the Start Date", IsCanEdit = false)]
        [UIOMaticFilterField(DefaultValue = "monthlyfirstday", DefaultToValue = "monthlylastday")]
        [UIOMaticSortOrder(1)]
        [Column]
        public DateTime StartDateTime { get; set; }
        [UIOMaticField("End Date", "Enter the End Date", IsCanEdit = false,DateFormat ="yyyy MM dd")]
        [Column]
        public DateTime EndDateTime { get; set; }
        [UIOMaticField("Redemption Points", "", IsCanEdit = false)]
        [UIOMaticFilterField]
        [Column]
        public int RedemptionPoint { get; set; }
        [UIOMaticField("Quantity", "", IsCanEdit = false)]
        [Column]
        public int Quantity { get; set; }

        [UIOMaticField("CreatedDateTime", "", View = "datetime", IsCanEdit = false)]
        [UIOMaticIgnoreField]
        [Column]
        [UIOMaticSortOrder(2, true)]
        public DateTime CreatedDateTime { get; set; }
        //[UIOMaticField("CreatedBy", "", IsCanEdit = false)]
        [UIOMaticIgnoreField]
        [Column]
        public int CreatedBy { get; set; }
        [UIOMaticField("UpdatedDateTime", "", View = "datetime", IsCanEdit = false)]
        [UIOMaticIgnoreField]
        [Column]
        public DateTime UpdatedDateTime { get; set; }
        //[UIOMaticField("UpdatedBy", "", IsCanEdit = false)]
        [UIOMaticIgnoreField]
        [Column]
        public int UpdatedBy { get; set; }

        [Ignore]
        [UIOMaticIgnoreFromListView]
        [UIOMaticField("Winner", "", View = "list",
            Config = "{'typeName': 'Example.Model.RedemptionRecord, Example', 'foreignKeyColumn' : 'RedemptionID', 'canEdit' : true}")]
        public IEnumerable<RedemptionRecord> Winner { get; set; }

        public override string ToString()
        {
            return ProductID;
        }

        public IEnumerable<Exception> Validate()
        {
            var exs = new List<Exception>();

            if (string.IsNullOrEmpty(ProductID))
                exs.Add(new Exception("Please provide a value for Product Code"));

            if (Quantity < 0)
                exs.Add(new Exception("Please provide a value for Quantity"));

            return exs;
        }


        public void SetDefaultValue()
        {
            if (CreatedDateTime == default(DateTime))
            {
                CreatedDateTime = DateTime.Now;
                CreatedBy = 0;
            }
            UpdatedDateTime = DateTime.Now;
            UpdatedBy = 0;
        }
    }
}