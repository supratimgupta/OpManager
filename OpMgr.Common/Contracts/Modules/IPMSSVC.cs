﻿using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IPMSSvc : ICRUDSvc<EmployeeGoalLogDTO, EmployeeGoalLogDTO>
    {
        StatusDTO<EmployeeGoalLogDTO> getSelfRating(int Achievement);
    }
}
