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
    /// <summary>
    /// Interface Customer
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Thêm mới vào database
        /// </summary>
        /// <returns></returns>
        /// CreatedBy:ntquan(06/05/2021)
        Task<int> Insert();

        /// <summary>
        /// Đọc dữ liệu từ file Excel
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// CreatedBy:ntquan(06/05/2021)
        Task<ServiceResult<List<Customer>>> ImportFromExcel(IFormFile formFile, CancellationToken cancellationToken);
    }
}
