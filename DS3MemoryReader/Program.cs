using System;
using System.Text;
using System.Threading;

namespace DS3MemoryReader
{
    class Program
    {
        static void Main(string[] args) {
            DS3ProcessInfo processInfo = new DS3ProcessInfo(); // Handles attaching/detaching from the main Dark Souls III process
            StringBuilder sb = new StringBuilder();
            Console.CursorVisible = false;

            // Base addresses for lots of information in DS3
            int baseA = 0x4740178;
            int baseB = 0x4768E78;
            DS3MemoryAddress coordinatesBase = new DS3MemoryAddress(baseB, new int[] { 0x40, 0x28 }); // Common pointer for coordinates

            // Define values we want to inspect
            var playerAngle = new DS3MemoryValue<float>(processInfo, coordinatesBase.AddOffset(0x74));
            var playerX = new DS3MemoryValue<float>(processInfo, coordinatesBase.AddOffset(0x80));
            var playerZ = new DS3MemoryValue<float>(processInfo, coordinatesBase.AddOffset(0x84));
            var playerY = new DS3MemoryValue<float>(processInfo, coordinatesBase.AddOffset(0x88));

            var name = new DS3MemoryValue<string>(processInfo, new DS3MemoryAddress(baseA, new int[] { 0x10, 0x88 }));
            var onlineArea = new DS3MemoryValue<int>(processInfo, new DS3MemoryAddress(baseB, new int[] { 0x80, 0x1ABC}));

            // Track console state so we can clear it if there would be visual artifacts
            int consoleState = 0;
            int previousConsoleState = consoleState;
            
            // Print inspected values
            while (true) {
                sb.Clear();

                processInfo.FindDS3Process();
                if (!processInfo.IsValid) {
                    // The process is invalid
                    consoleState = 1;
                    sb.AppendLine("No DS3 process found");
                } else {
                    // The process is valid
                    sb.AppendLine($"Attached to process {processInfo.Process?.Id}");

                    if (IsNearlyZero(playerX.Value) && IsNearlyZero(playerY.Value) && IsNearlyZero(playerZ.Value) && IsNearlyZero(playerAngle.Value)) {
                        // Generally we can assume the player has died or is in the middle of travelling if their coordinates are zeroed out
                        consoleState = 2;
                        sb.AppendLine("No player found");
                    } else {
                        // We have a valid player, print its coordinates
                        consoleState = 3;
                        sb.AppendLine("Player found");
                        sb.AppendLine();
                        sb.AppendLine($"Character Name: {name}");
                        sb.AppendLine($"X: {playerX.Value:0.000000}");
                        sb.AppendLine($"Y: {playerY.Value:0.000000}");
                        sb.AppendLine($"Z: {playerZ.Value:0.000000}");
                        sb.AppendLine($"Angle: {playerAngle.Value:0.000000}");

                        string onlineAreaDisplayValue = "Unknown";
                        if (DS3OnlineAreas.IdToName.TryGetValue(onlineArea.Value, out string area)) {
                            onlineAreaDisplayValue = area;
                        }
                        
                        sb.AppendLine($"Online Area: {onlineAreaDisplayValue} ({onlineArea})                    "); // spaces to clear out unwanted characters
                    }
                }

                // Clear console if necessary
                if (consoleState != previousConsoleState) {
                    previousConsoleState = consoleState;
                    Console.Clear();
                }

                // Reset cursor to top left and print output
                Console.SetCursorPosition(0, 0);
                Console.Write(sb);
                
                Thread.Sleep(16); // 60hz
            }
        }

        static bool IsNearlyZero(float value) {
            return value < 0.001f;
        }
    }
}
