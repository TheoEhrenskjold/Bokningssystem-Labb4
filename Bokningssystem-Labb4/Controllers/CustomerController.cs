using BokningsModels;
using Bokningssystem_Labb4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bokningssystem_Labb4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomer<Customer> _customer;
        public CustomerController(ICustomer<Customer> customer)
        {
            _customer = customer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            try
            {
                return Ok(await _customer.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error to find data in DB");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                var result = await _customer.GetSingle(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound($"Customer with ID: {id} not found");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error to find data in DB");
            }

        }

    }
}
