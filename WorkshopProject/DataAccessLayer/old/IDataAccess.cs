using System.Data.Entity.Infrastructure;
using Users;
using WorkshopProject.DataAccessLayer;

namespace WorkshopProject.DataAccessLayer
{
    public interface IDataAccess
    {
        //bool CheckConnection();
        bool Delete();
        WorkshopDBContext getContext();
        Member GetMember(int key);
        bool GetMode();
        Store GetStore(int key);
        Product GetProduct(int key);
        bool RemoveEntity<T>(int key) where T : IEntity;
        bool SaveEntity<T>(T entity, int key) where T : IEntity;
        void SetMode(bool isProduction);
        DbRawSqlQuery<T> SqlQuery<T>(string sql, params object[] paramaters) where T : IEntity;
    }
}