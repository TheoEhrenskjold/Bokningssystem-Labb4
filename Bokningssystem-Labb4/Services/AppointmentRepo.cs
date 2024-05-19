using BokningsModels;
using Bokningssystem_Labb4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Bokningssystem_Labb4.Services
{
    public class AppointmentRepo : IBokningssytem<Appointment>
    {
        private AppDBContext _appDbContext;

        

        public AppointmentRepo(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }
        public async Task<Appointment> Add(Appointment newEntity)
        {
            var result = await _appDbContext.AddAsync(newEntity);
            await _appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Appointment> Delete(int id)
        {
            var result = await _appDbContext.Appointments.
                FirstOrDefaultAsync(a => a.AppointmentID == id);
            if (result != null)
            {
                _appDbContext.Appointments.Remove(result);
                await _appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }


        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<Appointment>> GetAll()
        {
            return await _appDbContext.Appointments.ToListAsync();
        }

        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<Appointment>> GetSpecific(int id)
        {
            return await _appDbContext.Appointments.Where(a => a.AppointmentID == id).ToListAsync();
        }

        public async Task<Appointment> Update(Appointment entity)
        {
            var result = await _appDbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentID == entity.AppointmentID);
            if (result != null)
            {
                result.StartTime = entity.StartTime;
                result.EndTime = entity.EndTime;
                result.IsCancelled = entity.IsCancelled;
                
                await _appDbContext.SaveChangesAsync();
                //await SaveHistoryAsync(result.AppointmentID,result.CustomerID);
                return result;
            }
            return null;
        }

        [Authorize(Roles = "admin")]
        public async Task<IEnumerable<Customer>> GetCustomersWithBookingsInCurrentWeek()
        {
            DateTime today = DateTime.Today;
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime startOfWeek = today.AddDays(-1 * ((7 + (today.DayOfWeek - firstDayOfWeek)) % 7)).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7).Date;

            var customersWithBookings = await _appDbContext.Customers
                .Where(c => c.Appointments.Any(a => a.StartTime >= startOfWeek /*&& a.EndTime <= endOfWeek*/))
                .ToListAsync();

            return customersWithBookings;
        }

        [Authorize(Roles = "admin")]
        public async Task<int> GetNumberOfAppointmentsForCustomerInWeek(int customerId, int weekNumber)
        {
            // Hitta startdatum och slutdatum för den angivna veckan
            DateTime startDate = GetFirstDateOfWeek(DateTime.Today.Year, weekNumber);
            DateTime endDate = startDate.AddDays(7).Date;

            // Filtrera bokningar för den specifika kunden och den angivna veckan
            var numberOfAppointments = await _appDbContext.Appointments
                .Where(a => a.CustomerID == customerId &&
                            a.StartTime >= startDate &&
                            a.EndTime < endDate)
                .CountAsync();

            return numberOfAppointments;
        }



        private DateTime GetFirstDateOfWeek(int year, int weekNumber)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekNumber;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            DateTime firstDayOfWeek = firstMonday.AddDays(weekNum * 7);

            return firstDayOfWeek;
        }

        public async Task<IEnumerable<Appointment>> SaveHistoryAsync(int appointmentId, int userId)
        {
            var historyRecord = new AppHistory
            {
                AppointmentID = appointmentId,
                ChangeDate = DateTime.UtcNow,
                UserId = userId
            };

            _appDbContext.Apphistories.Add(historyRecord);
            await _appDbContext.SaveChangesAsync();

            return null;
        }

        public async Task<IEnumerable<AppHistory>> GetAppointmentHistoryAsync()
        {
            return await _appDbContext.Apphistories.ToListAsync();
        }

    }
}
