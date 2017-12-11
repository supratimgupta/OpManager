using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Hubs.Connections
{
    public sealed class UserConnectionRepo
    {
        private UserConnectionRepo()
        {
            //Private constructor to not get initialized externally
        }

        private static Dictionary<int, UserConnection> _dicRepo;

        private static UserConnectionRepo _userConnRepo;

        private static object _lockObj = new object();

        public static UserConnectionRepo GetInstance()
        {
            lock(_lockObj)
            {
                if(_userConnRepo==null)
                {
                    lock(_lockObj)
                    {
                        _userConnRepo = new UserConnectionRepo();
                        _dicRepo = new Dictionary<int, UserConnection>();
                    }
                }
            }
            return _userConnRepo;
        }

        public Dictionary<int, UserConnection> GetAllActiveUsers()
        {
            return _dicRepo;
        }

        //public UserConnection this[string connectionId]
        //{
        //    get
        //    {
        //        if(!string.IsNullOrEmpty(connectionId) && _dicRepo!=null && _dicRepo.Keys.Contains(connectionId))
        //        {
        //            return _dicRepo[connectionId];
        //        }
        //        return null;
        //    }
        //}

        public UserConnection this[int userRowId]
        {
            get
            {
                if(userRowId>0 && _dicRepo.Keys.Contains(userRowId))
                {
                    var userConn = _dicRepo[userRowId];
                    if(userConn!=null)
                    {
                        return userConn;
                    }
                }
                return null;
            }
        }

        public void AddUserConnection(int userId, UserConnection user, out bool isNewUser)
        {
            lock(_dicRepo)
            {
                isNewUser = false;
                if (_dicRepo == null)
                {
                    _dicRepo = new Dictionary<int, UserConnection>();
                }

                if (_dicRepo.Keys.Contains(userId))
                {
                    UserConnection existingUser = _dicRepo[userId];
                    existingUser.ConnectionID.Add(user.ConnectionID[0]);
                    _dicRepo[userId] = existingUser;
                }
                else
                {
                    _dicRepo.Add(userId, user);
                    isNewUser = true;
                }
            }
            
        }

        public void RemoveUserConnection(int userId, string connectionId)
        {
            lock(_dicRepo)
            {
                if (_dicRepo != null && _dicRepo.Keys.Contains(userId))
                {
                    UserConnection conn = _dicRepo[userId];
                    if(conn.ConnectionID!=null && conn.ConnectionID.Count>0)
                    {
                        if(conn.ConnectionID.Contains(connectionId))
                        {
                            conn.ConnectionID.Remove(connectionId);
                            if(conn.ConnectionID==null || conn.ConnectionID.Count==0)
                            {
                                _dicRepo.Remove(userId);
                            }
                        }
                    }
                    else
                    {
                        _dicRepo.Remove(userId);
                    }
                }
            }
        }
    }
}