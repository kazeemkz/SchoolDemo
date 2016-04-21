using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class SupplierOrder
    {
        public int SupplierOrderID { get; set; }

        public int TheSupplierID { get; set; }

        public string TheStoreItemIDs { get; set; }

        [NotMapped]
        public List<Store> TheStoreItems { get; set; }

        [Display(Name = "Amount Owing Supplier")]
        public decimal? AmountOwingSupplier { get; set; }

        [Display(Name = "Amount Paid Supplier")]
        public decimal? AmountPaidSupplier { get; set; }

        public bool Approved { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? DateLogged { get; set; }

        public string LoggedBy { get; set; }
        public string ApprovedBy { get; set; }

    }
}
