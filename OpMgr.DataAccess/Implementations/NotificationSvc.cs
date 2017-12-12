using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using MySql.Data.MySqlClient;
using System.Data;

namespace OpMgr.DataAccess.Implementations
{
    public class NotificationSvc : INotificationSvc
    {
        IConfigSvc _configSvc;
        ILogSvc _logger;

        public NotificationSvc(IConfigSvc configSvc, ILogSvc logger)
        {
            _configSvc = configSvc;
            _logger = logger;
        }

        public StatusDTO<NotificationDTO> Insert(NotificationDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO notificationreminder(UserMasterId, Notificationtext, Active, IsViewed, notificationActivefrom)" +
                                            "VALUES (@userMasterId, @notificationText, 1, 0, @activeFrom)";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                    command.Parameters.Add("@notificationText", MySqlDbType.String).Value = data.NotificationText;
                    command.Parameters.Add("@activeFrom", MySqlDbType.DateTime).Value = data.NotificationActiveFrom.Date;
                    // add createdby from session

                    StatusDTO<NotificationDTO> status = new StatusDTO<NotificationDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.FailureReason = "Notification Insertion Failed";
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<NotificationDTO> Update(NotificationDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE notificationreminder IsViewed=@isViewed WHERE notificationreminderId=@notId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@isViewed", MySqlDbType.Bit).Value = data.Viewed;
                    command.Parameters.Add("@notId", MySqlDbType.Int32).Value = data.NotificationReminderId;
                    // add createdby from session

                    StatusDTO<NotificationDTO> status = new StatusDTO<NotificationDTO>();

                    if (command.ExecuteNonQuery() > 0)
                    {
                        status.IsSuccess = true;
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.FailureReason = "Notification Update Failed";
                    }
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<NotificationDTO> Delete(NotificationDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<List<NotificationDTO>> Select(NotificationDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT notificationreminderId, Notificationtext from notificationreminder WHERE Active=1 AND IsViewed=0 AND notificationActivefrom>=CURDATE() AND UserMasterId=@userMasterId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@userMasterId", MySqlDbType.Int32).Value = data.User.UserMasterId;
                    // add createdby from session

                    using (MySqlDataAdapter mDa = new MySqlDataAdapter(command))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            StatusDTO<List<NotificationDTO>> status = new StatusDTO<List<NotificationDTO>>();

                            mDa.Fill(dt);

                            if(dt!=null)
                            {
                                status.ReturnObj = new List<NotificationDTO>();
                                NotificationDTO nDTO = null;
                                foreach(DataRow dr in dt.Rows)
                                {
                                    nDTO = new NotificationDTO();
                                    nDTO.NotificationText = dr["Notificationtext"].ToString();
                                    nDTO.NotificationReminderId = (int)dr["notificationreminderId"];

                                    status.ReturnObj.Add(nDTO);
                                }
                            }
                            return status;
                        }

                    }

                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<NotificationDTO> Select(int rowId)
        {
            throw new NotImplementedException();
        }
    }
}
