using System.Text.Json;
using Util;

namespace Xcvr
{
    internal static class Parser
    {
        private const string XcvrRelativeFilePath = "Config\\Xcvrs.json";
        internal static XcvrsList XcvrsList = new();

        internal static void ParseJson()
        {
            try
            {
                var baseDirectory = AppContext.BaseDirectory;
                var xcvrsFilePath = Path.Combine(baseDirectory, XcvrRelativeFilePath);
                var json = File.ReadAllText(xcvrsFilePath);
                var xcrvsListDeserialized = JsonSerializer.Deserialize<XcvrsList>(json);

                if (xcrvsListDeserialized == null)
                {
                    throw new InvalidOperationException("Deserialization of Xcvrs.json resulted in a null object.");
                }
                XcvrsList = xcrvsListDeserialized;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex.Message);
                Log.Warning("File Xcvrs.json not found.");
            }
            catch (JsonException ex)
            {
                Log.Error(ex.Message);
                Log.Warning("JSON deserialization error.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning("Could not parse Xcvrs.json.");
            }
        }

        internal static void LogXcvrList()
        {
            try
            {
                foreach (var xcvr in XcvrsList.Xcvrs)
                {
                    var portSettings = xcvr.PortSettings;
                    var commands = xcvr.Commands;
                    Log.Information($"Transceiver {xcvr.Id}, Manufacturer: {xcvr.Manufacturer}, Model: {xcvr.Model}, Protocol: {xcvr.Protocol}, Timeout: {xcvr.Timeout}");
                    Log.Information($"Read Command: '{commands.Read}', Read Prefix: '{commands.ReadPrefix}', Read Sufix: '{commands.ReadSufix}'");
                    Log.Information($"Write Command: '{commands.Write}', Write Prefix: '{commands.WritePrefix}, Write Sufix: '{commands.WriteSufix}'");
                    Log.Information($"Port {portSettings.PortName}, Baudrate: {portSettings.BaudRate}, Parity: {portSettings.Parity}, DataBits: {portSettings.DataBits}, StopBits: {portSettings.StopBits}, Handshake: {portSettings.Handshake}");
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Warning("Could not log Xcvrs.json.");
            }
        }
    }
}