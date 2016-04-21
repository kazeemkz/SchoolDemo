using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Store
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StoreID { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Required]
        public int Quanity { get; set; }
        [Display(Name = "Author Name")]
        public string AuthorName { get; set; }
        public string Size { get; set; }
        public string Level { get; set; }

        public string CreatedUserName { get; set; }
        public string EditedUserName { get; set; }
        public string DeletedUserName { get; set; }
        public DateTime DateEntered { get; set; }
        // [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        //  public string kazo { get; set; }

        // [NotMapped]
        // [Display(Name = "Quantity Needed")]
        [Range(0, 1000)]
        public int QuantityNeeded { get; set; }

        [NotMapped]
        [Display(Name = "Total Cost")]
        public decimal TotalCost
        {
            get { return (this.Amount * this.Quanity); }
        }


    }
}
