using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;

namespace RmsAuto.Store.Web.Controls
{
    public partial class WizardTopSideBar : System.Web.UI.UserControl
    {
        public string WizardToNavigate { get; set; }

        private Wizard _currentWizard;
        protected Wizard CurrentWizard
        {
            get
            {
                if (_currentWizard == null)
                {
                    if (!String.IsNullOrEmpty(WizardToNavigate))
                    {
                        _currentWizard = Parent.FindControl(WizardToNavigate) as Wizard;
                        if (_currentWizard == null)
                        {
                            throw new ArgumentOutOfRangeException("Wizard", "Параметр WizardToNavigate должен быть идентификатором контрола типа Wizard");
                        }
                    }
                    else
                    {
                        var ctl = Parent;

                        while (!(ctl is Wizard) && !(ctl is Page) && ctl != null)
                        {
                            ctl = ctl.Parent;
                        }
                        if (ctl is Wizard)
                        {
                            _currentWizard = ctl as Wizard;
                        }
                        else
                        {
                            throw new InvalidOperationException("Или здавайте параметр WizardToNavigate, или кладите этот контрол в хидер или сайд-бар контрола типа Wizard");
                        }
                    }
                }

                return _currentWizard;
            }
        }

        public int StepNum
        {
            get
            {
                return CurrentWizard == null ? 0 : CurrentWizard.ActiveStepIndex;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CurrentWizard.ActiveStepChanged += CurrentWizard_ActiveStepChanged;

            //if (!IsPostBack)
            //{
                BindSteps();
           //}
        }

        private void BindSteps()
        {
            contents.Controls.Clear();

            for (int j = 0; j < CurrentWizard.WizardSteps.Count; j++)
            {
                var step = CurrentWizard.WizardSteps[j];
                Control ctl = new WizardTopSideBarItemContainer(step);

                //  Добавим элемент
                if (CurrentWizard.ActiveStepIndex == j &&
                    StepElementSelected != null)
                {
                    StepElementSelected.InstantiateIn(ctl);
                }
                else
                {
                    StepElement.InstantiateIn(ctl);
                }
                contents.Controls.Add(ctl);
                ctl.DataBind();

                //  Добавим сепаратор
                if (SeparatorTemplate != null &&
                    j < CurrentWizard.WizardSteps.Count - 1)
                {
                    ctl = new WizardTopSideBarItemContainer(null);
                    SeparatorTemplate.InstantiateIn(ctl);
                    contents.Controls.Add(ctl);
                    ctl.DataBind();
                }
            }

            //DataBind();
        }

        void CurrentWizard_ActiveStepChanged(object sender, EventArgs e)
        {
            BindSteps();
        }


        [TemplateContainer(typeof(WizardTopSideBarItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate StepElement { get; set; }

        [TemplateContainer(typeof(WizardTopSideBarItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate StepElementSelected { get; set; }

        [TemplateContainer(typeof(WizardTopSideBarItemContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate SeparatorTemplate { get; set; }
    }

    public class WizardTopSideBarItemContainer : Control, INamingContainer
    {
        public WizardStepBase Step
        {
            get;
            protected set;
        }

        public WizardTopSideBarItemContainer(WizardStepBase step)
        {
            Step = step;
        }

        public int StepNumber
        {
            get
            {
                if (Step == null || Step.Wizard == null)
                {
                    throw new InvalidOperationException("Set Step property right before use StepNumber property.");
                }
                return Step.Wizard.WizardSteps.IndexOf(Step) + 1;
            }
        }

        public string StepTitle
        {
            get
            {
                return Step != null ? Step.Title : String.Empty;
            }
        }
    }
}