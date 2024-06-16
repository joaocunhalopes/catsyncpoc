using Util;

namespace CAT
{
    public static class Control
    {
        public static int ReadFrequency(string portName, string protocol, int timout, string readCommand, string readReplyPrefix, string readReplySufix)
        {
            int frequency = 0;
            switch (protocol)
            {
                case "KSI":
                    return (KSI.ReadFrequency(portName, timout, readCommand, readReplyPrefix, readReplySufix));
                case "CIV":
                    return (CIV.ReadFrequency(portName, timout, readCommand, readReplyPrefix, readReplySufix));
                default:
                    Log.Warning("Unknown protocol in CAT Read Frequency.");
                    return frequency;
            }
        }

        public static void WriteFrequency(string portName, string protocol, int timeout, string writeCommand, string writeReplyPrefix, string writeReplySufix, int currentFrequency)
        {
            switch (protocol)
            {
                case "KSI":
                    KSI.WriteFrequency(portName, timeout, writeCommand, writeReplyPrefix, writeReplySufix, currentFrequency);
                    break;
                case "CIV":
                    CIV.WriteFrequency(portName, timeout, writeCommand, writeReplyPrefix, writeReplySufix, currentFrequency);
                    break;
                default:
                    Log.Warning("Unknown protocol in CAT Read Frequency.");
                    break;
            }
        }
    }
}