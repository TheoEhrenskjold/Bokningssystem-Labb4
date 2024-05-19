using BokningsModels;
using Bokningssystem_Labb4.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bokningssystem_Labb4.Services
{
    public class CompanyRepo : ICustomer<Company>
    {
        private AppDBContext _appContext;
        public CompanyRepo(AppDBContext appContext)
        {
            _appContext = appContext;
        }
        public async Task<IEnumerable<Company>> GetAll()
        {
            return await _appContext.Companys.ToListAsync();
        }

        public async Task<Company> GetSingle(int id)
        {
            return await _appContext.Companys.FirstOrDefaultAsync(p => p.CompanyID == id);
        }
    }
}
