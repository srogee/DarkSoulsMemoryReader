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
        GetPlayerInformation = 0
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
        const int baseA = 0x4740178;
        const int baseB = 0x4768E78;

        Dictionary<string, DS3MemoryValue> valuesToInspect;

        public void Start() {
            // Will automatically find the process for us
            processInfo = new DS3ProcessInfo();

            DS3MemoryAddress coordinatesBase = new DS3MemoryAddress(baseB, 0x40, 0x28); // Common pointer for coordinates

            // Define values we want to inspect
            valuesToInspect = new Dictionary<string, DS3MemoryValue>() {
                ["PlayerAngle"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x74), DS3AddressUpdateType.Manual),
                ["PlayerX"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x80), DS3AddressUpdateType.Manual),
                ["PlayerZ"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x84), DS3AddressUpdateType.Manual),
                ["PlayerY"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x88), DS3AddressUpdateType.Manual),
                ["PlayerOnlineArea"] = new DS3MemoryValueInt32(processInfo, new DS3MemoryAddress(baseB, 0x80, 0x1ABC), DS3AddressUpdateType.Manual)
            };

            // Listen for requests from the frontend
            var ipc = new IPC(HandleMessage);
            ipc.Listen();
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue) {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName)) {
                expandoDict[propertyName] = propertyValue;
            } else {
                expandoDict.Add(propertyName, propertyValue);
            }
        }

        // Handles messages from the frontend
        dynamic HandleMessage(dynamic payload) {
            IPCRequestType requestType = (IPCRequestType)payload.Type;
            return requestType switch {
                IPCRequestType.GetPlayerInformation => Message_GetPlayerInformation(),
                _ => throw new NotImplementedException()
            };
        }

        // Gets information about the player
        dynamic Message_GetPlayerInformation() {
            dynamic returnValue = new ExpandoObject();

            processInfo.FindDS3Process();

            returnValue.HasValidProcess = processInfo.IsValid;
            if (processInfo.IsValid) {
                // Regenerate addresses for the values we want to get
                DS3MemoryValue.RegenerateAddresses(valuesToInspect.Values.ToArray());

                returnValue.HasValidPlayer = (valuesToInspect["PlayerOnlineArea"] as DS3MemoryValueInt32).Value != 0;
                foreach (var kvp in valuesToInspect) {
                    AddProperty(returnValue, kvp.Key, kvp.Value.GetValueGeneric());
                }
            }

            return returnValue;
        }
    }
}
