using Entities.Models;

namespace Contracts
{
    public interface IPlaneRepository
    {
        IEnumerable<Plane> GetPlanes(Guid pilotId, bool trackChanges);
        Plane GetPlaneById(Guid pilotId, Guid PlaneId, bool trackChanges);
    }
}
