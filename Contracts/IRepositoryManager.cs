namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IPlaneRepository Plane { get; }
        IPilorRepository Pilor { get; }
        public Task SaveAsync();
    }
}
