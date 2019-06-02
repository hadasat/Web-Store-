using Shopping;
using System.Data.Entity.Infrastructure;
using Users;
using WorkshopProject.DataAccessLayer.Context;
using WorkshopProject.Policies;

namespace WorkshopProject.DataAccessLayer
{
    /// <exception cref="DbUpdateException"></exception>
    /// <exception cref="DbUpdateConcurrencyException">Thrown when conflicting updates occur</exception>
    /// <exception cref="DbEntityValidationException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="ObjectDisposedException">Calling a disposed context</exception>
    /// <exception cref="InvalidOperationException"></exception>
    public interface IDataAccess1
    {
        /// <summary>
        /// Gets the mode of the DataAccess.
        /// True: Production.
        /// False: Test.
        /// </summary>
        /// <param name="isProduction"></param>
        bool GetMode();
        void SetMode(bool isProduction);
        WorkshopDBContext getContext();
        DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters) where T : class;
        T GetEntity<T>(int key) where T : class; 
        bool SaveEntity<T>(T entity, int key) where T : class;
        bool RemoveEntity<T>(int key) where T : class;
        bool Delete();
    }
}





        /*
        public interface IDataAccess
        {
            DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters);
            Discount GetDiscount(int id);
            Member GetMember(int id);

            /// <summary>
            /// Gets the mode of the DataAccess.
            /// True: Production.
            /// False: Test.
            /// </summary>
            /// <param name="isProduction"></param>
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

            /// <summary>
            /// Sets the mode of the DataAccess.
            /// True: Production.
            /// False: Test.
            /// </summary>
            /// <param name="isProduction"></param>
            void SetMode(bool isProduction);
        }
        */
  