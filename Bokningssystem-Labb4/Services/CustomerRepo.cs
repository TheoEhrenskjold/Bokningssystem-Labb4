using BokningsModels;
using Bokningssystem_Labb4.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Bokningssystem_Labb4.Services
{
    public class CustomerRepo : ICustomer<Customer>
    {
        private AppDBContext _appContext;
        public CustomerRepo(AppDBContext appContext)
        {
            _appContext = appContext;
        }
        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _appContext.Customers.ToListAsync();
        }

        public async Task<Customer> GetSingle(int id)
        {
            return await _appContext.Customers.FirstOrDefaultAsync(p => p.CustomerId == id);
        }

        
    }
}
