using System;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.ViewModels
{
    public class UpdateMemberViewModel
    {
        public string Id { get; set; }
        [BindProperty]
        public MemberViewModel Member { get; set; }
        public string ErrorMessage { get; set; }

        public UpdateMemberViewModel(string id, MemberViewModel member)
        {
            Member = member;
            Id = id;
        }

        public UpdateMemberViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public UpdateMemberViewModel()
        {
        }
    }
}
