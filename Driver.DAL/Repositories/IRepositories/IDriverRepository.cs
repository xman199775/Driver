using Driver.DAL.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.DAL.Repositories.IRepositories
{
    public interface IDriverRepository
    {
        Task<List<DriverEnitity>> GetAllDriversAsync();
        Task<DriverEnitity?> GetDriverByIdAsync(Guid id);
        Task<DriverEnitity> CreateDriverAsync(DriverEnitity driver);
        Task<DriverEnitity?> UpdateDriverAsync(DriverEnitity driver);
        Task DeleteDriverAsync(Guid id);
        Task<List<DriverEnitity>> GetAlphabetizedDriversAsync();
        Task<string> GetAlphabetizedNameAsync(Guid id);
    }
}
