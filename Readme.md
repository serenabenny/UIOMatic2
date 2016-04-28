# UI-O-Matic 2 #

[![Build status](https://ci.appveyor.com/api/projects/status/94932v6vx6mp2g57?svg=true)](https://ci.appveyor.com/project/TimGeyssens/uiomatic)
[![Documentation Status](https://readthedocs.org/projects/uiomatic/badge/?version=latest)](http://uiomatic.readthedocs.org/en/latest/)
[![NuGet release](https://img.shields.io/nuget/v/Nibble.Umbraco.UIOMatic.svg)](https://www.nuget.org/packages/Nibble.Umbraco.UIOMatic)
[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/developer-tools/ui-o-matic/)
[![Chat on Gitter](https://img.shields.io/badge/gitter-join_chat-green.svg)](https://gitter.im/TimGeyssens/UIOMatic)

**Auto generate an integrated crud UI in Umbraco for a db table based on a [petapoco ](http://www.toptensoftware.com/petapoco/)poco**

![](logo.png)

Implement an interface and decorate your class and properties with some additional attributes.
## What's new in UI-O-Matic 2 ##
1. Format column header to match UIOMaticField name.
2. Added export to CSV file.
3. Readonly for listview.
4. Hide it in the left side menu. 
5. Added Query featrue.
6. Default ordering with descing or ascing.
7. Datetime format in the listview using UIOMaticField attribute.

## Example ##
If you have the following db table

    CREATE TABLE [Redemption] (
      [Uid] uniqueidentifier DEFAULT (newid()) NOT NULL
    , [ProductID] nvarchar(100) NULL
    , [StartDateTime] datetime NOT NULL
    , [EndDateTime] datetime NOT NULL
    , [RedemptionPoint] int NOT NULL
    , [Quantity] int NOT NULL
    , [CreatedDateTime] datetime NOT NULL
    , [CreatedBy] int NOT NULL
    , [UpdatedDateTime] datetime NOT NULL
    , [UpdatedBy] int NOT NULL
    , [Status] nvarchar(50) NULL
    );
    GO
    ALTER TABLE [Redemption] ADD CONSTRAINT [PK_Redemption] PRIMARY KEY ([Uid]);
    GO
    CREATE TABLE [RedemptionRecord] (
      [Uid] uniqueidentifier DEFAULT (newid()) NOT NULL
    , [RedemptionID] uniqueidentifier NOT NULL
    , [ConfirmedDateTime] datetime NULL
    , [CollectedDateTime] datetime NULL
    , [CreatedDateTime] datetime DEFAULT (getdate()) NOT NULL
    , [CreatedBy] int NOT NULL
    , [UpdatedDateTime] datetime DEFAULT (getdate()) NOT NULL
    , [UpdatedBy] int NOT NULL
    , [Status] nvarchar(50) NOT NULL
    , [CollectionExpiryDateTime] datetime NULL
    );
    GO
    ALTER TABLE [RedemptionRecord] ADD CONSTRAINT [PK_RedemptionRecord] PRIMARY KEY ([Uid]);
    GO


This class

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
     }   `

Will generate the following UI

![](example.png)

## Documentation ##

For docs please go to[ http://uiomatic.readthedocs.org/](http://uiomatic.readthedocs.org/)

## Presentation ##
For the quick intro check [http://slides.com/timgeyssens/uiomatic#/](http://slides.com/timgeyssens/uiomatic#/)

### Test site ###
Backoffice credentials: 
- tim@nibble.be / password


