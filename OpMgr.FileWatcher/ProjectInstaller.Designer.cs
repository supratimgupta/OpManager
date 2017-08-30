using System;

namespace OpMgr.FileWatcher
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            try
            {
                this.fileWatcherSPInstaller = new System.ServiceProcess.ServiceProcessInstaller();
                this.fileWatcherSPInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
                this.fileWatcherSvcInstaller = new System.ServiceProcess.ServiceInstaller();
                // 
                // fileWatcherSPInstaller
                // 
                this.fileWatcherSPInstaller.Password = null;
                this.fileWatcherSPInstaller.Username = null;
                // 
                // fileWatcherSvcInstaller
                // 
                this.fileWatcherSvcInstaller.ServiceName = "FileWatcherSvc";
                this.fileWatcherSvcInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
                // 
                // ProjectInstaller
                // 
                this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.fileWatcherSPInstaller,
            this.fileWatcherSvcInstaller});
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("E:\\FileError.txt", ex.ToString());
            }
        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller fileWatcherSPInstaller;
        private System.ServiceProcess.ServiceInstaller fileWatcherSvcInstaller;
    }
}