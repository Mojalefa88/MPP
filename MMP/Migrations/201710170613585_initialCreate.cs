namespace MMP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        email = c.String(nullable: false),
                        password = c.String(nullable: false),
                        confirmPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Advices",
                c => new
                    {
                        advideID = c.Int(nullable: false, identity: true),
                        advice = c.String(nullable: false),
                        adiveDesc = c.String(),
                        categoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.advideID);
            
            CreateTable(
                "dbo.PropertyCategories",
                c => new
                    {
                        categoryID = c.Int(nullable: false, identity: true),
                        category = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.categoryID);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        propertyID = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        Desc = c.String(nullable: false),
                        categoryID = c.Int(nullable: false),
                        price = c.Double(nullable: false),
                        location = c.String(nullable: false),
                        status = c.String(),
                        date = c.DateTime(nullable: false),
                        userID = c.Int(nullable: false),
                        image = c.Binary(),
                    })
                .PrimaryKey(t => t.propertyID);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        type = c.String(nullable: false),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PropertyCategoryAdvices",
                c => new
                    {
                        PropertyCategory_categoryID = c.Int(nullable: false),
                        Advice_advideID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PropertyCategory_categoryID, t.Advice_advideID })
                .ForeignKey("dbo.PropertyCategories", t => t.PropertyCategory_categoryID, cascadeDelete: true)
                .ForeignKey("dbo.Advices", t => t.Advice_advideID, cascadeDelete: true)
                .Index(t => t.PropertyCategory_categoryID)
                .Index(t => t.Advice_advideID);
            
            CreateTable(
                "dbo.PropertyPropertyCategories",
                c => new
                    {
                        Property_propertyID = c.Int(nullable: false),
                        PropertyCategory_categoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Property_propertyID, t.PropertyCategory_categoryID })
                .ForeignKey("dbo.Properties", t => t.Property_propertyID, cascadeDelete: true)
                .ForeignKey("dbo.PropertyCategories", t => t.PropertyCategory_categoryID, cascadeDelete: true)
                .Index(t => t.Property_propertyID)
                .Index(t => t.PropertyCategory_categoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PropertyPropertyCategories", "PropertyCategory_categoryID", "dbo.PropertyCategories");
            DropForeignKey("dbo.PropertyPropertyCategories", "Property_propertyID", "dbo.Properties");
            DropForeignKey("dbo.PropertyCategoryAdvices", "Advice_advideID", "dbo.Advices");
            DropForeignKey("dbo.PropertyCategoryAdvices", "PropertyCategory_categoryID", "dbo.PropertyCategories");
            DropIndex("dbo.PropertyPropertyCategories", new[] { "PropertyCategory_categoryID" });
            DropIndex("dbo.PropertyPropertyCategories", new[] { "Property_propertyID" });
            DropIndex("dbo.PropertyCategoryAdvices", new[] { "Advice_advideID" });
            DropIndex("dbo.PropertyCategoryAdvices", new[] { "PropertyCategory_categoryID" });
            DropTable("dbo.PropertyPropertyCategories");
            DropTable("dbo.PropertyCategoryAdvices");
            DropTable("dbo.Packages");
            DropTable("dbo.Properties");
            DropTable("dbo.PropertyCategories");
            DropTable("dbo.Advices");
            DropTable("dbo.Admins");
        }
    }
}
 