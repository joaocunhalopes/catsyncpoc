using Util;

namespace CatSyncPoc
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Xcvr.Control.ReadXcvrsConfig();
            Xcvr.Control.OpenXcvrsPorts();

            bool displayMessage = true;
            while (!Console.KeyAvailable)
            {
                Xcvr.Control.ReadXcvrsFrequency();
                Xcvr.Control.EqualizeFrequencies();
                if (displayMessage)
                {
                    Log.Information("Program running. Use your transceivers *or* press any key to exit.");
                    displayMessage = false;
                }
            }

            Xcvr.Control.CloseXcvrsPorts();
        }
    }
}