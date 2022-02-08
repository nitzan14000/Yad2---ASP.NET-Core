using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class CustomDateAttribute : RangeAttribute
    {
        public CustomDateAttribute()
          : base(typeof(DateTime),//ages at least 5 years old to 85
                  DateTime.Now.AddYears(-85).ToShortDateString(),
                  DateTime.Now.AddYears(-5).ToShortDateString())
        { }
    }

    public class UserModel
    {
        public int Id{ get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(50)]
        public string FirstName{ get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(50)]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [CustomDate(ErrorMessage = "ages at least 5 years old to 85")]
        [Required(ErrorMessage = "Required fild")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(50)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(10),MinLength(3,ErrorMessage ="not enough charecters")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required fild"), MaxLength(50)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password not the same")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}
