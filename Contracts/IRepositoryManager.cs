namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        IPlaneRepository Plane { get; }
        IPilotRepository Pilot { get; }
        public Task SaveAsync();
    }
}
