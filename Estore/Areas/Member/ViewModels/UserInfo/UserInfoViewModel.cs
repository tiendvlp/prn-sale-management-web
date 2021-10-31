using System;
using Estore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Member.ViewModels.UserInfo
{
    public class UserInfoViewModel
    {
        [BindProperty]
        public MemberViewModel Member { get; set; }
        public string ErrorMessage { get; set; }

        public UserInfoViewModel(MemberViewModel member)
        {
            Member = member;
        }


        public UserInfoViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public UserInfoViewModel()
        {
        }
    }
}
