using Driver.DAL.Enitities;
using Driver.DAL.Repositories;
using Driver.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace Driver.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DriverController : ControllerBase
    {

        private readonly IDriverRepository _driverRepository;
        private readonly ILogger<DriverController> _logger;

        public DriverController(ILogger<DriverController> logger, IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
            _logger = logger;
        }

        [HttpGet("/GetAllDrivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            return Ok(await this._driverRepository.GetAllDriversAsync());

        }
        
        [HttpGet("/GetDriverById/{id}")]
        public async Task<IActionResult> GetDriverById(Guid id)
        {
            return Ok(await this._driverRepository.GetDriverByIdAsync(id));

        }

        [HttpPost("/CreateDriver")]
        public async Task<IActionResult> CreateDriver(DriverEnitity driver)
        {
            return Ok(await this._driverRepository.CreateDriverAsync(driver));
        }

        [HttpPut("/UpdateDriver")]
        public async Task<IActionResult> UpdateDriver(DriverEnitity driver)
        {
            return Ok(await this._driverRepository.UpdateDriverAsync(driver));
        }

        [HttpDelete("/UpdateDriver/{id}")]
        public async Task<IActionResult> DeleteDriverAsync(Guid id)
        {
            await this._driverRepository.DeleteDriverAsync(id);
            return Ok("done");
        }

        [HttpGet("/GetAlphabetizedName/{id}")]
        public async Task<IActionResult> GetAlphabetizedName(Guid id)
        {
            return Ok(await this._driverRepository.GetAlphabetizedNameAsync(id));
        }

        [HttpGet("/GetAlphabetizedDrivers")]
        public async Task<IActionResult> GetAlphabetizedDrivers()
        {
            return Ok(await this._driverRepository.GetAlphabetizedDriversAsync());
        }

    }
}