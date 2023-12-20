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
    }
}
