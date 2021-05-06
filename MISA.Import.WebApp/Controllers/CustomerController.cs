using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Import.Application.Customers;
using MISA.Import.Application.DtosCommon;
using MISA.Import.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost()]
        public async Task<ServiceResult<List<Customer>>> ImportExcel(IFormFile formFile, CancellationToken cancellationToken)
        {
            var result = await _customerService.ImportFromExcel(formFile, cancellationToken);
            return result;
        }

        [HttpPost("Customer")]
        public Task<int> Insert()
        {
            return null;
        }
    }
}
