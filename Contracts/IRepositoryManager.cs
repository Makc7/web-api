namespace Contracts
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        ICarRepository Car { get; }
        IPilotRepository Pilot { get; }
        public Task SaveAsync();
    }
}
