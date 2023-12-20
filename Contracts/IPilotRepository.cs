using Entities.Models;

namespace Contracts
{
    public interface IPilotRepository
    {
        IEnumerable<Pilot> GetAllPilots(bool trackChanges);
        public Pilot GetPilot(Guid id, bool trackChanges);
    }
}
