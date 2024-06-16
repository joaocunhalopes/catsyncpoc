namespace CAT
{
    internal static class CIV
    {
        internal static int ReadFrequency(string portName, int timeout, string readCommand, string readReplyPrefix, string readReplySufix)
        {
            byte[] byteCommand = HexStringToByteArray(readCommand);
            byte[] buffer = Serial.Control.WriteReadToPort(portName, byteCommand, timeout);
            string bufferString = ByteArrayToHexString(buffer);
            //Log.Debug(bufferString);
            string frequencyString = FilterBuffer(bufferString, readCommand, readReplyPrefix, readReplySufix);
            return int.Parse(DecodeFrequency(frequencyString));
        }

        internal static void WriteFrequency(string portName, int timeout, string writeCommand, string writeReplyPrefix, string writeReplySufix, int currentFrequency)
        {
            byte[] byteCommand = HexStringToByteArray(BuildCommand(writeCommand, currentFrequency));
            Serial.Control.WriteToPort(portName, byteCommand, timeout);
        }

        private static byte[] HexStringToByteArray(string command)
        {
            int numberOfChars = command.Length;
            byte[] byteCommand = new byte[numberOfChars / 2];
            for (int i = 0; i < numberOfChars; i += 2)
            {
                byteCommand[i / 2] = Convert.ToByte(command.Substring(i, 2), 16);
            }
            return byteCommand;
        }

        private static string ByteArrayToHexString(byte[] byteArray)
        {
            char[] hex = new char[byteArray.Length * 2];
            int index = 0;
            foreach (byte b in byteArray)
            {
                hex[index++] = GetHexChar(b >> 4);
                hex[index++] = GetHexChar(b & 0x0F);
            }
            return new string(hex);
        }

        private static char GetHexChar(int value)
        {
            return (char)(value < 10 ? value + '0' : value - 10 + 'A');
        }

        private static string FilterBuffer(string bufferString, string command, string replyPrefix, string replySufix)
        {
            bufferString = bufferString.Replace(command, string.Empty);
            int startIndex = bufferString.IndexOf(replyPrefix, StringComparison.Ordinal);
            startIndex += replyPrefix.Length;
            int endIndex = bufferString.IndexOf(replySufix, startIndex, StringComparison.Ordinal);
            return bufferString.Substring(startIndex, endIndex - startIndex);
        }

        private static string DecodeFrequency(string frequency)
        {
            return new string(new char[]
            {
                frequency[8], // 1 GHz
                frequency[9], // 100 MHz
                frequency[6], // 10 MHz
                frequency[7], // 1 MHz
                frequency[4], // 100 kHz
                frequency[5], // 10 kHz
                frequency[2], // 1 kHz
                frequency[3], // 100 Hz
                frequency[0], // 10 Hz
                frequency[1]  // 1 Hz
            });
        }

        private static string BuildCommand(string updateFrequency, int currentFrequency)
        {
            string currentFrequencyString = currentFrequency.ToString().PadLeft(10, '0');
            string frequencyStrintPrefix = updateFrequency.Substring(0, 12);
            string frequencyStrintSufix = updateFrequency.Substring(22, 2);
            string commandString = frequencyStrintPrefix + DecodeFrequency(currentFrequencyString) + frequencyStrintSufix;
            return commandString;
        }
    }
}