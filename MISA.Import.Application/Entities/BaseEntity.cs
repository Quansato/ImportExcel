using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Application.Entities
{
    /// <summary>
    /// Bảng nhóm khách hàng
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Ngày sửa 
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// người sửa
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
