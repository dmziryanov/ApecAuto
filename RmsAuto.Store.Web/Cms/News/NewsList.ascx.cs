using System;
using System.Threading;
using RmsAuto.Store.Cms.Routing;
using RmsAuto.Store.Cms.Dac;

namespace RmsAuto.Store.Web.Cms.News
{
    public struct YearContainer
    {
        public int? Year { get; set; }
        public string Text { get; set; }
    }

	public partial class NewsList : System.Web.UI.UserControl
	{
		protected void Page_PreRender( object sender, EventArgs e )
		{

			if( !IsPostBack )
			{
				//using (var ctx = new DCFactory<CmsDataContext>())
				//{
				//    _ddSelectYear.Items.Add( new ListItem( "Все года", "" ) );
				//    _ddSelectYear.Items.AddRange(
				//        ctx.DataContext.NewsItems.Where( n => n.NewsItemVisible && n.NewsItemDate <= DateTime.Now ).Select( n => new ListItem( n.NewsItemDate.Year.ToString(), n.NewsItemDate.Year.ToString() ) ).Distinct().ToArray()
				//        );
				//}
			}

			//Новости
			//_linqDataSource.Where = "NewsItemVisible && NewsItemDate<=DateTime.Now";
			//if( !String.IsNullOrEmpty( _ddSelectYear.SelectedValue ) )
			//{
			//_linqDataSource.Where += " && NewsItemDate.Year = " + DateTime.Now.Year;//_ddSelectYear.SelectedValue;
			//}
			//добавлено для локализации
			//_linqDataSource.Where += string.Format(" && Localization = \"{0}\"", Thread.CurrentThread.CurrentCulture.Name);
            _listView.DataSource = NewsItemsDac.GetAllNews(null);
			_listView.DataBind();
		}
        protected string GetFileUrl(int fileID)
        {
            return UrlManager.GetFileUrl(fileID);
        }

	}
}