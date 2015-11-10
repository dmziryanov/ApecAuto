using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmsAuto.Store.Cms.Entities;
using System.Data.Linq;
using RmsAuto.Store.Data;

namespace RmsAuto.Store.Cms.Dac
{
	public static class FilesDac
	{
		private static Func<CmsDataContext, int, Folder> _getFolderById =
			CompiledQuery.Compile(
			( CmsDataContext dc, int folderID ) =>
				dc.Folders.Where( f => f.FolderID == folderID ).SingleOrDefault() );

		private static Func<CmsDataContext, int, File> _getFileById =
			CompiledQuery.Compile(
			( CmsDataContext dc, int fileID ) =>
				dc.Files.Where( f => f.FileID == fileID ).SingleOrDefault() );

		private static Func<CmsDataContext, string, int> _getFileIDByFileName =
			CompiledQuery.Compile(
			(CmsDataContext dc, string fileName) =>
				dc.Files.Where(f => f.FileName == fileName).Select(f => f.FileID).SingleOrDefault());

		/// <summary>
		/// Возвращает ID файла по его наименованию
		/// </summary>
		/// <param name="fileName">Имя файла</param>
		/// <returns>ID файла</returns>
		public static int GetFileIDByName(string fileName)
		{
			using (var dc = new DCWrappersFactory<CmsDataContext>())
			{
				return _getFileIDByFileName(dc.DataContext, fileName);
			}
		}

		public static string GetFolderPath( int folderID )
		{
			using(var dc = new DCWrappersFactory<CmsDataContext>())
			{
				Folder folder = _getFolderById( dc.DataContext, folderID );

				if( folder != null )
				{
					List<Folder> list = new List<Folder>();
					for( Folder f = folder ; f != null ; f = f.Parent )
					{
						list.Add( f );
					}
					
                    list.Reverse();

					StringBuilder sb = new StringBuilder();
					foreach( Folder f in list )
					{
						if( sb.Length != 0 ) sb.Append( " / " );
						sb.Append( f.FolderName );
					}
					return sb.ToString();
				}
				else
				{
					return null;
				}
			}
		}

		public static File GetFile( CmsDataContext dc, int fileID )
		{
			return _getFileById( dc, fileID );
		}

        public static Func<CmsDataContext, String, Folder> GetFolderByName =
        	CompiledQuery.Compile(
			( CmsDataContext dc, String folderName ) =>
                dc.Folders.Where(f => f.FolderName == folderName ).SingleOrDefault());

        public static int GetMaxID(CmsDataContext dc)
        {
            var el =
                   from o in dc.Files
                   select (o.FileID);
            try
            {
                return (int)el.Max();
            }
            catch
            {
                return 0;
            }
        }

        public static int GetMaxID()
        {
            var dc = new DCWrappersFactory<CmsDataContext>();
            return GetMaxID(dc.DataContext);
        }

        public static Func<CmsDataContext, String, Int32, File> GetFileByNameAndFolderID =
            CompiledQuery.Compile(
            (CmsDataContext dc, String fileName, Int32 folderID) =>
                dc.Files.Where(f => f.FileName == fileName && f.FolderID == folderID).First());

        public static File GetFileRms(CmsDataContext cmsDataContext, int fileID)
        {
            spSelFilesFromRmsResult rmsFile = cmsDataContext.spSelFilesFromRms(fileID).SingleOrDefault();
            if (rmsFile == null) return null;
            File f = new File();
            f.ImageHeight = rmsFile.ImageHeight;
            f.ImageWidth = rmsFile.ImageWidth;
            f.FileBody = rmsFile.FileBody;
            f.FileCreationDate = rmsFile.FileCreationDate;
            f.FileID = rmsFile.FileID;
            f.FileMimeType = rmsFile.FileMimeType;
            f.FileModificationDate = rmsFile.FileModificationDate;
            f.FileName = rmsFile.FileName;
            f.FileNote = rmsFile.FileNote;
            f.FileSize = rmsFile.FileSize;
            f.FolderID = rmsFile.FolderID;
            f.IsImage = rmsFile.IsImage;
            f.Timestamp = rmsFile.Timestamp;
            return f;
        }
    }
}
