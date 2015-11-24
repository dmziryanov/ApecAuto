using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web.UI.WebControls;
using RmsAuto.Common.DataAnnotations;
using RmsAuto.TechDoc;
using RmsAuto.TechDoc.Entities;

namespace RmsAuto.Store.Adm.DynamicData.CustomPages.InvisibleManufacturers
{
	public partial class List : Security.BasePage/*System.Web.UI.Page*/
	{
        TecdocStoreDataContext _context;

		protected void Page_Init(object sender, EventArgs e)
		{
			
            _context = new TecdocStoreDataContext();
			_context.DeferredLoadingEnabled = false;
		}

		protected void Page_Unload( object sender, EventArgs e )
		{
			_context.Dispose();
		}


		protected void Page_Load(object sender, EventArgs e)
		{
			
			if (!IsPostBack)
			{
				var manufacturers = Facade.ListManufacturers();

                var invManufacturers = _context.InvisibleManufacturers.ToList();
				manufacturers.ForEach(m => m.Invisible = invManufacturers.Find(m1 => m1.ManufacturerID == m.ID) != null);

				foreach (var manufacturer in manufacturers)
				{
					TreeNode node = new TreeNode(manufacturer.Name, manufacturer.ID.ToString());
					node.Checked = manufacturer.Invisible;
					node.PopulateOnDemand = true;
					node.SelectAction = TreeNodeSelectAction.Expand;
					TreeView1.Nodes.Add(node);
				}
			}
		}

		protected void TreeView1_TreeNodePopulate(object sender, TreeNodeEventArgs e)
		{
			switch (e.Node.Depth)
			{
				case 0:
					PopulateModels(e.Node);
					break;
				case 1:
					PopulateModifications(e.Node);
					break;
				default:
					// Do nothing.
					break;
			}

		}

		private void PopulateModifications(TreeNode node)
		{
			var mods = Facade.ListModifications(Convert.ToInt32(node.Value));

            List<InvisibleModification> invMods = _context.InvisibleModifications.Select(m => m).ToList();
			mods.ForEach(m => m.Invisible = invMods.Find(m1 => m1.ModificationID == m.ID) != null);

			foreach (var mod in mods)
			{
				TreeNode childNode = new TreeNode(mod.FullName.Tex_Text, mod.ID.ToString());
				childNode.Checked = mod.Invisible || node.Checked;
				childNode.PopulateOnDemand = false;
				childNode.SelectAction = TreeNodeSelectAction.None;
				node.ChildNodes.Add(childNode);
			}
		}

		private void PopulateModels(TreeNode node)
		{
			var models = Facade.ListModels(Convert.ToInt32(node.Value));

            var invModels = _context.InvisibleModels.Select(m => m).ToList();
			models.ForEach(m => m.Invisible = invModels.Find(m1 => m1.ModelID == m.ID) != null);

			foreach (var model in models)
			{
				TreeNode childNode = new TreeNode(model.Name.Tex_Text, model.ID.ToString());
				childNode.Checked = model.Invisible || node.Checked;
				childNode.PopulateOnDemand = true;
				childNode.SelectAction = TreeNodeSelectAction.Expand;
				node.ChildNodes.Add(childNode);
			}
		}

		protected void TreeView1_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
		{
			UpdateNode(e.Node, true);			
			CheckChilds(e.Node);
			CheckParents(e.Node);
			_context.SubmitChanges(ConflictMode.ContinueOnConflict);
		}

		private void UpdateNode(TreeNode node, bool deleteChilds)
		{
			switch (node.Depth)
			{
				case 0:
					UpdateManufacturer(Convert.ToInt32(node.Value), node.Checked ? CrudActions.Create : CrudActions.Delete, deleteChilds);
					break;
				case 1:
					UpdateModel(Convert.ToInt32(node.Value), node.Checked ? CrudActions.Create : CrudActions.Delete, deleteChilds);
					break;
				case 2:
					UpdateModification(Convert.ToInt32(node.Value), node.Checked ? CrudActions.Create : CrudActions.Delete);
					break;
			}
		}

		private void CheckParents(TreeNode node)
		{
			if (node.Parent == null) return;
			bool allChecked = true;
			foreach (TreeNode child in node.Parent.ChildNodes)
			{
				if (child.Checked) continue;

				allChecked = false;
				break;
			}
			node.Parent.Checked = allChecked;
			UpdateNode(node.Parent, false);
			CheckParents(node.Parent);
		}

		private void CheckChilds(TreeNode node)
		{
			if (node.ChildNodes.Count == 0) return;
			foreach (TreeNode child in node.ChildNodes)
			{
				child.Checked = node.Checked;
				CheckChilds(child);
			}
		}

		private void UpdateManufacturer(int manufacturerID, CrudActions action, bool deleteChilds)
		{
			switch (action)
			{
				case CrudActions.Create:
					if (_context.InvisibleManufacturers.SingleOrDefault(
											 m => m.ManufacturerID == manufacturerID) == null)
                        _context.InvisibleManufacturers.InsertOnSubmit(new InvisibleManufacturer(manufacturerID));
					break;
				case CrudActions.Delete:
                    var mv = _context.InvisibleManufacturers.SingleOrDefault(m => m.ManufacturerID == manufacturerID);
					if (mv != null)
                        _context.InvisibleManufacturers.DeleteOnSubmit(mv);
					break;
			}
			if (deleteChilds)
				UpdateModels(manufacturerID, action);
		}

		private void UpdateModels(int manufacturerID, CrudActions action)
		{
			var models = Facade.ListModels(manufacturerID);
			foreach (var model in models)
			{
				UpdateModel(model.ID, action, true);
			}
		}

		private void UpdateModel(int modelID, CrudActions action, bool deleteChilds)
		{
			switch (action)
			{
				case CrudActions.Create:
					if (_context.InvisibleModels.SingleOrDefault(
							m => m.ModelID == modelID) == null)
                        _context.InvisibleModels.InsertOnSubmit(new InvisibleModel(modelID));
					break;
				case CrudActions.Delete:
                    var mv = _context.InvisibleModels.SingleOrDefault(m => m.ModelID == modelID);
					if (mv != null)
                        _context.InvisibleModels.DeleteOnSubmit(mv);
					break;
			}
			if (deleteChilds)
				UpdateModifications(modelID, action);
		}

		private void UpdateModifications(int modelID, CrudActions action)
		{
			var modifications = Facade.ListModifications(modelID);
			foreach (var modification in modifications)
			{				
				UpdateModification(modification.ID, action);
			}
		}

		private void UpdateModification(int modificationID, CrudActions action)
		{
			switch (action)
			{
				case CrudActions.Create:
					if (_context.InvisibleModifications.SingleOrDefault(
					 m => m.ModificationID == modificationID) == null)
                        _context.InvisibleModifications.InsertOnSubmit(new InvisibleModification(modificationID));
					break;
				case CrudActions.Delete:
                    var mv = _context.InvisibleModifications.SingleOrDefault(m => m.ModificationID == modificationID);
					if (mv != null)
                        _context.InvisibleModifications.DeleteOnSubmit(mv);
					break;
			}
		}
		
	}
}
