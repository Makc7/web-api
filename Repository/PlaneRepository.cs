using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;

namespace Repository
{
    public class PlaneRepository: RepositoryBase<Plane>, IPlaneRepository
    {
        public PlaneRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public async Task<PagedList<Plane>> GetPlanesAsync(Guid pilotId, PlaneParameters planeParameters, bool trackChanges)
        {
            var planes = await FindByCondition(c => c.PilotId.Equals(pilotId), trackChanges).Search(planeParameters.SearchTerm).Sort(planeParameters.OrderBy).ToListAsync();
            return PagedList<Plane>.ToPagedList(planes, planeParameters.PageNumber, planeParameters.PageSize);
        }
        public async Task<Plane> GetPlaneByIdAsync(Guid pilotId, Guid id, bool trackChanges) => await FindByCondition(c => c.PilotId.Equals(pilotId) &&
            c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreatePlaneForPilot(Guid pilotId, Plane plane)
        {
            plane.PilotId = pilotId;
            Create(plane);
        }
        public void DeletePlane(Plane plane) => Delete(plane);
    }
}
