using BokningsModels;
using Bokningssystem_Labb4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bokningssystem_Labb4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICustomer<Company> _company;
        public CompanyController(ICustomer<Company> company)
        {
            _company = company;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            try
            {
                return Ok(await _company.GetAll());
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
                var result = await _company.GetSingle(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound($"Company with ID: {id} not found");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error to find data in DB");
            }

        }
    }
}
