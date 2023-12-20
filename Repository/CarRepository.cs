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

        public async Task<PagedList<Plane>> GetPlanesAsync(Guid pilotId, PlaneParameters carParameters, bool trackChanges)
        {
            var cars = await FindByCondition(c => c.PilorId.Equals(pilotId), trackChanges).Search(carParameters.SearchTerm).Sort(carParameters.OrderBy).ToListAsync();
            return PagedList<Plane>.ToPagedList(cars, carParameters.PageNumber, carParameters.PageSize);
        }
        public async Task<Plane> GetPlaneByIdAsync(Guid pilotId, Guid id, bool trackChanges) => await FindByCondition(c => c.PilorId.Equals(pilotId) &&
            c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        public void CreatePlaneForPilor(Guid pilotId, Plane car)
        {
            car.PilorId = pilotId;
            Create(car);
        }
        public void DeletePlane(Plane car) => Delete(car);
    }
}
