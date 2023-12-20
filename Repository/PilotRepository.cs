using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class PilorRepository: RepositoryBase<Pilor>, IPilorRepository
    {
        public PilorRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)        
        {
        }

        public async Task<IEnumerable<Pilor>> GetAllPilorsAsync(bool trackChanges) => await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();
        public async Task<Pilor> GetPilorAsync(Guid id, bool trackChanges) => await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreatePilor(Pilor pilot) => Create(pilot);
        public async Task<IEnumerable<Pilor>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) => await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        public void DeletePilor(Pilor pilot) => Delete(pilot);
    }
}
