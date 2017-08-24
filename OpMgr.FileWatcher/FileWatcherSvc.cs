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

namespace OpMgr.FileWatcher
{
    partial class FileWatcherSvc : ServiceBase
    {
        FileSystemWatcher _stuFileSystemWatcher;
        FileSystemWatcher _staFileSystemWatcher;
        public FileWatcherSvc()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            InitiateStudentFileSystemWatcher();
            InitiateStaffFileSystemWatcher();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            _stuFileSystemWatcher = null;
            _staFileSystemWatcher = null;
        }

        private void InitiateStudentFileSystemWatcher()
        {
            _stuFileSystemWatcher = new FileSystemWatcher();
            _stuFileSystemWatcher.Path = ConfigurationManager.AppSettings["StudentBulkUploadExcelFilePath"];
            _stuFileSystemWatcher.Created += _stuFileSystemWatcher_Created;
        }

        void _stuFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            StudentUpload stuUpload = new StudentUpload();
            stuUpload.ImportFileToSQL(e.FullPath);
            File.Move(e.FullPath, ConfigurationManager.AppSettings["StudentFileArchive"] + "\\" + DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss") + ".arc");
        }

        private void InitiateStaffFileSystemWatcher()
        {
            _staFileSystemWatcher = new FileSystemWatcher();
            _staFileSystemWatcher.Path = ConfigurationManager.AppSettings["StaffBulkUploadExcelFilePath"];
            _staFileSystemWatcher.Created += _staFileSystemWatcher_Created;
        }

        void _staFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            StaffUpload staUpload = new StaffUpload();
            staUpload.StaffImportFileToSQL(e.FullPath);
            File.Move(e.FullPath, ConfigurationManager.AppSettings["StaffFileArchive"] + "\\" + DateTime.Now.ToString("dd.MM.yyyy.hh.mm.ss") + ".arc");
        }
    }
}
