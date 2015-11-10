using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using RmsAuto.TechDoc.Entities.TecdocBase;
using System.Xml.Schema;
using System.Xml;

namespace RmsAuto.TechDoc.Entities.Helpers
{
    public class SearchTreeNodeHelper
    {
        public int SearchTreeNodeID { get; set; }
        public int? ParentSearchTreeNodeID { get; set; }
        public string Text { get; set; }

        public int ModelID { get; set; }
        public int CarTypeID { get; set; }

		public List<SearchTreeNodeHelper> Children { get; set; }

        public SearchTreeNodeHelper()
        {
        }

        public SearchTreeNodeHelper(SearchTreeNode srNode, CarType ctType)
        {
            this.SearchTreeNodeID = srNode.ID;
            this.ParentSearchTreeNodeID = srNode.ParentNodeID;
            this.Text = srNode.Name.Tex_Text;

            this.CarTypeID = ctType.ID;
            this.ModelID = ctType.TYP_MOD_ID;
        }
    }
}
