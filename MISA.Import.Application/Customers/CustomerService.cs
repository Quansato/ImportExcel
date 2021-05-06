using Microsoft.AspNetCore.Http;
using MISA.Import.Application.DtosCommon;
using MISA.Import.Application.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.Application.Customers
{
    public class CustomerService : ICustomerService
    {
        public async Task<ServiceResult<List<Customer>>> ImportFromExcel(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return ServiceResult<List<Customer>>.GetResult(-1, "Tệp hiện tại chưa có dữ liệu");
            }
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<List<Customer>>.GetResult(-1, "Không hỗ trợ cho loại file này");
            }

            var list = new List<Customer>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        //var sDate = worksheet.Cells[row, 6].Value.ToString();

                        //double date = double.Parse(sDate);

                        //var dateTime = DateTime.FromOADate(date).ToString("MMMM dd, yyyy");

                        list.Add(new Customer
                        {
                           CustomerCode = worksheet.Cells[row, 1].Value.ToString().Trim(),
                           FullName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                           MemberCardCode = worksheet.Cells[row, 3].Value.ToString().Trim(),
                           CustomerGroupName = worksheet.Cells[row, 4].Value.ToString().Trim(),
                           PhoneNumber = worksheet.Cells[row, 5].Value.ToString().Trim(),
                           //DateOfBirth = Convert.ToDateTime(worksheet.Cells[row, 6].Value.ToString()),                          
                           //DateOfBirth = DateTime.Parse(birthday),
                           CompanyName = worksheet.Cells[row, 7].Value.ToString().Trim(),
                           CompanyTaxCode = worksheet.Cells[row, 8].Value.ToString().Trim(),
                           Email = worksheet.Cells[row, 9].Value.ToString().Trim(),
                           Address = worksheet.Cells[row, 10].Value.ToString().Trim(),
                           Description = worksheet.Cells[row, 11].Value.ToString().Trim(),
                        });
                    }
                }
            }

            return ServiceResult<List<Customer>>.GetResult(200, "OK", list);
        }

        public Task<int> Insert()
        {
            throw new NotImplementedException();
        }
    }
}
