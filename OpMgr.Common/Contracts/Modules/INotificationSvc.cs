using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;

namespace OpMgr.Common.Contracts.Modules
{
    public interface INotificationSvc : ICRUDSvc<NotificationDTO, NotificationDTO>
    {
        StatusDTO<NotificationDTO> Insert(NotificationDTO data);

        StatusDTO<NotificationDTO> Update(NotificationDTO data);

        StatusDTO<NotificationDTO> Delete(NotificationDTO data);

        StatusDTO<List<NotificationDTO>> Select(NotificationDTO data);

        StatusDTO<NotificationDTO> Select(int rowId);
    }
}
