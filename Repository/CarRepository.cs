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
    }
}
