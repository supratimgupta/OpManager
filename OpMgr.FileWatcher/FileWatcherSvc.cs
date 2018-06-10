using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using OpMgr.FileWatcher.DataAccess;

namespace OpMgr.FileWatcher
{
    partial class FileWatcherSvc : ServiceBase
    {
        FileSystemWatcher _stuFileSystemWatcher;
        FileSystemWatcher _staFileSystemWatcher;
        FileSystemWatcher _apprFileSystemWatcher;
        public FileWatcherSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            InitiateStudentFileSystemWatcher();
            InitiateStaffFileSystemWatcher();
            InitiateApprFileSystemWatcher();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            _stuFileSystemWatcher = null;
            _staFileSystemWatcher = null;
            _apprFileSystemWatcher = null;
        }

        private void InitiateStudentFileSystemWatcher()
        {
            _stuFileSystemWatcher = new FileSystemWatcher(ConfigurationManager.AppSettings["StudentBulkUploadExcelFilePath"]);
            
            _stuFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _stuFileSystemWatcher.Filter = "*.*";
            _stuFileSystemWatcher.EnableRaisingEvents = true;
                        
            _stuFileSystemWatcher.Created += _stuFileSystemWatcher_Created;
        }

        void _stuFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            AbsFileUpload stuUpload = new StudentUpload();
            stuUpload.ImportFileToSQL(e.FullPath);
            File.Move(e.FullPath, ConfigurationManager.AppSettings["StudentFileArchive"] + "\\" + DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss") + ".arc");
        }

        private void InitiateStaffFileSystemWatcher()
        {
            _staFileSystemWatcher = new FileSystemWatcher(ConfigurationManager.AppSettings["StaffBulkUploadExcelFilePath"]);

            _staFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _staFileSystemWatcher.Filter = "*.*";
            _staFileSystemWatcher.EnableRaisingEvents = true;

            _staFileSystemWatcher.Created += _staFileSystemWatcher_Created;
        }

        void _staFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            AbsFileUpload staUpload = new StaffUpload();
            staUpload.ImportFileToSQL(e.FullPath);
            File.Move(e.FullPath, ConfigurationManager.AppSettings["StaffFileArchive"] + "\\" + DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss") + ".arc");
        }

        private void InitiateApprFileSystemWatcher()
        {
            _apprFileSystemWatcher = new FileSystemWatcher(ConfigurationManager.AppSettings["AppraisalDataUploadPath"]);

            _apprFileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _apprFileSystemWatcher.Filter = "*.*";
            _apprFileSystemWatcher.EnableRaisingEvents = true;

            _apprFileSystemWatcher.Created += _apprFileSystemWatcher_Created;
        }

        void _apprFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            //Need to change the following from Staff upload to appraisal upload
            MySqlDataAccess pmsdataaccess = new MySqlDataAccess();
            AbsFileUpload appraisalupload = new AppraisalUpload(pmsdataaccess);
            //AbsFileUpload appraisalupload = new AppraisalUpload();
            appraisalupload.ImportFileToSQL(e.FullPath);
            //Move the same to archive
            File.Move(e.FullPath, ConfigurationManager.AppSettings["AppraisalFileArchive"] + "\\" + DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss") + ".arc");
        }
    }
}
