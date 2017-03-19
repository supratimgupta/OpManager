using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpMgr.Common.Contracts.Modules;
using MySql.Data.MySqlClient;
using OpMgr.Common.Contracts;
using System.Data;
using OpMgr.Common.DTOs;

namespace OpMgr.DataAccess.Implementations
{
    public class BookMasterSvc : IBookMasterSvc, IDisposable
    {
        private IConfigSvc _configSvc;
        private DataTable _dtData;
        private DataSet _dsData;

        public BookMasterSvc(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        public StatusDTO<BookMasterDTO> Delete(BookMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public StatusDTO<BookMasterDTO> Insert(BookMasterDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "ins_LibraryBook";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    if (data.CreatedBy != null)
                    {
                        command.Parameters.Add("@CreatedBy", MySqlDbType.Int32).Value = data.CreatedBy;
                    }
                    else
                    {
                        command.Parameters.Add("@CreatedBy", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@BookName", MySqlDbType.String).Value = data.BookName;
                    command.Parameters.Add("@AuthorName1", MySqlDbType.String).Value = data.AuthorName1;
                    if (data.AuthorName2 != null)
                    {
                        command.Parameters.Add("@AuthorName2", MySqlDbType.String).Value = data.AuthorName2;
                    }
                    else
                    {
                        command.Parameters.Add("@AuthorName2", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if (data.PurchaseDate != null)
                    {
                        command.Parameters.Add("@PurchaseDate", MySqlDbType.Date).Value = Convert.ToDateTime(data.PurchaseDate);
                    }
                    else
                    {
                        command.Parameters.Add("@PurchaseDate", MySqlDbType.Date).Value = DBNull.Value;
                    }
                    if (data.AccDate != null)
                    {
                        command.Parameters.Add("@AccDate", MySqlDbType.Date).Value = Convert.ToDateTime(data.AccDate);
                    }
                    else
                    {
                        command.Parameters.Add("@AccDate", MySqlDbType.Date).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@AccNo", MySqlDbType.String).Value = data.AccNo;
                    command.Parameters.Add("@CallNo", MySqlDbType.String).Value = data.CALLNO;
                    command.Parameters.Add("@ISBNNO", MySqlDbType.String).Value = data.ISBNNo;

                    command.Parameters.Add("@Edition", MySqlDbType.String).Value = data.Edition;
                    command.Parameters.Add("@Publisher", MySqlDbType.String).Value = data.Publisher;
                    if (data.PublishingYear != null)
                    {
                        command.Parameters.Add("@PublishingYear", MySqlDbType.DateTime).Value = Convert.ToDateTime(data.PublishingYear);
                    }
                    else
                    {
                        command.Parameters.Add("@PublishingYear", MySqlDbType.DateTime).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@Place", MySqlDbType.String).Value = data.Place;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Location.LocationId;
                    command.Parameters.Add("@Series", MySqlDbType.String).Value = data.Series;
                    command.Parameters.Add("@Price", MySqlDbType.Decimal).Value = Convert.ToDecimal(data.Price);
                    command.Parameters.Add("@NOFCID", MySqlDbType.String).Value = data.NOFCD;
                    command.Parameters.Add("@BookCategoryId", MySqlDbType.Int32).Value = data.BookCategory.BookCategoryId;

                    MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    _dtData = new DataTable();
                    _dtData.Load(rdr);
                    StatusDTO<BookMasterDTO> status = new StatusDTO<BookMasterDTO>();
                    return status;
                    //MySqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection);
                    //StatusDTO<BookMasterDTO> status = new StatusDTO<BookMasterDTO>();
                    //status.IsSuccess = true;
                    //status.IsException = false;
                    //status.ReturnObj = data;
                    //return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
        public StatusDTO<BookMasterDTO> Select(int bookMasterId)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "get_LibraryBook";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;
                    command.Parameters.Add("@BookMasterId1", MySqlDbType.Int32).Value = bookMasterId;

                    MySqlDataAdapter rdr = new MySqlDataAdapter(command);
                    _dsData = new DataSet();
                    rdr.Fill(_dsData);
                    StatusDTO<BookMasterDTO> status = new StatusDTO<BookMasterDTO>();
                    BookMasterDTO bookmasterdto = new BookMasterDTO();
                    if (_dsData != null && _dsData.Tables.Count > 0)
                    {
                        if (_dsData.Tables[0].Rows.Count > 0)
                        {
                            bookmasterdto.BookMasterId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["BookMasterId"]);
                            bookmasterdto.BookName = _dsData.Tables[0].Rows[0]["BookName"].ToString();
                            bookmasterdto.AuthorName1 = _dsData.Tables[0].Rows[0]["AuthorName1"].ToString();
                            bookmasterdto.AuthorName2 = _dsData.Tables[0].Rows[0]["AuthorName2"].ToString();
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["PurchaseDate"].ToString()))
                            {
                                bookmasterdto.PurchaseDate = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["PurchaseDate"]);
                            }
                            else
                            {
                                bookmasterdto.PurchaseDate = null;
                            }
                            bookmasterdto.AccNo = _dsData.Tables[0].Rows[0]["AccNo"].ToString();
                            if(!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["AccDate"].ToString()))
                            {
                                bookmasterdto.AccDate = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["AccDate"]);
                            }
                            else
                            {
                                bookmasterdto.AccDate = null;
                            }
                            
                            bookmasterdto.CALLNO = _dsData.Tables[0].Rows[0]["CallNo"].ToString();
                            bookmasterdto.ISBNNo = _dsData.Tables[0].Rows[0]["ISBNNO"].ToString();
                            bookmasterdto.Edition = _dsData.Tables[0].Rows[0]["Edition"].ToString();
                            bookmasterdto.Publisher = _dsData.Tables[0].Rows[0]["Publisher"].ToString();
                            if (!String.IsNullOrEmpty(_dsData.Tables[0].Rows[0]["PublishingYear"].ToString()))
                            {
                                bookmasterdto.PublishingYear = Convert.ToDateTime(_dsData.Tables[0].Rows[0]["PublishingYear"]);
                            }
                            else
                            {
                                bookmasterdto.PublishingYear = null;
                            }
                            bookmasterdto.Place = _dsData.Tables[0].Rows[0]["Place"].ToString();
                            bookmasterdto.Series = _dsData.Tables[0].Rows[0]["Series"].ToString();
                            bookmasterdto.Price = Convert.ToString(_dsData.Tables[0].Rows[0]["Price"]);
                            bookmasterdto.NOFCD = _dsData.Tables[0].Rows[0]["NOFCID"].ToString();
                            bookmasterdto.BookCategory = new BookCategoryDTO();
                            bookmasterdto.BookCategory.BookCategoryId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["BookCategoryId"]);
                            bookmasterdto.Location = new LocationDTO();
                            bookmasterdto.Location.LocationId = Convert.ToInt32(_dsData.Tables[0].Rows[0]["LocationId"]);
                        }
                    }
                    status.ReturnObj = bookmasterdto;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public StatusDTO<List<BookMasterDTO>> Select(BookMasterDTO data)
        {
            throw new NotImplementedException();
        }

        public StatusDTO<BookMasterDTO> Update(BookMasterDTO data)
        {
            using (IDbSvc dbSvc = new DbSvc(_configSvc))
            {
                try
                {
                    dbSvc.OpenConnection();
                    MySqlCommand command = new MySqlCommand();
                    command.CommandText = "up_LibraryBook";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = dbSvc.GetConnection() as MySqlConnection;

                    command.Parameters.Add("@BookMasterId1", MySqlDbType.Int32).Value = data.BookMasterId;
                    if (data.UpdatedBy != null)
                    {
                        command.Parameters.Add("@UpdatedBy", MySqlDbType.Int32).Value = data.UpdatedBy;
                    }
                    else
                    {
                        command.Parameters.Add("@UpdatedBy", MySqlDbType.Int32).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@BookName", MySqlDbType.String).Value = data.BookName;
                    command.Parameters.Add("@AuthorName1", MySqlDbType.String).Value = data.AuthorName1;
                    if (data.AuthorName2 != null)
                    {
                        command.Parameters.Add("@AuthorName2", MySqlDbType.String).Value = data.AuthorName2;
                    }
                    else
                    {
                        command.Parameters.Add("@AuthorName2", MySqlDbType.String).Value = DBNull.Value;
                    }
                    if (data.PurchaseDate != null)
                    {
                        command.Parameters.Add("@PurchaseDate", MySqlDbType.Date).Value = Convert.ToDateTime(data.PurchaseDate);
                    }
                    else
                    {
                        command.Parameters.Add("@PurchaseDate", MySqlDbType.Date).Value = DBNull.Value;
                    }
                    if (data.AccDate != null)
                    {
                        command.Parameters.Add("@AccDate", MySqlDbType.Date).Value = Convert.ToDateTime(data.AccDate);
                    }
                    else
                    {
                        command.Parameters.Add("@AccDate", MySqlDbType.Date).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@AccNo", MySqlDbType.String).Value = data.AccNo;
                    command.Parameters.Add("@CallNo", MySqlDbType.String).Value = data.CALLNO;
                    command.Parameters.Add("@ISBNNO", MySqlDbType.String).Value = data.ISBNNo;

                    command.Parameters.Add("@Edition", MySqlDbType.String).Value = data.Edition;
                    command.Parameters.Add("@Publisher", MySqlDbType.String).Value = data.Publisher;
                    if (data.PublishingYear != null)
                    {
                        command.Parameters.Add("@PublishingYear", MySqlDbType.DateTime).Value = Convert.ToDateTime(data.PublishingYear);
                    }
                    else
                    {
                        command.Parameters.Add("@PublishingYear", MySqlDbType.DateTime).Value = DBNull.Value;
                    }
                    command.Parameters.Add("@Place", MySqlDbType.String).Value = data.Place;
                    command.Parameters.Add("@LocationId", MySqlDbType.Int32).Value = data.Location.LocationId;
                    command.Parameters.Add("@Series", MySqlDbType.String).Value = data.Series;
                    command.Parameters.Add("@Price", MySqlDbType.Decimal).Value = Convert.ToDecimal(data.Price);
                    command.Parameters.Add("@NOFCID", MySqlDbType.String).Value = data.NOFCD;
                    command.Parameters.Add("@BookCategoryId", MySqlDbType.Int32).Value = data.BookCategory.BookCategoryId;

                    command.ExecuteNonQuery();
                    StatusDTO<BookMasterDTO> status = new StatusDTO<BookMasterDTO>();
                    status.IsSuccess = true;
                    status.IsException = false;
                    status.ReturnObj = data;
                    return status;
                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }
        }
    }
}
