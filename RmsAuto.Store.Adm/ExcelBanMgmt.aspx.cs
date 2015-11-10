using System;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Linq;
using System.Data.Linq;
using RmsAuto.Store.Entities;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Adm
{
    public partial class ExcelBanMgmt : Security.BasePage/*System.Web.UI.Page*/
    {
        protected MetaTable table;

        protected void Page_Load(object sender, EventArgs e)
        {
            table = linqDataSource.GetTable();
            
            if (!IsPostBack)
            {
                _errorPanel.Visible = false;
            }
        }
        protected void LoginsGridView_OnRowCommand(object sender, GridViewCommandEventArgs args)
        {
            
        }
        protected void _btnApplyFilter_Click(object sender, EventArgs e)
        {
            _loginsGridView.PageIndex = 0;
        }
        protected void resetBuns_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _loginsGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)_loginsGridView.Rows[i].Cells[5].Controls[0];

                if (cb.Checked)
                {
                    int uid = Convert.ToInt32(((Label)_loginsGridView.Rows[i].Cells[6].Controls[0]).Text);
                    DeleteAllBanClientEntries(uid);
                    _loginsGridView.Rows[i].Visible = cb.Checked = false;
                }
            }
        }
        static Func<StoreDataContext, int, IQueryable<BanClientAction>>
        _getBanClientEntries = CompiledQuery.Compile((StoreDataContext dc, int uid) =>
        from entry in dc.BanClientActions where entry.UserID == uid select entry);

        void DeleteAllBanClientEntries(int uid)
        {
            using (var dc = new DCWrappersFactory<StoreDataContext>())
            {
                dc.DataContext.BanClientActions.DeleteAllOnSubmit(_getBanClientEntries(dc.DataContext, uid));
                dc.DataContext.SubmitChanges();
            }
        }
    }
}
