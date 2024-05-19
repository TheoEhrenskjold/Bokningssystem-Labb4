using BokningsModels;
using Bokningssystem_Labb4.Data;
using Bokningssystem_Labb4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace Bokningssystem_Labb4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IBokningssytem<Appointment> _appointmentRepo;

        public AppointmentController(IBokningssytem<Appointment> appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }



        [HttpGet("customers/week")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersWithBookingsInCurrentWeek()
        {
            try
            {
                var customersWithBookings = await _appointmentRepo.GetCustomersWithBookingsInCurrentWeek();
                return Ok(customersWithBookings);
            }
            catch (Exception ex)
            {
                // Logga felmeddelandet eller hantera felet på lämpligt sätt
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetAllAppointmentHistory()
        {
            try
            {
                var history = await _appointmentRepo.GetAppointmentHistoryAsync();
                if (history == null || !history.Any())
                {
                    return NotFound("No history found.");
                }

                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{customerId}/week/{weekNumber}")]
        public async Task<ActionResult<int>> GetNumberOfAppointmentsForCustomerInWeek(int customerId, int weekNumber)
        {
            try
            {
                var numberOfAppointments = await _appointmentRepo.GetNumberOfAppointmentsForCustomerInWeek(customerId, weekNumber);
                return Ok(numberOfAppointments);
            }
            catch (Exception ex)
            {
                // Logga felmeddelandet eller hantera felet på lämpligt sätt
                return StatusCode(500, "Internal server error");
            }
        }



        

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Appointment>> UpdateAppointment(int id, Appointment appointment)
        {
            try
            {
                if (id != appointment.AppointmentID)
                {
                    return BadRequest("Appointment ID mismatch.");
                }

                var existingAppointment = await _appointmentRepo.GetSpecific(id);
                if (existingAppointment == null)
                {
                    return NotFound($"Appointment with ID = {id} not found.");
                }

                var updatedAppointment = await _appointmentRepo.Update(appointment);

                // Check if the update was successful before attempting to save history
                if (updatedAppointment == null)
                {
                    return NotFound($"Failed to update appointment with ID = {id}.");
                }

                // Assuming you have the current user ID available somehow, replace `currentUserId` with the actual user ID
                await _appointmentRepo.SaveHistoryAsync(updatedAppointment.AppointmentID, updatedAppointment.CustomerID);

                return Ok(updatedAppointment);
            }
            catch (JsonException jsonEx)
            {
                return BadRequest($"JSON Error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating data in DB: {ex.Message}");
            }
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)
        {
            try
            {
                var AppToDel = await _appointmentRepo.GetSpecific(id);
                if (AppToDel == null)
                {
                    return NotFound();
                }
                return await _appointmentRepo.Delete(id);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error to delete data in DB");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] AppointmentDto appointmentDto)
        {
            try
            {
                if (appointmentDto == null)
                {
                    return BadRequest();
                }

                var appointment = new Appointment
                {
                    StartTime = appointmentDto.StartTime,
                    EndTime = appointmentDto.EndTime,
                    IsCancelled = appointmentDto.IsCancelled,
                    CustomerID = appointmentDto.CustomerID,
                    CompanyID = appointmentDto.CompanyID
                };

                var result = await _appointmentRepo.Add(appointment);
                return CreatedAtAction(nameof(GetSpecific), new { id = result.AppointmentID }, result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error to create data in DB");
            }
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<Appointment>> GetSpecific(int id)
        {
            try
            {
                var result = await _appointmentRepo.GetSpecific(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error to find data in DB");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                return Ok(await _appointmentRepo.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error to find data in DB");
            }
        }

    }
}
