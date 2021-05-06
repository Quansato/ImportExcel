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
    public class CustomerGroup:BaseEntity
    {
        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid CustomerGroupId { get; set; }
        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        public string CustomerGroupName { get; set; } 
    }
}
