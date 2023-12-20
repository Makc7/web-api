using System;
using Entities.Models;

namespace Contracts
{
    public interface IPilotRepository
    {
        Task<IEnumerable<Pilot>> GetAllPilotsAsync(bool trackChanges);
        public Task<Pilot> GetPilotAsync(Guid id, bool trackChanges);
        void CreatePilot(Pilot driver);       
        Task<IEnumerable<Pilot>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeletePilot(Pilot driver);
    }
}
