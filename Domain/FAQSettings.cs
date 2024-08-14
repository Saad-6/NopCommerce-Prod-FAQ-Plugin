using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.F.A.Q.Domain;
public class FAQSettings : ISettings
{
    public bool AllowAnonymousUsersToAskFAQs { get; set; }
    public FAQSettings()
    {
        AllowAnonymousUsersToAskFAQs = true;
    }
}
