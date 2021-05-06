using Microsoft.AspNetCore.Http;
using MISA.Import.Application.DtosCommon;
using MISA.Import.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.Application.Customers
{
    public interface ICustomerService
    {
        Task<int> Insert();
        Task<ServiceResult<List<Customer>>> ImportFromExcel(IFormFile formFile, CancellationToken cancellationToken);
    }
}
