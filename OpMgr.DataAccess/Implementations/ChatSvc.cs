using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.DTOs;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.Contracts;
using MySql.Data.MySqlClient;
using System.Data;

namespace OpMgr.DataAccess.Implementations
{
    public class ChatSvc : IChatSvc
    {
        private IConfigSvc _configSvc;

        public ChatSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        public List<ChatLogDTO> GetUnreadMessages(int toUserId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT * FROM (SELECT DISTINCT CL.Id, U.FName, U.LName, U.MName, CL.Message, CL.SentByUserMasterId, CL.DatetimeLog FROM chat_log CL LEFT JOIN UserMaster U ON CL.SentByUserMasterId=U.UserMasterId" + 
                                          " WHERE (CL.IsRead=0 OR CL.IsRead IS NULL) AND CL.ToUserMasterId=@toUser ORDER BY CL.DatetimeLog desc) A";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@toUser", MySqlDbType.Int32).Value = toUserId;

                    DataTable dtData = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(dtData);

                    List<ChatLogDTO> lstChatLog = null;

                    if(dtData!=null && dtData.Rows.Count>0)
                    {
                        lstChatLog = new List<ChatLogDTO>();
                        ChatLogDTO chatLog = null;
                        foreach(DataRow dr in dtData.Rows)
                        {
                            chatLog = new ChatLogDTO();
                            chatLog.SentByUser = new UserMasterDTO();
                            chatLog.SentByUser.FName = dr["FName"].ToString();
                            chatLog.SentByUser.MName = dr["MName"].ToString();
                            chatLog.SentByUser.LName = dr["LName"].ToString();

                            chatLog.Message = dr["Message"].ToString();
                            chatLog.Id = (int)dr["Id"];

                            chatLog.SentByUser.UserMasterId = (int)dr["SentByUserMasterId"];
                            chatLog.DateTimeLog = (DateTime)dr["DatetimeLog"];

                            DateTime dateTimeNow = DateTime.Now;

                            TimeSpan timeDifference = dateTimeNow.Subtract(chatLog.DateTimeLog);

                            int days = timeDifference.Days;
                            int hours = timeDifference.Hours;
                            int mins = timeDifference.Minutes;
                            int secs = timeDifference.Seconds;

                            chatLog.MessageSentTimesAgo = string.Empty;

                            if(days>0)
                            {
                                chatLog.MessageSentTimesAgo = days+" day" +(days>1?"s":string.Empty);
                            }
                            else if(hours>0)
                            {
                                chatLog.MessageSentTimesAgo = hours + " hour" + (hours > 1 ? "s" : string.Empty);
                            }
                            else if(mins>0)
                            {
                                chatLog.MessageSentTimesAgo = mins + " minute" + (mins > 1 ? "s" : string.Empty);
                            }
                            else if (secs > 0)
                            {
                                chatLog.MessageSentTimesAgo = secs + " second" + (secs > 1 ? "s" : string.Empty);
                            }
                            chatLog.MessageSentTimesAgo = chatLog.MessageSentTimesAgo + " ago.";

                            lstChatLog.Add(chatLog);
                        }
                    }

                    return lstChatLog;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public StatusDTO<ChatLogDTO> SendMessage(ChatLogDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "INSERT INTO chat_log(SentByUserMasterId, ToUserMasterId, DatetimeLog, Message, IsRead) "+
                                          "VALUES (@sentByUser, @toUser, @dateTimeLog, @message, @isRead); SELECT LAST_INSERT_ID();";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    if (data.SentByUser != null)
                    {
                        command.Parameters.Add("@sentByUser", MySqlDbType.Int32).Value = data.SentByUser.UserMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@sentByUser", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    if (data.SentToUser != null)
                    {
                        command.Parameters.Add("@toUser", MySqlDbType.Int32).Value = data.SentToUser.UserMasterId;
                    }
                    else
                    {
                        command.Parameters.Add("@toUser", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@dateTimeLog", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.Parameters.Add("@message", MySqlDbType.String).Value = data.Message;
                    command.Parameters.Add("@isRead", MySqlDbType.Bit).Value = data.IsRead;

                    DataTable _dtData = new DataTable();
                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<ChatLogDTO> status = new StatusDTO<ChatLogDTO>();
                    status.ReturnObj = new ChatLogDTO();
                    status.ReturnObj.Id = Convert.ToInt32(_dtData.Rows[0]["LAST_INSERT_ID()"]);
                    status.IsSuccess = true;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public bool MarkAllChatAsReadForUser(int senderRowId, int recieverRowId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE chat_log Set IsRead=1 WHERE SentByUserMasterId=@fromUser AND ToUserMasterId=@toUser";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@fromUser", MySqlDbType.Int32).Value = senderRowId;
                    command.Parameters.Add("@toUser", MySqlDbType.Int32).Value = recieverRowId;
                    if (command.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public List<ChatLogDTO> GetChatHistory(int me, int other, int skipRows, int takeRow)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "SELECT * FROM (SELECT * FROM (SELECT DISTINCT CL.Id, U.FName, U.LName, U.MName, CL.Message, CL.SentByUserMasterId, CL.DatetimeLog FROM chat_log CL LEFT JOIN UserMaster U ON CL.SentByUserMasterId=U.UserMasterId" +
                                          " WHERE (CL.ToUserMasterId=@me AND CL.SentByUserMasterId=@other) OR (CL.ToUserMasterId=@other1 AND CL.SentByUserMasterId=@me1) ORDER BY CL.DatetimeLog desc) A LIMIT " + skipRows + ", " + takeRow + ") B ORDER BY DatetimeLog";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@me", MySqlDbType.Int32).Value = me;
                    command.Parameters.Add("@other", MySqlDbType.Int32).Value = other;
                    command.Parameters.Add("@other1", MySqlDbType.Int32).Value = other;
                    command.Parameters.Add("@me1", MySqlDbType.Int32).Value = me;

                    DataTable dtData = new DataTable();
                    MySqlDataAdapter mDa = new MySqlDataAdapter(command);
                    mDa.Fill(dtData);

                    List<ChatLogDTO> lstChatLog = null;

                    if (dtData != null && dtData.Rows.Count > 0)
                    {
                        lstChatLog = new List<ChatLogDTO>();
                        ChatLogDTO chatLog = null;
                        foreach (DataRow dr in dtData.Rows)
                        {
                            chatLog = new ChatLogDTO();
                            chatLog.SentByUser = new UserMasterDTO();
                            chatLog.SentByUser.FName = dr["FName"].ToString();
                            chatLog.SentByUser.MName = dr["MName"].ToString();
                            chatLog.SentByUser.LName = dr["LName"].ToString();

                            chatLog.Message = dr["Message"].ToString();
                            chatLog.Id = (int)dr["Id"];

                            chatLog.SentByUser.UserMasterId = (int)dr["SentByUserMasterId"];
                            chatLog.DateTimeLog = (DateTime)dr["DatetimeLog"];

                            chatLog.MessageSentTimesAgo = "sent at: " + chatLog.DateTimeLog.ToString("yyyy-MM-dd hh:mm:ss");

                            lstChatLog.Add(chatLog);
                        }
                    }

                    return lstChatLog;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool MarkAsRead(int chatLogId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "UPDATE chat_log Set IsRead=1 WHERE Id=@chatLogId";
                    command.CommandType = CommandType.Text;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@chatLogId", MySqlDbType.Int32).Value = chatLogId;
                    
                    if(command.ExecuteNonQuery()>0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}
