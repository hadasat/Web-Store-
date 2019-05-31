using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shopping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TansactionsNameSpace;
using Users;
using WorkshopProject;
using WorkshopProject.External_Services;
using WorkshopProject.System_Service;


namespace IntegrationTests
{

    //Message Format: {message: String}
    //Search Format: {List<Product> products}

    [TestClass()]
    public class TransactionServiceTest
    {
        int credit = 0, csv = 0;
        string expirydate = "", shippingAddress = "";
        public string successMsg = "success";
        SystemServiceImpl menager = new SystemServiceImpl();
        SystemServiceImpl user = new SystemServiceImpl();
        int productAmount = 20, amountToBuy = 5;
        int[] storeId = new int[4], productId = new int[4];
        Store[] store = new Store[4];
        Product[] product = new Product[4];
        ShoppingBasket userShoppingBasket;

        private void newStore(int num)
        {
            string storeIdStr = menager.AddStore("store" + num);
            storeId[num] = JsonConvert.DeserializeObject<IdMessage>(storeIdStr).id;
            store[num] = WorkShop.getStore(storeId[num]);
        }

        private void newProduct(int pIndex, int sIndex)
        {
            string productIdstr = menager.AddProductToStore(storeId[sIndex], "product" + sIndex, "p", 12.3, "none");
            productId[pIndex] = JsonConvert.DeserializeObject<IdMessage>(productIdstr).id;
            product[pIndex] = store[sIndex].findProduct(productId[pIndex]);

        }

        [TestInitialize]
        public void Init()
        {
            user.Register("user-hadas", "user-atiya", DateTime.Now.AddYears(-25), "shit");
            user.login("user-hadas", "user-atiya");
            userShoppingBasket = user.user.shoppingBasket;
            menager.Register("hadas", "atiya", DateTime.Now.AddYears(-25), "shit");
            menager.login("hadas", "atiya");
            
            for(int i=0; i < store.Length; i++)
            {
                newStore(i);
            }
            for(int i = 0; i< product.Length; i++) {
                newProduct(i, i%2);
            }
            
            //adding products to stores
            menager.AddProductToStock(storeId[0], productId[0], productAmount);
            menager.AddProductToStock(storeId[0], productId[2], productAmount);
            menager.AddProductToStock(storeId[1], productId[1], productAmount);
            menager.AddProductToStock(storeId[1], productId[3], productAmount);
        }

        [TestCleanup]
        public void Cealup()
        {   //remove p0
            menager.RemoveProductFromStore(storeId[0], productId[0]);
            menager.RemoveProductFromStore(storeId[0], productId[2]);
            menager.closeStore(storeId[0]);
            //remove p1
            menager.RemoveProductFromStore(storeId[1], productId[1]);
            menager.RemoveProductFromStore(storeId[1], productId[3]);
            menager.closeStore(storeId[1]);
        }

        private void addingProductToBasket(int amount,int index)
        {
            int storeIndex = index % 2;
            string msg = user.AddProductToBasket(storeId[storeIndex], productId[index], amount);
            string returnMsg = JsonConvert.DeserializeObject<Message>(msg).message;
            Assert.AreEqual(successMsg.ToLower(), returnMsg.ToLower(), "product "+index + " to store " + storeIndex);
        }

        [TestMethod()]
        [TestCategory("TransactionServiceTest")]
        public void AddProductToBasketTest()
        {
            Init();
            //adding product to user basket
            addingProductToBasket(amountToBuy,0);
            Assert.AreEqual(amountToBuy, userShoppingBasket.getProductAmount(product[0]));
            addingProductToBasket(amountToBuy,0);
            Assert.AreEqual(2*amountToBuy, userShoppingBasket.getProductAmount(product[0]));
            addingProductToBasket(amountToBuy, 2);
            Assert.AreEqual(amountToBuy, userShoppingBasket.getProductAmount(product[2]));
            addingProductToBasket(amountToBuy, 1);
            Assert.AreEqual(amountToBuy, userShoppingBasket.getProductAmount(product[1]));

            Transaction.updateUser(user.user);

            userShoppingBasket.cleanBasket();
        }

        [TestMethod()]
        [TestCategory("TransactionServiceTest")]
        [TestCategory("Regression")]
        public void BuyShoppingBasketTest()
        {
            //PaymentStub.setRet(true);
            //SupplyStub.setRet(true);
            IPayment payStub = new PaymentStub(true);
            ISupply supplyStub = new SupplyStub(true);

            ConsistencyStub.setRet(true);

            //init
            addingProductToBasket(amountToBuy, 0);
            addingProductToBasket(amountToBuy, 0);
            addingProductToBasket(amountToBuy, 1);
            //PaymentStub.setRet(true);
            //SupplyStub.setRet(true);
            ConsistencyStub.setRet(true);

            int cardNumber = 0, ccv = 0, month = 10, year = 2050, id = 123456789;
            string holder = "mosh moshe", city = "shit", country = "shit", zip = "12345", address = "";
            Transaction transaction = new Transaction(user.user, cardNumber,month,year,holder,ccv,id,holder,address,city,country,zip,payStub,supplyStub);
            int transId = transaction.id;
            Assert.IsTrue(transId > 0,"1");
            //chack the basket is empty
            Assert.IsTrue(userShoppingBasket.isEmpty(),"2");
            //check the product amount has reduced
            Assert.AreEqual(productAmount - (2 * amountToBuy), product[0].amount);
            Assert.AreEqual(productAmount - amountToBuy, product[1].amount);
            //try to purches bad amount
        }

