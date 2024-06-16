using System.Text;

namespace CAT
{
    internal static class KSI
    {
        internal static int ReadFrequency(string portName, int timeout, string readCommand, string readReplyPrefix, string readReplySufix)
        {
            byte[] byteCommand = Encoding.ASCII.GetBytes(readCommand);
            byte[] buffer = Serial.Control.WriteReadToPort(portName, byteCommand, timeout);
            string bufferString = Encoding.UTF8.GetString(buffer);
            //Log.Debug(bufferString);
            string frequencyString = FilterBuffer(bufferString, readCommand, readReplyPrefix, readReplySufix);
            return int.Parse(frequencyString);
        }

        internal static void WriteFrequency(string portName, int timeout, string writeCommand, string writeReplyPrefix, string writeReplySufix, int currentFrequency)
        {
            byte[] byteCommand = Encoding.ASCII.GetBytes(BuildCommand(writeCommand, currentFrequency));
            Serial.Control.WriteToPort(portName, byteCommand, timeout);
        }

        private static string FilterBuffer(string bufferString, string command, string replyPrefix, string replySufix)
        {
            bufferString = bufferString.Replace(command, string.Empty);
            int startIndex = bufferString.IndexOf(replyPrefix, StringComparison.Ordinal);
            startIndex += replyPrefix.Length;
            int endIndex = bufferString.IndexOf(replySufix, startIndex, StringComparison.Ordinal);
            return bufferString.Substring(startIndex, endIndex - startIndex);
        }

        private static string BuildCommand(string updateFrequency, int currentFrequency)
        {
            string currentFrequencyString = currentFrequency.ToString().PadLeft(11, '0');
            string frequencyStrintPrefix = updateFrequency.Substring(0, 2);
            string frequencyStrintSufix = updateFrequency.Substring(13, 1);
            string commandString = frequencyStrintPrefix + currentFrequencyString + frequencyStrintSufix;
            return commandString;
        }
    }
}