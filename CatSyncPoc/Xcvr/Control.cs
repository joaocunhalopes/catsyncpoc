namespace Xcvr
{
    public static class Control
    {
        public static void ReadXcvrsConfig()
        {
            Parser.ParseJson();
            Parser.LogXcvrList();
        }

        public static void OpenXcvrsPorts()
        {
            foreach (var xcvr in Parser.XcvrsList.Xcvrs)
            {
                Serial.Control.OpenPort(xcvr.PortSettings.PortName, xcvr.PortSettings.BaudRate, xcvr.PortSettings.Parity, xcvr.PortSettings.DataBits, xcvr.PortSettings.StopBits, xcvr.PortSettings.Handshake);
            }
        }

        public static void ReadXcvrsFrequency()
        {
            foreach (var xcvr in Parser.XcvrsList.Xcvrs)
            {
                int frequency = CAT.Control.ReadFrequency(xcvr.PortSettings.PortName, xcvr.Protocol, xcvr.Timeout, xcvr.Commands.Read, xcvr.Commands.ReadPrefix, xcvr.Commands.ReadSufix);
                xcvr.PreviousFrequency = xcvr.CurrentFrequency;
                xcvr.CurrentFrequency = frequency;
            }
        }

        public static void EqualizeFrequencies()
        {
            Xcvr xcvr1 = Parser.XcvrsList.Xcvrs[0];
            Xcvr xcvr2 = Parser.XcvrsList.Xcvrs[1];
            
            //Log.Debug($"{xcvr1.CurrentFrequency},{xcvr2.CurrentFrequency}");

            if (xcvr1.CurrentFrequency != xcvr2.CurrentFrequency)
            {
                if (xcvr1.CurrentFrequency != xcvr1.PreviousFrequency)
                {
                    // 1 has changed => update 2.
                    CAT.Control.WriteFrequency(xcvr2.PortSettings.PortName, xcvr2.Protocol, xcvr2.Timeout, xcvr2.Commands.Write, xcvr2.Commands.WritePrefix, xcvr2.Commands.WriteSufix, xcvr1.CurrentFrequency);
                    xcvr2.CurrentFrequency = xcvr1.CurrentFrequency;
                }
                else if (xcvr2.CurrentFrequency != xcvr2.PreviousFrequency)
                {
                    // 2 has changed => update 1.
                    CAT.Control.WriteFrequency(xcvr1.PortSettings.PortName, xcvr1.Protocol, xcvr1.Timeout, xcvr1.Commands.Write, xcvr1.Commands.WritePrefix, xcvr1.Commands.WriteSufix, xcvr2.CurrentFrequency);
                    xcvr1.CurrentFrequency = xcvr2.CurrentFrequency;
                }
            }
        }
        public static void CloseXcvrsPorts()
        {
            Serial.Control.ClosePorts();
        }
    }
}