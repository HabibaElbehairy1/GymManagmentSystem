using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Common
{
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFailed, 
    }
}
