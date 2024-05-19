namespace Bokningssystem_Labb4.Services
{
    public interface ICustomer<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetSingle(int id);
    }
}
