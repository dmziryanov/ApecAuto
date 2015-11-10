<%@ Page ValidateRequest="false" Language="c#" %>
<%@ Import Namespace="RmsAuto.Store.Cms.Entities" %>
<%@ Import Namespace="RmsAuto.Store.Data" %>
<%@ Import Namespace="System.Linq" %>

<script runat="server">

	public int FolderID
	{
		get { return Convert.ToInt32( Request[ "FolderID" ] ); }
	}

	protected override void OnLoad( EventArgs e )
	{
		base.OnLoad( e );

	}

	protected override void OnPreRender( EventArgs e )
	{
		base.OnPreRender( e );

		try
		{

			UploadPanel.Visible = true;
			
		}
		catch( Exception ex )
		{
			ErrorLabel.Text = ex.Message;
		}
	}

	protected void UploadButton_Click( object sender, EventArgs e )
	{
		try
		{
			if( !FileUploadBox.HasFile )
				throw new Exception( "Выберите файл" );

            using (var dc = new DCWrappersFactory<CmsDataContext>())
            {
				File file = new File();
				file.FolderID = FolderID;
				file.FileBody = new System.Data.Linq.Binary( FileUploadBox.FileBytes );
				file.FileName = FileUploadBox.FileName;
				file.FileSize = FileUploadBox.FileBytes.Length;
				file.FileMimeType = FileUploadBox.PostedFile.ContentType;
				file.FileNote = string.IsNullOrEmpty( CommentBox.Text.Trim() ) ? null : CommentBox.Text.Trim();
				file.FileCreationDate = DateTime.Now;
				file.FileModificationDate = DateTime.Now;

				try
				{
					using( System.IO.MemoryStream stm = new System.IO.MemoryStream( FileUploadBox.FileBytes ) )
					using( System.Drawing.Image img = System.Drawing.Image.FromStream( stm ) )
					{
						file.IsImage = true;
						file.ImageWidth = img.Width;
						file.ImageHeight = img.Height;
					}
				}
				catch
				{
				}

				dc.DataContext.Files.InsertOnSubmit( file );
				dc.DataContext.SubmitChanges();

				ClientScript.RegisterStartupScript( GetType(), "key", "__uploadCompleted();", true );
			}
		}
		catch( Exception ex )
		{
			ErrorLabel.Text = ex.Message;
		}
	}

</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title></title>
	<base target="_self" />
	<style>
		body  
		{
			padding:0px; 
			margin: 0px; 
			font-family: Tahoma, Arial, Sans-Serif; 
			font-size: 8pt;
			background-color: #F0F0EE;
		}
		input  
		{
			width:280px; 
			border: solid 1px #808080;
			font-family: Tahoma, Arial, Sans-Serif; 
			font-size: 8pt  
		}
		input.submit_btn { width: auto }
	</style>
	
	<script language="javascript">
		function __uploadCompleted()
		{
			window.parent.__refreshContent();
		}
	</script>
</head>
<body>
    <form runat="server"> 

		<asp:Panel runat="server" ID="UploadPanel">
			<table border="0" cellpadding="2" cellspacing="0">
				<tr>
					<td class="column1"><label id="commentLabel" for="<%=CommentBox.ClientID%>">Описание:</label></td>
					<td nowrap="nowrap">
						<asp:TextBox runat="server" ID="CommentBox" />
					</td>
				</tr>
				<tr>
					<td class="column1"><label id="uploadFileLabel" for="<%=FileUploadBox.ClientID%>">Файл:</label></td>
					<td nowrap="nowrap">
						<asp:FileUpload runat=server ID="FileUploadBox" />
					</td>
				</tr>
				<tr>
					<td class="column1"></td>
					<td nowrap="nowrap">
						<asp:Button runat=server ID="UploadButton" Text="Загрузить" CssClass="submit_btn" OnClick="UploadButton_Click" />
						<asp:Label runat="server" ID="ErrorLabel" EnableViewState="false" Font-Bold="true" ForeColor="Red" />
					</td>
				</tr>
			</table>
		</asp:Panel>


	</form>
</body> 
</html> 
