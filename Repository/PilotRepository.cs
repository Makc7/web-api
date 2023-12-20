using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PilotRepository: RepositoryBase<Pilot>, IPilotRepository
    {
        public PilotRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)        
        {
        }

        public async Task<IEnumerable<Pilot>> GetAllPilotsAsync(bool trackChanges) => await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
        public async Task<Pilot> GetPilotAsync(Guid id, bool trackChanges) => await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreatePilot(Pilot pilot) => Create(pilot);
        public async Task<IEnumerable<Pilot>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeletePilot(Pilot pilot) => Delete(pilot);
    }
}
