using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Windows.Forms;
using SetupForms;
using System.IO;
using System.Diagnostics;

namespace RmsAuto.Store.MaintSvcs
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public class WindowWrapper : System.Windows.Forms.IWin32Window
        {
            public WindowWrapper(IntPtr handle)
            {
                _hwnd = handle;
            }

            public IntPtr Handle
            {
                get { return _hwnd; }
            }

            private IntPtr _hwnd;
        }
        private WindowWrapper GetWrapper()
        {
            //Get the process Hwnd
            IntPtr hwnd = IntPtr.Zero;
            WindowWrapper wrapper = null;
            Process[] procs = Process.GetProcessesByName("msiexec");

            if (null != procs && procs.Length > 0)
                hwnd = procs[0].MainWindowHandle;

            if (hwnd != IntPtr.Zero)
                wrapper = new WindowWrapper(hwnd);

            //Set the windows forms owner to setup project so it can be focused and 
            //set infront
            return wrapper;
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            string franchName = this.Context.Parameters["FRANCHNAME"];
            string importFileFolder = this.Context.Parameters["IMPORTFILEFOLDER"];
			string internalFranchName = this.Context.Parameters["INTERNALFRANCHNAME"];

            List<string> ImportFolders = new List<string>();
            ImportFolders.Add(importFileFolder + @"\prices");
            ImportFolders.Add(importFileFolder + @"\price_factors");
            ImportFolders.Add(importFileFolder + @"\crosses");
            ImportFolders.Add(importFileFolder + @"\crosses_brands");
            ImportFolders.Add(importFileFolder + @"\crosses_groups");
            ImportFolders.Add(importFileFolder + @"\crosses_links");

			if (!Directory.Exists(importFileFolder))
			{
				//Показать поверх окна установки
				if (TopMostMessageBox.Show("Папка " + importFileFolder + " не существует" + Environment.NewLine + @"cоздать?", "Внимание", MessageBoxButtons.OKCancel) == DialogResult.OK)
				{
					foreach (string ppath in ImportFolders)
					{
						Directory.CreateDirectory(ppath);
					}
				}
			}

			else
			{
				foreach (string ppath in ImportFolders)
				{
					if (!Directory.Exists(ppath))
					{
						if (TopMostMessageBox.Show("Папка " + ppath + " не существует" + Environment.NewLine + @"cоздать?", "Внимание", MessageBoxButtons.OKCancel) == DialogResult.OK)
						{
							Directory.CreateDirectory(ppath);
						}
					}
				}
			}

            string targetDir = this.Context.Parameters["TARGETDIR"];
            String[] lines = new String[2];
            installerPriceMaintSvc.ServiceName = franchName + ".PriceMaintService";
            installerNotificationSvc.ServiceName = franchName + ".NotificationService";
            lines[0] = installerPriceMaintSvc.ServiceName;
            lines[1] = installerNotificationSvc.ServiceName;
            stateSaver.Add("FRANCHNAME", franchName);

            Program.PriceMaintServiceName = installerPriceMaintSvc.ServiceName;
            Program.NotificationServiceName = installerNotificationSvc.ServiceName;
            //выводимое имя должно отличаться
            installerPriceMaintSvc.DisplayName = franchName + ".Price maintenance service";
            installerNotificationSvc.DisplayName = franchName + ".Notification service";
            installerPriceMaintSvc.Description = "Сервис импорта прайс-листов в БД " + franchName;
            installerNotificationSvc.Description = "Сервис отправки оповещений " + franchName;

            FileStream fs = File.Create(targetDir + @"\Params.txt");
            fs.Close();
            File.WriteAllLines(targetDir + @"\Params.txt", lines);

            fs = File.Create(targetDir + @"\Uninstall.bat");

            fs.Close();
            lines = new string[1];
            lines[0] = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\installutil /uninstall " + System.Reflection.Assembly.GetExecutingAssembly().Location;
            File.WriteAllLines(targetDir + @"\Uninstall.bat", lines);

            base.Install(stateSaver); //Для сохранения конфига это нужно вызывать до config.save()
            //TODO: в будущем возможно при сетапе вводить логин/пароль под которыми будет запускаться сервис и выставлять StartUpType и LogOn у сервиса

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);
            //   config.GetSection("smtp").ElementInformation

            ConnectionStringSetupDialog cssDlg = new ConnectionStringSetupDialog();
            cssDlg.CommonDB = config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Properties.Settings.ex_rmsauto_commonConnectionString"].ConnectionString;
            cssDlg.StoreDB = config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Properties.Settings.ex_rmsauto_storeConnectionString"].ConnectionString;
            cssDlg.LogDB = config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Properties.Settings.ex_rmsauto_logConnectionString"].ConnectionString;
            cssDlg.ShowDialog();

            config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Properties.Settings.ex_rmsauto_storeConnectionString"].ConnectionString = cssDlg.StoreDB;
            config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Cms.Properties.Settings.ex_rmsauto_storeConnectionString"].ConnectionString = cssDlg.StoreDB;
            config.ConnectionStrings.ConnectionStrings["RmsAuto.Forum.Core.Properties.Settings.ex_rmsauto_storeConnectionString"].ConnectionString = cssDlg.StoreDB;
            config.ConnectionStrings.ConnectionStrings["RmsAuto.TechDoc.Properties.Settings.ex_rmsauto_storeConnectionString"].ConnectionString = cssDlg.StoreDB;
            config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Properties.Settings.ex_rmsauto_logConnectionString"].ConnectionString = cssDlg.LogDB;
            config.ConnectionStrings.ConnectionStrings["ex_rmsauto_storeConnectionString"].ConnectionString = cssDlg.StoreDB;
            config.ConnectionStrings.ConnectionStrings["RmsAuto.Acctg.Properties.Settings.ex_rmsauto_storeConnectionString"].ConnectionString = cssDlg.StoreDB;
            config.ConnectionStrings.ConnectionStrings["RmsAuto.Store.Properties.Settings.ex_rmsauto_commonConnectionString"].ConnectionString = cssDlg.CommonDB;

            //TODO: попытаться записать комменты в секцию appConfig, чтобы она выглядела как-то так:
            #region --- comments ---
            //<!-- папки с файлами для импорта -->
            //<add key="pricePickupPath" value="C:\Franch\prices" />
            //<add key="priceFactorPickupPath" value="C:\Franch\price_factors" />
            //<add key="crossPickupPath" value="C:\Franch\crosses" />
            //<add key="crossBrandsPickupPath" value="C:\Franch\crosses_brands" />
            //<add key="crossGroupsPickupPath" value="C:\Franch\crosses_groups" />
            //<add key="crossLinksPickupPath" value="C:\Franch\crosses_links" />
            //<!-- периодичность работы сервисов -->
            //<add key="importPollingSeconds" value="60" />
            //<add key="statusPollingSeconds" value="60" />
            //<add key="notificationPollingSeconds" value="60" />
            //<add key="recNotificationPollingSeconds" value="3600" />
            //<!-- настройки логгирования -->
            //<add key="logDetails" value="true" />
            //<add key="logEncoding" value="windows-1251" />
            //<add key="traceMode" value="true" />
            //<!--
            //logDetails - causes writing of .err file
            //logEncoding - .err file encoding
            //traceMode - causes writing trace messages to EventLog
            //-->
            #endregion
            //TODO: Записать в раздел конфига почту рассылки как название Франча@rmsauto.ru
            config.Save(ConfigurationSaveMode.Modified, false);
            ConfigurationManager.RefreshSection("ConnectionStrings");

            //задаем значения путей для импорта
			config.AppSettings.Settings["pricePickupPath"].Value = ImportFolders[0];
            config.AppSettings.Settings["priceFactorPickupPath"].Value = ImportFolders[1];
            config.AppSettings.Settings["crossPickupPath"].Value = ImportFolders[2];
            config.AppSettings.Settings["crossBrandsPickupPath"].Value = ImportFolders[3];
            config.AppSettings.Settings["crossGroupsPickupPath"].Value = ImportFolders[4];
            config.AppSettings.Settings["crossLinksPickupPath"].Value = ImportFolders[5];
			//задаем внутреннее имя франча
			config.AppSettings.Settings["InternalFranchName"].Value = internalFranchName;

            config.Save(ConfigurationSaveMode.Minimal, false);
            ConfigurationManager.RefreshSection("appSettings");
            if (!cssDlg.ContinueSetup) { throw new InstallException("Установка прервана пользователем"); }
        }

        public override void Commit(IDictionary mySavedState)
        {
            base.Commit(mySavedState);
        }

        protected override void OnBeforeInstall(System.Collections.IDictionary savedState)
        {
            //your verification code here
            base.OnBeforeInstall(savedState);
        }

        public override void Uninstall(System.Collections.IDictionary stateSaver)
        {
            installerPriceMaintSvc.ServiceName = stateSaver["FRANCHNAME"] + ".PriceMaintService";
            installerNotificationSvc.ServiceName = stateSaver["FRANCHNAME"] + ".NotificationService";
            base.Uninstall(stateSaver);
        }
    }

    static public class TopMostMessageBox
    {
        static public DialogResult Show(string message)
        {
            return Show(message, string.Empty, MessageBoxButtons.OK);
        }

        static public DialogResult Show(string message, string title)
        {
            return Show(message, title, MessageBoxButtons.OK);
        }

        static public DialogResult Show(string message, string title, MessageBoxButtons buttons)
        {
            // Create a host form that is a TopMost window which will be the 
            // parent of the MessageBox.
            Form topmostForm = new Form();
            // We do not want anyone to see this window so position it off the 
            // visible screen and make it as small as possible
            topmostForm.Size = new System.Drawing.Size(1, 1);
            topmostForm.StartPosition = FormStartPosition.Manual;
            System.Drawing.Rectangle rect = SystemInformation.VirtualScreen;
            topmostForm.Location = new System.Drawing.Point(rect.Bottom + 10,
                rect.Right + 10);
            topmostForm.Show();
            // Make this form the active form and make it TopMost
            topmostForm.Focus();
            topmostForm.BringToFront();
            topmostForm.TopMost = true;
            // Finally show the MessageBox with the form just created as its owner
            DialogResult result = MessageBox.Show(topmostForm, message, title,
                buttons);
            topmostForm.Dispose(); // clean it up all the way

            return result;
        }
    }
}
