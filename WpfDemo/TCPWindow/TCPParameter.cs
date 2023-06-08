using Common.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPWindow
{
    public class TCPParameter : ParameterBase
    {
        private string ip = "127.0.0.1";
        private int port = 8080;
        private bool isCheckCR = false;
        private bool isCheckLF = false;

        public string IP
        {
            get { return ip; }
            set
            {
                ip = value;
                OnPropertyChanged(nameof(IP));
            }
        }
        public int Port
        {
            get { return port; }
            set
            {
                port = value;
                OnPropertyChanged(nameof(port));
            }
        }
        public bool IsCheckCR
        {
            get { return isCheckCR; }
            set
            {
                isCheckCR = value;
                OnPropertyChanged(nameof(IsCheckCR));
            }
        }
        public bool IsCheckLF
        {
            get { return isCheckLF; }
            set
            {
                isCheckLF = value;
                OnPropertyChanged(nameof(IsCheckLF));
            }
        }

        public override void Copy(IParameter source)
        {
            var tmp = source as TCPParameter;
            if (tmp == null)
                return;

            this.IP = tmp.IP;
            this.Port = tmp.Port;
            this.IsCheckCR = tmp.IsCheckCR;
            this.IsCheckLF = tmp.IsCheckLF;
        }
    }
}
