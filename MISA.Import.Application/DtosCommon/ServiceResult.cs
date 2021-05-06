using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Application.DtosCommon
{
    public class ServiceResult<MISAEntity>
    {
        #region Declare
        /// <summary>
        /// Mã trả về
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Thông báo trả về
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Số dòng lỗi
        /// </summary>
        public int CountLineError { get; set; }

        /// <summary>
        /// Dữ liệu
        /// </summary>
        public MISAEntity Data { get; set; }
        #endregion

        #region Method
        public static ServiceResult<MISAEntity> GetResult(int code, string msg, int count = 0, MISAEntity data = default(MISAEntity))
        {
            return new ServiceResult<MISAEntity>
            {
                Code = code,
                Msg = msg,
                CountLineError = count,
                Data = data,
            };
        }
        #endregion
    }
}
