using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.wpf.Classes
{
    public class CommStatus
    {
        private bool _commOk;

        public bool CommOk
        {
            get { return _commOk; }
            set { _commOk = value; }
        }

        private string _commAddress;

        public string ComAddress
        {
            get { return _commAddress; }
            set { _commAddress = value; }
        }

        private int _commFaultCode;

        public int ComFaultCode
        {
            get { return _commFaultCode; }
            set { _commFaultCode = value; }
        }
    }
}
