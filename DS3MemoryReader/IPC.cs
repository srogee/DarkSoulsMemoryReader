using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DS3MemoryReader
{
    public class IPC
    {
        private const string startKey = "@IPCMessageStart";
        private const string endKey = "@IPCMessageEnd";

        public delegate dynamic MessageHandlerDelegate(dynamic messagePayload);
        public MessageHandlerDelegate MessageHandler;

        public IPC(MessageHandlerDelegate handler) {
            MessageHandler = handler;
        }

        public void Listen() {
            Stream input = Console.OpenStandardInput();

            int bufferSize = 1024;
            byte[] buffer = new byte[bufferSize];
            int length;
            string message = "";

            while (input.CanRead && (length = input.Read(buffer, 0, bufferSize)) > 0) {
                var payload = new byte[length];
                Buffer.BlockCopy(buffer, 0, payload, 0, length);
                message += Encoding.UTF8.GetString(payload);

                while (message.StartsWith(startKey) && message.Contains(endKey)) {
                    ParseAndHandleMessage(message.Substring(startKey.Length, message.IndexOf(endKey) - startKey.Length));
                    message = message.Substring(message.IndexOf(endKey) + endKey.Length);
                }
            }
        }

        private void ParseAndHandleMessage(string messageJson) {
            dynamic message = JsonConvert.DeserializeObject<dynamic>(messageJson);
            if (message != null) {
                dynamic response = new ExpandoObject();
                response.Id = message.Id;
                response.Payload = MessageHandler(message.Payload);
                Console.Write(startKey + JsonConvert.SerializeObject(response) + endKey);
            }
        }
    }
}
