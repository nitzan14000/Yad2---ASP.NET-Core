using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Required field")]
        [Column(TypeName = "varchar(50)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required field"), MinLength(3, ErrorMessage = "Min. of 3 characters")]
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; }

    }
}
