using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Data.ConnectionUI;
using System.Threading;

namespace SetupForms
{
    public partial class ConnectionStringSetupDialog : Form
    {
        //TODO: ПРоверить задание ConnectionString
        public string StoreDB { get { return StoreConnectionLabel.Text; } set { StoreConnectionLabel.Text = value; } }
        public string LogDB { get { return LogConnectionLabel.Text; } set { LogConnectionLabel.Text = value; } }
        public string CommonDB { get { return CommonConnectionLabel.Text; } set { CommonConnectionLabel.Text = value; } }
        
        public Boolean ContinueSetup = true;
        DataConnectionDialog dcd1 = new DataConnectionDialog();
        DataConnectionDialog dcd2 = new DataConnectionDialog();
        DataConnectionDialog dcd3 = new DataConnectionDialog();

        public ConnectionStringSetupDialog()
        {
            InitializeComponent();
            
			DataSource sqlDataSource = DataSource.SqlDataSource;
            sqlDataSource.Providers.Add(DataProvider.SqlDataProvider);

            dcd1.DataSources.Add(sqlDataSource);
            dcd1.SelectedDataSource = sqlDataSource;
            dcd1.SelectedDataProvider = DataProvider.SqlDataProvider;
           // dcd1.Ch

            dcd2.DataSources.Add(sqlDataSource);
            dcd2.SelectedDataProvider = DataProvider.SqlDataProvider;
            dcd2.SelectedDataSource = sqlDataSource;

            dcd3.DataSources.Add(sqlDataSource);
            dcd3.SelectedDataProvider = DataProvider.SqlDataProvider;
            dcd3.SelectedDataSource = sqlDataSource;
        }

        private void ShowDialogBtnForCommonConnection_Click(object sender, EventArgs e)
        {   
            //TODO: необходимо присваивать имеющейся ConnectionString  CommonDB в dcd1 
            //по идее так: dcd3.ConnectionString = LogDB но почему-то это не работает;
            Thread th1 = new Thread(() => ShowDCD(dcd3, CommonConnectionLabel));
            th1.SetApartmentState(ApartmentState.STA);
            th1.Start();
        }

        private void ShowDialogBtnForLogConnection_Click(object sender, EventArgs e)
        {
            //Все эти вещи с потоками нужны были, чтобы нормально показывалась формочка с настройкой коннекций
            //Это называется Single-threaded apartment.
            Thread th1 = new Thread(() => ShowDCD(dcd2, LogConnectionLabel));
            th1.SetApartmentState(ApartmentState.STA);
            th1.Start();
            //th1.Join(); вот это вызывать нельзя иначе дедлок
        }
        
        [STAThread] //В такой метод может зайти только один поток одновременно
        protected void ShowDCD(DataConnectionDialog dcd, Label lab)
        {
            if (DataConnectionDialog.Show(dcd, this) == DialogResult.OK)
            {
                lab.Text = dcd.DisplayConnectionString;
            }
        }
      
        private void OkButton_Click(object sender, EventArgs e)
        {
            ContinueSetup = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            ContinueSetup = false;
            this.Close();
        }

        private void ShowDialogBtnForStoreConnection_Click(object sender, EventArgs e)
        {
            Thread th1 = new Thread(() => ShowDCD(dcd1, StoreConnectionLabel));
            th1.SetApartmentState(ApartmentState.STA);
            th1.Start();

        }
    }
}
