﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class ChangePasswordModel
    {
        [Required,DataType(DataType.Password),Display(Name = "Current password")]
        public string CurrentPassword { get; set; }
        [Required,DataType(DataType.Password),Display(Name = "New password")]
        public string NewPassword { get; set; }
        [Required,DataType(DataType.Password),Display(Name = "Confirm new password")]
        [Compare("NewPassword",ErrorMessage ="Confirm password does not match with new password..!")]
        public string ConfirmPassword { get; set; }
    }
}
