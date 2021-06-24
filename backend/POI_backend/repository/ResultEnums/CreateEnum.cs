using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ResultEnums
{
    public enum CreateEnum
    {
        Duplicate = 0,
        Success = 1,
        ErrorInServer = 2,
        Error = 3,
        NotOwner = 4
    }
}
