using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DS3MemoryReader
{
    public enum IPCRequestType
    {
        GetPlayerInformation = 0,
        GetWorldFlags = 1
    }

    class Program
    {
        static void Main(string[] args) {
            DS3MemoryInspector inspector = new DS3MemoryInspector();
            inspector.Start();
        }
    }

    class DS3MemoryInspector
    {
        DS3ProcessInfo processInfo;

        // Base addresses for lots of information in DS3
        Dictionary<string, DS3MemoryValue> valuesToInspect;
        Dictionary<string, DS3MemoryValueBoolFlag> flagsToInspect;

        public void Start() {
            processInfo = new DS3ProcessInfo();

            valuesToInspect = new Dictionary<string, DS3MemoryValue>() {
                ["Player.Angle"] = new DS3MemoryValueFloat(processInfo, DS3MemoryAddress.KnownAddresses["Player.Angle"]),
                ["Player.X"] = new DS3MemoryValueFloat(processInfo, DS3MemoryAddress.KnownAddresses["Player.X"]),
                ["Player.Y"] = new DS3MemoryValueFloat(processInfo, DS3MemoryAddress.KnownAddresses["Player.Y"]),
                ["Player.Z"] = new DS3MemoryValueFloat(processInfo, DS3MemoryAddress.KnownAddresses["Player.Z"]),
                ["Player.OnlineArea"] = new DS3MemoryValueInt32(processInfo, DS3MemoryAddress.KnownAddresses["Player.OnlineArea"]),
            };

            flagsToInspect = new Dictionary<string, DS3MemoryValueBoolFlag>();
            foreach (var tuple in DS3MemoryAddress.KnownAddresses) {
                var flagId = tuple.Key;
                if (!valuesToInspect.ContainsKey(flagId)) {
                    var address = tuple.Value;
                    if (address.ExtraInfo is int startingBit) {
                        var flag = new DS3MemoryValueBoolFlag(processInfo, address, startingBit);
                        flagsToInspect.Add(flagId, flag);
                    }
                }
            }

            var ipc = new IPC(HandleMessage);
            ipc.Listen();
        }

        // Handles messages from the frontend
        private dynamic HandleMessage(dynamic payload) {
            IPCRequestType requestType = (IPCRequestType)payload.Type;
            return requestType switch {
                IPCRequestType.GetPlayerInformation => Message_GetPlayerInformation(),
                IPCRequestType.GetWorldFlags => Message_GetWorldFlags(),
                _ => throw new NotImplementedException()
            };
        }

        // Gets information about the player
        private dynamic Message_GetPlayerInformation() {
            if (VerifyProcessIsValid(out dynamic returnValue)) {
                // Regenerate addresses for the values we want to get
                DS3MemoryValue.RegenerateAddresses(valuesToInspect.Values.ToArray());
                foreach (var kvp in valuesToInspect) {
                    SetExpandoPropertyHierarchy(returnValue, kvp.Key, kvp.Value.GetValueGeneric());
                }
            }

            return returnValue;
        }

        // Gets world flag states
        private dynamic Message_GetWorldFlags() {
            if (VerifyProcessIsValid(out dynamic returnValue)) {
                // Regenerate addresses for the values we want to get
                DS3MemoryValue.RegenerateAddresses(flagsToInspect.Values.ToArray());

                foreach (var kvp in flagsToInspect) {
                    SetExpandoProperty(returnValue, kvp.Key, kvp.Value.GetValueGeneric());
                }
            }

            return returnValue;
        }

        private bool VerifyProcessIsValid(out dynamic returnValue) {
            processInfo.FindDS3Process();
            returnValue = new ExpandoObject();
            returnValue.HasValidProcess = processInfo.IsValid;
            return processInfo.IsValid;
        }

        private static void SetExpandoPropertyHierarchy(ExpandoObject expando, string propertyHierarchy, object propertyValue) {
            var pieces = propertyHierarchy.Split(".");
            ExpandoObject currentObject = expando;

            for (int i = 0; i < pieces.Length - 1; i++) {
                ExpandoObject temp = GetExpandoProperty(currentObject, pieces[i]) as ExpandoObject;

                if (temp == null) {
                    temp = new ExpandoObject();
                    SetExpandoProperty(currentObject, pieces[i], temp);
                }

                currentObject = temp;
            }

            SetExpandoProperty(currentObject, pieces[pieces.Length - 1], propertyValue);
        }

        private static void SetExpandoProperty(ExpandoObject expando, string propertyName, object propertyValue) {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName)) {
                expandoDict[propertyName] = propertyValue;
            } else {
                expandoDict.Add(propertyName, propertyValue);
            }
        }

        private static object GetExpandoProperty(ExpandoObject expando, string propertyName) {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.TryGetValue(propertyName, out object value)) {
                return value;
            }

            return null; // Unsure if necessary
        }
    }
}
