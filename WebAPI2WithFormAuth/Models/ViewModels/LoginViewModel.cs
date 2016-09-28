using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2WithFormAuth.Models.ViewModels
{
    public class LoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}