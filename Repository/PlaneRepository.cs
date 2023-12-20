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

        public IEnumerable<Plane> GetPlanes(Guid driverId, bool trackChanges) => 
            FindByCondition(c => c.PilotId.Equals(driverId), trackChanges).OrderBy(e => e.Brend);
        public Plane GetPlaneById(Guid driverId, Guid id, bool trackChanges) => FindByCondition(c => c.PilotId.Equals(driverId) &&
            c.Id.Equals(id), trackChanges).SingleOrDefault();
        public void CreatePlaneForPilot(Guid driverId, Plane plane)
        {
            plane.PilotId = driverId;
            Create(plane);
        }
    }
}