        [TestMethod()]
        [TestCategory("TransactionServiceTest")]
        [TestCategory("RegrationTest")]
        public void GetShoppingCartTest()
        {
            //init
            addingProductToBasket(amountToBuy, 2);
            addingProductToBasket(amountToBuy, 2);
            addingProductToBasket(amountToBuy, 3);

            string cart = user.GetShoppingCart(storeId[0]);
            ShoppingCart shopping = JsonConvert.DeserializeObject<ShoppingCart>(cart);

            ShoppingCart actualShopping = userShoppingBasket.getCart(store[0]);
            int recivedNum = shopping.getProducts().Count();
            int actualNum = actualShopping.getProducts().Count;
            Assert.AreEqual(actualNum, recivedNum);

            recivedNum = shopping.getTotalAmount();
            actualNum = actualShopping.getTotalAmount();
            Assert.AreEqual(actualNum, recivedNum);

            recivedNum = shopping.getProductAmount(product[2]);
            actualNum = actualShopping.getProductAmount(product[2]);
            Assert.AreEqual(actualNum, recivedNum);

            recivedNum = shopping.getProductAmount(product[3]);
            actualNum = actualShopping.getProductAmount(product[3]);
            Assert.AreEqual(actualNum, recivedNum);

            recivedNum = shopping.getProductAmount(product[1]);
            actualNum = actualShopping.getProductAmount(product[1]);
            Assert.AreEqual(actualNum, recivedNum);

            userShoppingBasket.cleanBasket();
        }

        [TestMethod()]
        [TestCategory("TransactionServiceTest")]
        [TestCategory("RegretionTest")]
        public void GetShoppingBasketTest()
        {
            //init
            addingProductToBasket(amountToBuy, 2);
            addingProductToBasket(amountToBuy, 2);
            addingProductToBasket(amountToBuy, 3);
            addingProductToBasket(amountToBuy, 1);

            string basket = user.GetShoppingBasket();
            ShoppingBasket shopping = JsonConvert.DeserializeObject<ShoppingBasket>(basket);
            
            //check id
            int recivedNum = shopping.id;
            int actualNum = userShoppingBasket.id;
        //    Assert.AreEqual(actualNum, recivedNum);
            //check each product
            recivedNum = shopping.getProductAmount(product[2]);
            actualNum = userShoppingBasket.getProductAmount(product[2]);
            Assert.AreEqual(actualNum, recivedNum,"2");

            recivedNum = shopping.getProductAmount(product[3]);
            actualNum = userShoppingBasket.getProductAmount(product[3]);
            Assert.AreEqual(actualNum, recivedNum,"3");

            recivedNum = shopping.getProductAmount(product[1]);
            actualNum = userShoppingBasket.getProductAmount(product[1]);
            Assert.AreEqual(actualNum, recivedNum,"4");
            //check total amount
            recivedNum = shopping.getCarts().Count;
            actualNum = userShoppingBasket.getCarts().Count;
            Assert.AreEqual(actualNum, recivedNum,"5");

        }

        [TestMethod()]
        [TestCategory("TransactionServiceTest")]

        public void SetProductAmountInBaketTest()
        {
            //init
            addingProductToBasket(amountToBuy, 0);
            addingProductToBasket(amountToBuy, 0);
            addingProductToBasket(amountToBuy, 1);


            int newAmount = 3;
            int pAmount = userShoppingBasket.getProductAmount(product[0]);
            Assert.AreNotEqual(newAmount, pAmount);
            //set p0 to 3
            user.SetProductAmountInBasket(storeId[0], productId[0], newAmount);
            pAmount = userShoppingBasket.getProductAmount(product[0]);
            Assert.AreEqual(newAmount, pAmount);

            //check product[1] change
            pAmount = userShoppingBasket.getProductAmount(product[1]);
            Assert.AreNotEqual(newAmount, pAmount,"2");
            user.SetProductAmountInBasket(storeId[1], productId[1], newAmount);
            pAmount = userShoppingBasket.getProductAmount(product[1]);
            Assert.AreEqual(newAmount, pAmount);

            //set p0 to 0
            user.SetProductAmountInBasket(storeId[0], productId[0], 0);
            pAmount = userShoppingBasket.getProductAmount(product[0]);
            Assert.AreEqual(0, pAmount,"3");
        }
    }
}
