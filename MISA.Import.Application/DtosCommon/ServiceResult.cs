using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Application.DtosCommon
{
    public class ServiceResult<MISAEntity>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public MISAEntity Data { get; set; }

        public static ServiceResult<MISAEntity> GetResult(int code, string msg, MISAEntity data = default(MISAEntity))
        {
            return new ServiceResult<MISAEntity>
            {
                Code = code,
                Msg = msg,
                Data = data
            };
        }
    }
}
