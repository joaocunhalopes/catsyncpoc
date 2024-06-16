using System.IO.Ports;

namespace Serial
{
    public static class Control
    {
        public static List<SerialPort> SerialPortList { get; set; } = new();

        public static SerialPort OpenPort(string portName, int baudRate, string parity, int dataBits, string stopBits, string handshake)
        {
            SerialPort port = new SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = Enum.Parse<Parity>(parity, true),
                DataBits = dataBits,
                StopBits = Enum.Parse<StopBits>(stopBits, true),
                Handshake = Enum.Parse<Handshake>(handshake, true)
            };
            Port.Open(port);
            SerialPortList.Add(port);
            return port;
        }

        public static void WriteToPort(string portName, byte[] data, int timeout)
        {
            var port = SerialPortList.FirstOrDefault(port => port.PortName == portName);
            if (port == null)
                return;
            Port.Write(port, data, timeout);
        }

        public static byte[] WriteReadToPort(string portName, byte[] data, int timeout)
        {
            byte[] buffer = Array.Empty<byte>();
            var port = SerialPortList.FirstOrDefault(port => port.PortName == portName);
            if (port == null)
                return buffer;
            else
            {
                buffer = Port.WriteRead(port, data, timeout);
                return buffer;
            }
        }

        public static void ClosePorts()
        {
            foreach (var port in SerialPortList)
            {
                Port.Close(port);
            }
        }
    }
}