﻿using Entities.Models;

namespace Contracts
{
    public interface IEmployeeRepository
    {
       public void Create(Employee employee);
    }
}