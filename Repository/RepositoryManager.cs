using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;
        private IPlaneRepository _carRepository;
        private IDriverRepository _driverRepository;
        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public ICompanyRepository Company
        {
            get
            {
                if (_companyRepository == null)
                    _companyRepository = new CompanyRepository(_repositoryContext);
                return _companyRepository;
            }
        }
        public IEmployeeRepository Employee
        {
            get
            {
                if (_employeeRepository == null)
                    _employeeRepository = new EmployeeRepository(_repositoryContext);
                return _employeeRepository;
            }
        }
        public IPlaneRepository Plane
        {
            get
            {
                if (_carRepository == null)
                    _carRepository = new PlaneRepository(_repositoryContext);
                return _carRepository;
            }
        }
        public IDriverRepository Driver
        {
            get
            {
                if (_driverRepository == null)
                    _driverRepository = new DriverRepository(_repositoryContext);
                return _driverRepository;
            }
        }
        public void Save() => _repositoryContext.SaveChanges();
    }
}