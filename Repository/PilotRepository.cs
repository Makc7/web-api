using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class PilotRepository: RepositoryBase<Pilot>, IPilotRepository
    {
        public PilotRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)        
        {
        }

        public IEnumerable<Pilot> GetAllPilots(bool trackChanges) => FindAll(trackChanges).OrderBy(c => c.Name).ToList();
        public Pilot GetPilot(Guid id, bool trackChanges) => FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefault();
        public void CreatePilot(Pilot driver) => Create(driver);
        public IEnumerable<Pilot> GetByIds(IEnumerable<Guid> ids, bool trackChanges) => FindByCondition(x => ids.Contains(x.Id), trackChanges).ToList();
    }
}
