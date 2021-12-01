using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public enum ImportanceLevel
    {
        Spam,
        Debug,
        Warning,
        Stats,
        Error,
        FatalError,
        Ignore
    }
}
