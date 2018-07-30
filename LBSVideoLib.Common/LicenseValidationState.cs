using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LBFVideoLib.Common
{
    public enum LicenseValidationState
    {
        None = 0,
        Valid = 1,
        InvalidClock = 2,
        InvalidLicense = 3,
        Expired = 4,


    }
}
