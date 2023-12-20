using Entities.Models;
using Entities.RequestFeatures;

namespace Contracts
{
    public interface IPlaneRepository
    {
        Task<PagedList<Plane>> GetPlanesAsync(Guid driverId, PlaneParameters planeParameters, bool trackChanges);
        Task<Plane> GetPlaneByIdAsync(Guid driverId, Guid planeId, bool trackChanges);
        void CreatePlaneForPilot(Guid driverId, Plane plane);
        void DeletePlane(Plane plane);
    }
}
