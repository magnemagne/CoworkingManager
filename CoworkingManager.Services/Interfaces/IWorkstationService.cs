using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoworkingManager.Models;

namespace CoworkingManager.Services.Interfaces
{
    public interface IWorkstationService
    {
        Task<IEnumerable<Workstation>> GetWorkstationsAsync();
        Task<Workstation?> GetWorkstationByIdAsync(int id);
        Task<InsertResult<Workstation>> CreateWorkstationAsync(Workstation workstation);
        Task<bool> IsWorkstationAvailableAsync(int workstationId, DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<bool> UpdateWorkstationAsync(Workstation workstation);
        Task<bool> DeleteWorkstationAsync(int id);
    }
}