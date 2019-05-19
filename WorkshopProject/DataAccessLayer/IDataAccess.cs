using Users;

namespace WorkshopProject.DataAccessLayer
{
    public interface IDataAccess
    {
        Member GetMember(int id);
        bool GetMode();
        Product GetProduct(int id);
        Store GetStore(int id);
        bool SaveMember(Member entity);
        bool SaveProduct(Product entity);
        bool SaveStore(Store entity);
        void SetMode(bool isProduction);
    }
}