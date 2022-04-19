using RMS.PromoPlanner.Dtos;
using System.Collections.Generic;

namespace RMS.Web.Areas.App.Models.Products
{
    public class ProductSelectionViewModel
    {
        public bool IsEditMode { get; set; }
        public IEnumerable<CustomProductForView> Products { get; set; }
    }
}
