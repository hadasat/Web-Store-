using Shopping;
using Users;
using WorkshopProject.Policies;

namespace WorkshopProject.DataAccessLayer
{
    public interface IDataAccess
    {
        Discount GetDiscount(int id);
        Member GetMember(int id);
        bool GetMode();
        IBooleanExpression GetPolicy(int id);
        Product GetProduct(int id);
        ShoppingBasket GetShoppingBasket(int id);
        ShoppingCart GetShoppingCart(int id);
        Store GetStore(int id);
        bool RemoveDiscount(int id);
        bool RemoveMember(int id);
        bool RemovePolicy(int id);
        bool RemoveProduct(int id);
        bool RemoveShoppingBasket(int id);
        bool RemoveShoppingCart(int id);
        bool RemoveStore(int id);
        bool SaveDiscount(Discount entity);
        bool SaveMember(Member entity);
        bool SavePolicy(IBooleanExpression entity);
        bool SaveProduct(Product entity);
        bool SaveShoppingBasket(ShoppingBasket entity);
        bool SaveShoppingCart(ShoppingCart entity);
        bool SaveStore(Store entity);
        void SetMode(bool isProduction);
    }
}