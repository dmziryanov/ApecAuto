namespace RmsAuto.Store.MaintSvcs
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
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.installerPriceMaintSvc = new System.ServiceProcess.ServiceInstaller();
            this.installerNotificationSvc = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // installerPriceMaintSvc
            // 
            this.installerPriceMaintSvc.Description = "Сервис импорта прайс-листов в БД интернет-магазина РМС-Авто";
            this.installerPriceMaintSvc.DisplayName = "logistauto.Price maintenance service";
            this.installerPriceMaintSvc.ServiceName = "logistauto.PriceMaintService";
            // 
            // installerNotificationSvc
            // 
            this.installerNotificationSvc.Description = "Сервис отправки оповещений интернет-магазина РМС-Авто";
            this.installerNotificationSvc.DisplayName = "logistauto.Notification service";
            this.installerNotificationSvc.ServiceName = "logistauto.NotificationService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.installerPriceMaintSvc,
            this.installerNotificationSvc});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller installerPriceMaintSvc;
        private System.ServiceProcess.ServiceInstaller installerNotificationSvc;
	}
}