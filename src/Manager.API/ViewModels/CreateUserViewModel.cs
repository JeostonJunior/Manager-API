﻿using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "The Name cannot be null")]
        [StringLength(80, MinimumLength =3, ErrorMessage = "Invalid Name length")]        
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email cannot be null")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = "Invalid Email length")]       
        [RegularExpression(@"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "The Password cannot be null")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Invalid Password length")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
