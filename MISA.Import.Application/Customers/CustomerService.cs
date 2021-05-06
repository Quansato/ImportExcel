using Dapper;
using Microsoft.AspNetCore.Http;
using MISA.Import.Application.DtosCommon;
using MISA.Import.Application.Entities;
using MySqlConnector;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
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
        #region Declare
        protected string _tableName = string.Empty;
        protected string _connectionString = "" +
            "Host=47.241.69.179;" +
            "Port=3306;" +
            "User Id= dev; " +
            "Password=12345678;" +
            "Database=MISACukCuk_MF796_NTQUAN;";
        protected IDbConnection _dbConnection;
        #endregion

        #region Constructor
        public CustomerService()
        {
            _dbConnection = new MySqlConnection(_connectionString);
        }
        #endregion

        #region Method
        public async Task<ServiceResult<List<Customer>>> ImportFromExcel(IFormFile formFile, CancellationToken cancellationToken)
        {
            //Kiểm tra tệp có dữ liệu hay không
            if (formFile == null || formFile.Length <= 0)
            {
                return ServiceResult<List<Customer>>.GetResult(-1, "Tệp hiện tại chưa có dữ liệu");
            }
            //Kiểm tra file có phỉa là .xlss hay không
            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<List<Customer>>.GetResult(-1, "Không hỗ trợ cho loại file này");
            }

            var list = new List<Customer>();
            var countErr = 0;
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);
                

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        //process datetime
                        var dob = FormatDateTime(worksheet.Cells[row, 6].Value == null ? null : worksheet.Cells[row, 6].Value.ToString());
                        DateTime? dt;
                        if (dob != null)
                        {
                            dt = DateTime.ParseExact(dob, new string[] { "yyyy-MM-dd", "yyyy-MM", "yyyy" }, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            dt = null;
                        };
                        var customerCode = worksheet.Cells[row, 1].Value.ToString();
                        var phoneNumber = worksheet.Cells[row, 5].Value.ToString();
                        var customerGroupName = worksheet.Cells[row, 4].Value.ToString();
                        var listErr = ValidateData(list, customerCode, phoneNumber, customerGroupName);
                        if (listErr != MISAConstant.isValid) countErr += 1;
                        list.Add(new Customer
                        {
                            CustomerCode = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            FullName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            MemberCardCode = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            CustomerGroupName = worksheet.Cells[row, 4].Value.ToString().Trim(),
                            PhoneNumber = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            DateOfBirth = dt,
                            CompanyName = worksheet.Cells[row, 7].Value.ToString().Trim(),
                            CompanyTaxCode = worksheet.Cells[row, 8].Value.ToString().Trim(),
                            Email = worksheet.Cells[row, 9].Value == null ? "" : worksheet.Cells[row, 9].Value.ToString().Trim(),
                            Address = worksheet.Cells[row, 10].Value.ToString().Trim(),
                            Description = worksheet.Cells[row, 11].Value.ToString().Trim(),
                            ListError = listErr
                        });
                    }
                }
            }

            return ServiceResult<List<Customer>>.GetResult(200, "OK",countErr, list);
        }

        /// <summary>
        /// Kiểm tra dữ liệu khi import từ excel
        /// </summary>
        /// <param name="list">Danh sách dữ liệu</param>
        /// <param name="customerCode">mã khách hàng</param>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <param name="customerGroupName">tên nhóm khách hàng</param>
        /// <returns>Thông báo lỗi nếu có</returns>
        /// CreatedBy:ntquan(06/05/2021)
        public string ValidateData(List<Customer> list, string customerCode, string phoneNumber, string customerGroupName)
        {
            var listErr = "";
            var isErr = 0;

            //process customerCode 
            var countCustomer = GetCustomerByCode(customerCode);
            if (countCustomer != null)
            {
                listErr += "Mã khách hàng " + MISAConstant.isExistInDb;
                isErr += 1;
            }
            // kiểm tra mã đã tồn tại trong file chưa
            if (list.Any(item => item.CustomerCode == customerCode))
            {
                listErr += "Mã khách hàng " + MISAConstant.isExistInFile;
                isErr += 1;
            }

            //process phoneNumber
            var countCustomerV2 = GetCustomerByPhoneNumber(phoneNumber);
            if (countCustomerV2 != null)
            {
                listErr += "Số điện thoại " + MISAConstant.isExistInDb;
                isErr += 1;
            }
            // kiểm tra số điện thoại đã tồn tại trong file chưa
            if (list.Any(item => item.PhoneNumber == phoneNumber))
            {
                listErr += "Số điện thoại" + MISAConstant.isExistInFile;
                isErr += 1;
            }

            //process customerGroupName
            var countCG = GetCustomerGroupByName(customerGroupName);
            if (countCustomerV2 == null)
            {
                listErr += "Nhóm khách hàng " + MISAConstant.isNotExistDb;
                isErr += 1;
            }

            if (isErr == 0) listErr = MISAConstant.isValid;
            return listErr;
        }

        /// <summary>
        /// Định dạng thời gian cho chuẩn form
        /// </summary>
        /// <param name="value">Thời gian</param>
        /// <returns>Thời gian đã được định dạng</returns>
        /// CreatedBy:ntquan(06/05/2021)
        public string FormatDateTime(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var dayList = value.Split("/");
            var result = String.Join("-", dayList.Reverse());
            return result;
        }

        public Task<int> Insert()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy khách hàng theo mã khách hàng
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>Thực thể khách hàng</returns>
        /// CreatedBy:ntquan(06/05/2021)
        public Customer GetCustomerByCode(string customerCode)
        {
            var storeName = $"Proc_GetCustomerByCode";
            DynamicParameters dynamicParameters = new DynamicParameters();
            var storeGetByIdInputParamName = $"@CustomerCode";
            dynamicParameters.Add(storeGetByIdInputParamName, customerCode);

            var entity = _dbConnection.QueryFirstOrDefault<Customer>(storeName, param: dynamicParameters, commandType: CommandType.StoredProcedure);
            return entity;
        }

        /// <summary>
        /// Lấy khách hàng theo số điện thoại
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <returns>Thực thể khách hàng</returns>
        /// CreatedBy:ntquan(06/05/2021)
        public Customer GetCustomerByPhoneNumber(string phoneNumber)
        {
            var storeName = $"Proc_GetCustomerByPhoneNumber";
            DynamicParameters dynamicParameters = new DynamicParameters();
            var storeGetByIdInputParamName = $"@PhoneNumber";
            dynamicParameters.Add(storeGetByIdInputParamName, phoneNumber);

            var entity = _dbConnection.QueryFirstOrDefault<Customer>(storeName, param: dynamicParameters, commandType: CommandType.StoredProcedure);
            return entity;
        }

        /// <summary>
        /// Lấy nhóm khách hàng theo tên
        /// </summary>
        /// <param name="cgName">Tên nhóm khách hàng</param>
        /// <returns>Thực thể nhóm khách hàng</returns>
        /// CreatedBy:ntquan(06/05/2021)
        public CustomerGroup GetCustomerGroupByName(string cgName)
        {
            var storeName = $"Proc_GetCustomerGroupByName";
            DynamicParameters dynamicParameters = new DynamicParameters();
            var storeGetByIdInputParamName = $"@CustomerGroupName";
            dynamicParameters.Add(storeGetByIdInputParamName, cgName);

            var entity = _dbConnection.QueryFirstOrDefault<CustomerGroup>(storeName, param: dynamicParameters, commandType: CommandType.StoredProcedure);
            return entity;
        }

        #endregion
    }
}
