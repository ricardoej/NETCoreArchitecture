using repository.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace service.core
{
    public abstract class BaseService: IService
    {
        protected readonly IUnitOfWork unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
