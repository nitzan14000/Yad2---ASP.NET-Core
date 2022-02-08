using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Web;

namespace Entities
{
    public class ProductModel
    {
        
        public int Id{ get; set; }
        public virtual UserModel Owner { get; set; }
        public virtual UserModel User { get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(50)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(500)]
        public string ShortDescription{ get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(4000)]
        public string LongDescription{ get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "decimal(18, 3)"), Range(typeof(Decimal), "1", "1000000", ErrorMessage = "Price must be a number between {1} and {2}.")]
        public decimal Price{ get; set; }
        public byte[] Image1 { get; set; }
        public byte[] Image2 { get; set; }
        public byte[] Image3 { get; set; }
        public State ProductState { get; set; }

        public ProductModel()
        {
            Date = DateTime.Now;
            ProductState = State.UnSold;
        }
    }
    public enum State
    {
        Sold,
        UnSold,
        InCart
    }
}
