namespace WorkshopProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        condition_id = c.Int(),
                        successor_id = c.Int(),
                        Store_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.IBooleanExpressions", t => t.condition_id)
                .ForeignKey("dbo.Discounts", t => t.successor_id)
                .ForeignKey("dbo.Stores", t => t.Store_id)
                .Index(t => t.condition_id)
                .Index(t => t.successor_id)
                .Index(t => t.Store_id);
            
            CreateTable(
                "dbo.IBooleanExpressions",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Store_id = c.Int(),
                        Store_id1 = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Stores", t => t.Store_id)
                .ForeignKey("dbo.Stores", t => t.Store_id1)
                .Index(t => t.Store_id)
                .Index(t => t.Store_id1);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        birthdate = c.DateTime(nullable: false),
                        country = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.StoreManagers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        father_id = c.Int(),
                        myRoles_id = c.Int(),
                        store_id = c.Int(),
                        StoreManager_id = c.Int(),
                        StoreManager_id1 = c.Int(),
                        Member_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.StoreManagers", t => t.father_id)
                .ForeignKey("dbo.Roles", t => t.myRoles_id)
                .ForeignKey("dbo.Stores", t => t.store_id)
                .ForeignKey("dbo.StoreManagers", t => t.StoreManager_id)
                .ForeignKey("dbo.StoreManagers", t => t.StoreManager_id1)
                .ForeignKey("dbo.Members", t => t.Member_id)
                .Index(t => t.father_id)
                .Index(t => t.myRoles_id)
                .Index(t => t.store_id)
                .Index(t => t.StoreManager_id)
                .Index(t => t.StoreManager_id1)
                .Index(t => t.Member_id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        AddRemoveProducts = c.Boolean(nullable: false),
                        AddRemovePurchasing = c.Boolean(nullable: false),
                        AddRemoveDiscountPolicy = c.Boolean(nullable: false),
                        AddRemoveStorePolicy = c.Boolean(nullable: false),
                        AddRemoveStoreManger = c.Boolean(nullable: false),
                        CloseStore = c.Boolean(nullable: false),
                        CustomerCommunication = c.Boolean(nullable: false),
                        AppointOwner = c.Boolean(nullable: false),
                        AppointManager = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        rank = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        price = c.Double(nullable: false),
                        category = c.String(),
                        rank = c.Int(nullable: false),
                        description = c.String(),
                        amount = c.Int(nullable: false),
                        storeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ShoppingBaskets",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreManagers", "Member_id", "dbo.Members");
            DropForeignKey("dbo.StoreManagers", "StoreManager_id1", "dbo.StoreManagers");
            DropForeignKey("dbo.StoreManagers", "StoreManager_id", "dbo.StoreManagers");
            DropForeignKey("dbo.StoreManagers", "store_id", "dbo.Stores");
            DropForeignKey("dbo.IBooleanExpressions", "Store_id1", "dbo.Stores");
            DropForeignKey("dbo.IBooleanExpressions", "Store_id", "dbo.Stores");
            DropForeignKey("dbo.Discounts", "Store_id", "dbo.Stores");
            DropForeignKey("dbo.StoreManagers", "myRoles_id", "dbo.Roles");
            DropForeignKey("dbo.StoreManagers", "father_id", "dbo.StoreManagers");
            DropForeignKey("dbo.Discounts", "successor_id", "dbo.Discounts");
            DropForeignKey("dbo.Discounts", "condition_id", "dbo.IBooleanExpressions");
            DropIndex("dbo.StoreManagers", new[] { "Member_id" });
            DropIndex("dbo.StoreManagers", new[] { "StoreManager_id1" });
            DropIndex("dbo.StoreManagers", new[] { "StoreManager_id" });
            DropIndex("dbo.StoreManagers", new[] { "store_id" });
            DropIndex("dbo.StoreManagers", new[] { "myRoles_id" });
            DropIndex("dbo.StoreManagers", new[] { "father_id" });
            DropIndex("dbo.IBooleanExpressions", new[] { "Store_id1" });
            DropIndex("dbo.IBooleanExpressions", new[] { "Store_id" });
            DropIndex("dbo.Discounts", new[] { "Store_id" });
            DropIndex("dbo.Discounts", new[] { "successor_id" });
            DropIndex("dbo.Discounts", new[] { "condition_id" });
            DropTable("dbo.ShoppingCarts");
            DropTable("dbo.ShoppingBaskets");
            DropTable("dbo.Products");
            DropTable("dbo.Stores");
            DropTable("dbo.Roles");
            DropTable("dbo.StoreManagers");
            DropTable("dbo.Members");
            DropTable("dbo.IBooleanExpressions");
            DropTable("dbo.Discounts");
        }
    }
}
