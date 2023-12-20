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

        public async Task<PagedList<Plane>> GetPlanesAsync(Guid driverId, PlaneParameters planeParameters, bool trackChanges)
        {
            var planes = await FindByCondition(c => c.PilotId.Equals(driverId), trackChanges).Search(planeParameters.SearchTerm).Sort(planeParameters.OrderBy).ToListAsync();
            return PagedList<Plane>.ToPagedList(planes, planeParameters.PageNumber, planeParameters.PageSize);
        }
        public async Task<Plane> GetPlaneByIdAsync(Guid driverId, Guid id, bool trackChanges) => await FindByCondition(c => c.PilotId.Equals(driverId) &&
            c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreatePlaneForPilot(Guid driverId, Plane plane)
        {
            plane.PilotId = driverId;
            Create(plane);
        }
        public void DeletePlane(Plane plane) => Delete(plane);
    }
}
