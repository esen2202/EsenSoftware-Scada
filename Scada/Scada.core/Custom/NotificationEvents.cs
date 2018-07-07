using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.core
{
    public delegate void NotificationEventDelegate(string Title, string Message, core.StatusColor Status, int Second);
    public class NotificationEvents
    {
    }
}
