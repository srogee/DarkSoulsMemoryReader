using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace DS3MemoryReader
{
    class DS3MemoryValueTracker
    {
        private List<Tuple<string, DS3MemoryValue>> values;
        private bool wroteColumnHeaders = false;
        private string filename;
        private string[] previouslyWrittenValues;
        private bool onlyWriteChanges;

        public DS3MemoryValueTracker(string filename, bool onlyWriteChanges = true) {
            values = new List<Tuple<string, DS3MemoryValue>>();
            this.filename = filename;

            previouslyWrittenValues = null;
            this.onlyWriteChanges = onlyWriteChanges;
        }

        public void TrackValue(string name, DS3MemoryValue value) {
            if (wroteColumnHeaders) {
                throw new Exception("Adding values after data has already been saved is not supported");
            }

            values.Add(new Tuple<string, DS3MemoryValue>(name, value));
        }

        public void SaveValues() {
            if (!wroteColumnHeaders) {
                using (StreamWriter sw = File.CreateText(filename)) {
                    var headersToWrite = values.Select(tuple => EscapeStrForCSV(tuple.Item1));
                    headersToWrite = headersToWrite.Prepend("Unix Timestamp (UTC)");
                    sw.WriteLine(string.Join(",", headersToWrite));
                }
                wroteColumnHeaders = true;
            }

            var valuesToWrite = values.Select(tuple => EscapeStrForCSV(tuple.Item2.ToString()));

            if (onlyWriteChanges && previouslyWrittenValues != null) {
                if (Enumerable.SequenceEqual(previouslyWrittenValues, valuesToWrite)) {
                    return; // Exit early if nothing has changed
                }
            }

            using (StreamWriter sw = File.AppendText(filename)) {
                if (onlyWriteChanges) {
                    // Write values as "" if they haven't changed
                    string[] valuesArray = valuesToWrite.ToArray();

                    if (previouslyWrittenValues == null) {
                        previouslyWrittenValues = valuesArray;
                    } else {
                        for (int i = 0; i < valuesArray.Length; i++) {
                            if (valuesArray[i] == previouslyWrittenValues[i]) {
                                // If the value has not changed, write it as the empty string
                                valuesArray[i] = string.Empty;
                            } else {
                                // If the value has changed, update it in previouslyWrittenValues
                                previouslyWrittenValues[i] = valuesArray[i];
                            }
                        }
                    }

                    valuesToWrite = valuesArray;
                }

                valuesToWrite = valuesToWrite.Prepend(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
                sw.WriteLine(string.Join(",", valuesToWrite));
            }
        }

        private static string EscapeStrForCSV(string value) {
            var mustQuote = value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n");

            if (!mustQuote) {
                return value;
            }

            value = value.Replace("\"", "\"\"");

            return string.Format("\"{0}\"", value);
        }
    }
}
