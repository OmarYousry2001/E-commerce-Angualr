using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.GenericResponse
{
    public enum ResponseType
    {
        Success,
        Created,
        Accepted,      
        NotFound,
        Unauthorized,
        BadRequest,
        Unprocessable
    }


}
