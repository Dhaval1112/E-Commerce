using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class ResetPaswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required,Compare("NewPassword",ErrorMessage ="Both password should be same..!"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsSuccess { get; set; }
    }
}
