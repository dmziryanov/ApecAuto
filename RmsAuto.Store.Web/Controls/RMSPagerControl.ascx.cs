using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Controls
{
    public partial class RMSPagerControl : System.Web.UI.UserControl
    {
        private int _maxIndex = 1;

        public int MaxIndex
        {
            get { return _maxIndex; }
            set { _maxIndex = value; }
        }
        private int _minIndex = 1;
        private int _currentIndex;

        public int MinIndex
        {
            get { return _minIndex; }
            set { _minIndex = value; }
        }

        
		public int CurrentIndex
        {
            get
			{
                if (Convert.ToInt32(hfCurrentPageIndex.Value) < MinIndex) { hfCurrentPageIndex.Value = MinIndex.ToString(); }
                else if (Convert.ToInt32(hfCurrentPageIndex.Value) > MaxIndex) { hfCurrentPageIndex.Value = MaxIndex.ToString();}
                    
                return Convert.ToInt32(hfCurrentPageIndex.Value); 
            }
            
            set
			{
                if (value < MinIndex) hfCurrentPageIndex.Value = MinIndex.ToString();
                else if (value > MaxIndex) hfCurrentPageIndex.Value = MaxIndex.ToString();

                hfCurrentPageIndex.Value = value.ToString();
            }
        }

        private List<Control> List = new List<Control>();

        protected void Page_Load(object sender, EventArgs e)
        {
			
        }

        //TODO а не лучше ли (да стопудово лучше) перегрузить имеющееся событие RenderControl ?
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (!Visible) return;

            Label Current = new Label();
            Label Lab;
            LinkButton l;
            Current.Text = CurrentIndex.ToString();
            Current.CssClass = "current";

            List.Add(Current);

            for (int i = CurrentIndex - 1; i >= 1; i--)
            {
                if (CurrentIndex - i == 1 && i > 1)
                {
                    l = new LinkButton();
                    l.Text = (CurrentIndex - 1).ToString();
                    l.CssClass = "PagerItem";
                    List.Insert(0, l);
                }

                if (CurrentIndex - i == 2 && CurrentIndex - 1 > 1 && i > 1)
                {
                    Lab = new Label();
                    Lab.Text = "...";
                    List.Insert(0, Lab);
                }

                //Добавляем первый элемент
                if (i == 1)
                {
                    l = new LinkButton();
                    l.CssClass = "PagerItem";
                    l.Text = "1";
                    List.Insert(0, l);
                }
            }

            for (int i = CurrentIndex + 1; i <= MaxIndex; i++)
            {
                if (i - CurrentIndex == 1 && i < MaxIndex)
                {
                    l = new LinkButton();
                    l.CssClass = "PagerItem";
              
                    l.Text = (i).ToString();
                    List.Add(l);
                }

                if (i - CurrentIndex == 2 && MaxIndex - CurrentIndex > 1 && i < MaxIndex)
                {
                    Lab = new Label();
                    Lab.Text = "...";
                    List.Add(Lab);
                }

                if (i == MaxIndex)
                {
                    l = new LinkButton();
                    l.CssClass = "PagerItem";
                    l.Text = MaxIndex.ToString();
                    List.Add(l);
                }

            }

            ImageButton m = new ImageButton();
            m.ImageUrl = "/images/Prev.png";
            m.Visible = true;
            m.CssClass = "arrow_page_left";
            List.Insert(0, m);

       
            m = new ImageButton();
            m.ImageUrl = "/images/Next.png";
            m.Visible = true;
            m.CssClass = "arrow_page_right";
            List.Add(m);

            //Добавляем контролы из листа       
            for (int i = 0; i < List.Count -1; i++)
            {
                List[i].ID = i.ToString();
                MainItemsPlaceHolder.Controls.Add(List[i]);
                List[i].Visible = true;
            }

            //Последний итем добавляем отдельно так как нужно присвоить максимально возможный ИД, 
            //чтобы на месте ИД имажбаттон не оказался ИД линк баттон.
            MainItemsPlaceHolder.Controls.Add(List[List.Count - 1]);
            List[List.Count - 1].ID = (MaxIndex+1).ToString();
            this.RenderChildren(writer);
       }


    }
}