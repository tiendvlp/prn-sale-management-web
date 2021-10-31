using System;
using Estore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.ViewModels.Member
{
    public class CreateMemberViewModel
    {
        [BindProperty]
        public MemberViewModel Member { get; set; }
        public string ErrorMessage { get; set; }

        public CreateMemberViewModel(MemberViewModel member)
        {
            Member = member;
        }


        public CreateMemberViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public CreateMemberViewModel()
        {
        }
    }
}
