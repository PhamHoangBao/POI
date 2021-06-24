using System;
using System.Collections.Generic;
using System.Text;

namespace POI.repository.ResultEnums
{
    public enum UpdateEnum
    {
        Success = 1,
        Error = 2,
        NothingChanged = 3,
        ErrorInServer = 4,
        NotOwner = 5
    }
}
