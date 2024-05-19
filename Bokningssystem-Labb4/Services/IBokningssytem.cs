using BokningsModels;

namespace Bokningssystem_Labb4.Services
{
    public interface IBokningssytem<T>
    {
        Task<T> Add(T newEntity);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetSpecific(int id);
        Task<T> Update(T entity);
        Task<T> Delete(int id);

        Task<IEnumerable<Customer>> GetCustomersWithBookingsInCurrentWeek();
        Task <int> GetNumberOfAppointmentsForCustomerInWeek(int customerId, int weekNumber);
        
        Task<IEnumerable<Appointment>> SaveHistoryAsync(int appointmentId, int userId);

        Task<IEnumerable<AppHistory>> GetAppointmentHistoryAsync();

    }
}
