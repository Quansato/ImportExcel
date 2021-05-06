using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Application.Entities
{
    public class CustomerGroup:BaseEntity
    {
        public Guid CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; } 
    }
}
