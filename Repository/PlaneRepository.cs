using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class PlaneRepository: RepositoryBase<Plane>, IPlaneRepository
    {
        public PlaneRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public IEnumerable<Plane> GetCars(Guid pilotId, bool trackChanges) => 
            FindByCondition(c => c.pilotId.Equals(pilotId), trackChanges).OrderBy(e => e.Brend);
        public Plane GetCarById(Guid pilotId, Guid id, bool trackChanges) => FindByCondition(c => c.PilotId.Equals(pilotId) &&
            c.Id.Equals(id), trackChanges).SingleOrDefault();
    }
}
