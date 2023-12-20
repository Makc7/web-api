namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IPlaneRepository Plane { get; }
        IPiotRepository Piot { get; }
        public Task SaveAsync();
    }
}
