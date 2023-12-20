using Entities.Models;

namespace Contracts
{
    public interface IPlaneRepository
    {
        IEnumerable<Plane> GetPlanes(Guid driverId, bool trackChanges);
        Plane GetPlaneById(Guid driverId, Guid planeId, bool trackChanges);
    }
}
