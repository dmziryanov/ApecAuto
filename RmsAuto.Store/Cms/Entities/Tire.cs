using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace RmsAuto.Store.Cms.Entities
{
    public partial class Tires
    {
        partial void OnValidate(ChangeAction action)
        {
                using (CmsDataContext dc = new CmsDataContext())
                {
                    try
                    {
                        if (this.ManufacturerId == null)
                            this.ManufacturerId = dc.TireBrands.SingleOrDefault(m => m.Name == this.Manufacturer).Id;
                        this.Manufacturer = dc.TireBrands.SingleOrDefault(m => m.Id == this.ManufacturerId).Name;
                    }
                    catch
                    { }
                    finally
                    {
 
                    }
                }                
        }
   }
    
}
