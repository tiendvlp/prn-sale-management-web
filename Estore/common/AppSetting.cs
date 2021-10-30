using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.common
{
    public class AdminAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class AppSetting
    {
        public List<AdminAccount> Admins { get; set; }
    }
}