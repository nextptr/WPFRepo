using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ComPort
{

    public delegate void ReceiveDataEventHandle(object sender,object dat);
    public interface ISeriaPort
    {
        string PortName { get; set; }
        int BaudRate { get; set; }
        int DataBits { get; set; }
        Parity Parity { get; set; }
        StopBits StopBits { get; set; }
        bool IsConnected { get; set; }
        SeriaPortDevicetype Devicetype { get; set; }
        event ReceiveDataEventHandle ReceiveDataEvent;

        bool Connect();
        bool DisConnect();
        void SendCommand(string command);
        void SendCommand(byte[] command);
    }

    public enum SeriaPortDevicetype
    {
        Unknown,
        PowerMater
    }
}
