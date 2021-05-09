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
        const int baseA = 0x4740178;
        const int baseB = 0x4768E78;
        const int gameFlagData = 0x473BE28;

        Dictionary<string, DS3MemoryValue> valuesToInspect;
        Dictionary<string, DS3MemoryValueBoolFlag> flagsToInspect;

        public void Test() {
            Setup();

            while (true) {
                processInfo.FindDS3Process();

                if (processInfo.IsValid) {
                    // Regenerate addresses for the values we want to get
                    DS3MemoryValue.RegenerateAddresses(flagsToInspect.Values.ToArray());

                    Console.WriteLine(DateTime.Now);
                    foreach (var flag in flagsToInspect) {
                        Console.WriteLine($"{flag.Key} -> {flag.Value.Value}");
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        public void Start() {
            Setup();

            // Listen for requests from the frontend
            var ipc = new IPC(HandleMessage);
            ipc.Listen();
        }

        private void Setup() {
            // Will automatically find the process for us
            processInfo = new DS3ProcessInfo();

            DS3MemoryAddress coordinatesBase = new DS3MemoryAddress(baseB, 0x40, 0x28); // Common pointer for coordinates

            // Define values we want to inspect
            valuesToInspect = new Dictionary<string, DS3MemoryValue>() {
                ["Player.Angle"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x74), DS3AddressUpdateType.Manual),
                ["Player.X"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x80), DS3AddressUpdateType.Manual),
                ["Player.Z"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x84), DS3AddressUpdateType.Manual),
                ["Player.Y"] = new DS3MemoryValueFloat(processInfo, coordinatesBase.AddOffset(0x88), DS3AddressUpdateType.Manual),
                ["Player.OnlineArea"] = new DS3MemoryValueInt32(processInfo, new DS3MemoryAddress(baseB, 0x80, 0x1ABC), DS3AddressUpdateType.Manual)
            };

            flagsToInspect = new Dictionary<string, DS3MemoryValueBoolFlag>() {
                //["Bosses.IudexGundyr.PulledSwordOut"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A67), 5),
                //["Bosses.IudexGundyr.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A67), 6),
                ["Bosses.IudexGundyr.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A67), 7),
                //["Bosses.Vordt.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF67), 6),
                ["Bosses.Vordt.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF67), 7),
                //["Bosses.CurseRottedGreatwood.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1967), 6),
                ["Bosses.CurseRottedGreatwood.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1967), 7),
                //["Bosses.CrystalSage.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D69), 3),
                ["Bosses.CrystalSage.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D69), 5),
                //["Bosses.Deacons.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3C67), 6),
                ["Bosses.Deacons.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3C67), 7),
                //["Bosses.AbyssWatchers.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D67), 6),
                ["Bosses.AbyssWatchers.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D67), 7),
                //["Bosses.Wolnir.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5067), 6),
                ["Bosses.Wolnir.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5067), 7),
                ["Bosses.OldDemonKing.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5064), 1),
                //["Bosses.YhormTheGiant.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5567), 6),
                ["Bosses.YhormTheGiant.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5567), 7),
                ["Bosses.Pontiff.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B69), 5),
                ["Bosses.Aldrich.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B67), 7),
                ["Bosses.Dancer.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF6C), 5),
                //["Bosses.Oceiros.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF6B), 1),
                ["Bosses.Oceiros.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF64), 1),
                //["Bosses.ChampionGundyr.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A64), 1), // Is this correct?
                ["Bosses.ChampionGundyr.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A64), 0),
                ["Bosses.NamelessKing.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2369), 5),
                ["Bosses.DragonslayerArmour.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1467), 7),
                //["Bosses.TwinPrinces.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3764), 0),
                ["Bosses.TwinPrinces.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3764), 1),
                //["Bosses.SoulOfCinder.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5F67), 6),
                ["Bosses.SoulOfCinder.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5F67), 7),
                //["Bosses.ChampionsGravetender.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6468), 2),
                ["Bosses.ChampionsGravetender.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6468), 3),
                //["Bosses.Friede.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6467), 6),
                ["Bosses.Friede.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6467), 7),
                //["Bosses.Halflight.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7867), 6),
                ["Bosses.Halflight.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7867), 7),
                //["Bosses.Midir.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7869), 4),
                ["Bosses.Midir.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7869), 5),
                //["Bosses.Gael.Encountered"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7D67), 6),
                ["Bosses.Gael.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7D67), 7),
                ["Bosses.DemonPrince.Defeated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7367), 7),
                // TODO: Missing Ancient Wyvern?

                // Doors and Shortcuts
                ["Doors.PostIudexGundyr.Opened"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x24923), 3), // Gates to Firelink Shrine?
                ["Doors.ArchivesStart.Opened"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x22631), 7), // Grand Archives Main Door?
                ["Doors.PreTwinPrinces.Opened"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x22637), 3), // Twin Princes? Or is this the elevator

                // Elevators
                ["Elevators.Pontiff.Activated"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x23A3A), 3), // Pontiff's Shortcut?

                // Misc
                ["Misc.CoiledSword.Embedded"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A0F), 2),

                // Bonfires
                ["Bonfires.CemeteryOfAsh.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A03), 6),
                ["Bonfires.IudexGundyr.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A03), 5),
                ["Bonfires.FirelinkShrine.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A03), 7),
                ["Bonfires.UntendedGraves.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A03), 4),
                ["Bonfires.ChampionGundyr.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5A03), 3),
                ["Bonfires.HighWallOfLothric.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF02), 6),
                ["Bonfires.TowerOnTheWall.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF03), 2),
                ["Bonfires.VordtOfTheBorealValley.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF03), 5),
                ["Bonfires.DancerOfTheBorealValley.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF03), 3),
                ["Bonfires.OceirosTheConsumedKing.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0xF03), 6),
                ["Bonfires.FootOfTheHighWall.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1903), 3),
                ["Bonfires.UndeadSettlement.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1903), 7),
                ["Bonfires.DilapidatedBridge.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1903), 4),
                ["Bonfires.CliffUnderside.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1903), 5),
                ["Bonfires.RoadOfSacrifices.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D03), 1),
                ["Bonfires.HalfwayFortress.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D03), 7),
                ["Bonfires.CrucifixionWoods.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D03), 0),
                ["Bonfires.FarronKeep.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D03), 4),
                ["Bonfires.KeepRuins.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D03), 3),
                ["Bonfires.OldWolfOfFarron.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2D03), 2),
                ["Bonfires.CathedralOfTheDeep.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3C03), 4),
                ["Bonfires.CleansingChapel.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3C03), 7),
                ["Bonfires.RosariasBedChamber.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3C03), 5),
                ["Bonfires.CatacombsOfCarthus.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5003), 1),
                ["Bonfires.AbandonedTomb.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5003), 6),
                ["Bonfires.DemonRuins.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5003), 4),
                ["Bonfires.OldKingsAntechamber.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5003), 5),
                ["Bonfires.IrithyllOfTheBorealValley.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B03), 0),
                ["Bonfires.CentralIrithyll.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B03), 3),
                ["Bonfires.ChurchOfYorshka.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B03), 7),
                ["Bonfires.DistantManor.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B03), 2),
                ["Bonfires.WaterReserve.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B03), 1),
                ["Bonfires.AnorLondo.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B03), 4),
                ["Bonfires.PrisonTower.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x4B02), 7),
                ["Bonfires.IrithyllDungeon.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5503), 7),
                ["Bonfires.ProfanedCapital.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5503), 5),
                ["Bonfires.ArchdragonPeak.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2303), 7),
                ["Bonfires.DragonkinMausoleum.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2303), 4),
                ["Bonfires.GreatBelfry.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x2303), 5),
                ["Bonfires.LothricCastle.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1403), 7),
                ["Bonfires.DragonBarracks.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x1403), 5),
                ["Bonfires.GrandArchives.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x3703), 6),
                ["Bonfires.FlamelessShrine.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5F03), 7),
                ["Bonfires.KilnOfTheFirstFlame.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x5F03), 6),
                ["Bonfires.Snowfield.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6403), 6),
                ["Bonfires.RopeBridgeCave.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6403), 5),
                ["Bonfires.AriandelChapel.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6403), 2),
                ["Bonfires.CorvianSettlement.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6403), 4),
                ["Bonfires.SnowyMountainPass.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6403), 3),
                ["Bonfires.DepthsOfThePainting.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x6403), 0),
                ["Bonfires.TheDregHeap.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7303), 6),
                ["Bonfires.EarthenPeakRuins.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7303), 5),
                ["Bonfires.WithinTheEarthenPeakRuins.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7303), 4),
                ["Bonfires.TheDemonPrince.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7303), 7),
                ["Bonfires.MausoleumLookout.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7803), 5),
                ["Bonfires.RingedInnerWall.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7803), 4),
                ["Bonfires.RingedCityStreets.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7803), 3),
                ["Bonfires.SharedGrave.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7803), 2),
                ["Bonfires.ChurchOfFilianore.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7803), 7),
                ["Bonfires.DarkeaterMidir.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7803), 6),
                ["Bonfires.FilianoresRest.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7D03), 6),
                ["Bonfires.SlaveKnightGael.Lit"] = new DS3MemoryValueBoolFlag(processInfo, new DS3MemoryAddress(gameFlagData, 0, 0x7D03), 7),
            };
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

                if ((valuesToInspect["Player.OnlineArea"] as DS3MemoryValueInt32).Value != 0) {
                    foreach (var kvp in valuesToInspect) {
                        SetExpandoProperty(returnValue, kvp.Key, kvp.Value.GetValueGeneric());
                    }
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
    }
}
