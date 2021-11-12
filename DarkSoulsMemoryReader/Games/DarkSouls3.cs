using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkSoulsMemoryReader
{
    public static class DarkSouls3
    {
        public const int BaseA = 0x4740178;
        public const int BaseB = 0x4768E78;
        public const int GameFlagData = 0x473BE28;

        public static Dictionary<string, MemoryValue> GetOrCacheMemoryValues() {
            if (knownMemoryValues == null) {
                knownMemoryValues = new Dictionary<string, MemoryValue>();
                foreach (var value in RealMemoryValues) {
                    knownMemoryValues.Add(value.Key, value.Value);
                }
                foreach (var value in VirtualMemoryValues) {
                    knownMemoryValues.Add(value.Key, value.Value);
                }
            }

            return knownMemoryValues;
        }

        private static Dictionary<string, MemoryValue> knownMemoryValues;

        private static Dictionary<string, MemoryValue> RealMemoryValues = new Dictionary<string, MemoryValue>() {
            // Player data
            ["Player.Angle"] = new FloatMemoryValue(new MemoryAddress(BaseB, 0x40, 0x28, 0x74)),
            ["Player.X"] = new FloatMemoryValue(new MemoryAddress(BaseB, 0x40, 0x28, 0x80)),
            ["Player.Y"] = new FloatMemoryValue(new MemoryAddress(BaseB, 0x40, 0x28, 0x84)),
            ["Player.Z"] = new FloatMemoryValue(new MemoryAddress(BaseB, 0x40, 0x28, 0x88)),
            ["Player.OnlineArea"] = new Int32MemoryValue(new MemoryAddress(BaseB, 0x80, 0x1ABC)),

            // Bosses
            ["Bosses.IudexGundyr.PulledSwordOut"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A67), 5),
            ["Bosses.IudexGundyr.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A67), 6),
            ["Bosses.IudexGundyr.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A67), 7),
            ["Bosses.Vordt.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF67), 6),
            ["Bosses.Vordt.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF67), 7),
            ["Bosses.CurseRottedGreatwood.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1967), 6),
            ["Bosses.CurseRottedGreatwood.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1967), 7),
            ["Bosses.CrystalSage.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D69), 3),
            ["Bosses.CrystalSage.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D69), 5),
            ["Bosses.Deacons.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3C67), 6),
            ["Bosses.Deacons.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3C67), 7),
            ["Bosses.AbyssWatchers.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D67), 6),
            ["Bosses.AbyssWatchers.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D67), 7),
            ["Bosses.Wolnir.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5067), 6),
            ["Bosses.Wolnir.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5067), 7),
            ["Bosses.OldDemonKing.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5064), 1),
            ["Bosses.YhormTheGiant.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5567), 6),
            ["Bosses.YhormTheGiant.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5567), 7),
            ["Bosses.Pontiff.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B69), 5),
            ["Bosses.Aldrich.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B67), 7),
            ["Bosses.Dancer.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF6C), 5),
            ["Bosses.Oceiros.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF6B), 1),
            ["Bosses.Oceiros.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF64), 1),
            ["Bosses.ChampionGundyr.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A64), 1), // Is this correct?
            ["Bosses.ChampionGundyr.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A64), 0),
            ["Bosses.NamelessKing.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2369), 5),
            ["Bosses.DragonslayerArmour.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1467), 7),
            ["Bosses.TwinPrinces.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3764), 0),
            ["Bosses.TwinPrinces.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3764), 1),
            ["Bosses.SoulOfCinder.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5F67), 6),
            ["Bosses.SoulOfCinder.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5F67), 7),
            ["Bosses.ChampionsGravetender.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6468), 2),
            ["Bosses.ChampionsGravetender.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6468), 3),
            ["Bosses.Friede.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6467), 6),
            ["Bosses.Friede.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6467), 7),
            ["Bosses.Halflight.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7867), 6),
            ["Bosses.Halflight.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7867), 7),
            ["Bosses.Midir.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7869), 4),
            ["Bosses.Midir.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7869), 5),
            ["Bosses.Gael.Encountered"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7D67), 6),
            ["Bosses.Gael.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7D67), 7),
            ["Bosses.DemonPrince.Defeated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7367), 7),
            // TODO: Missing Ancient Wyvern?

            // Doors and Shortcuts
            ["Doors.PostIudexGundyr.Opened"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x24923), 3), // Gates to Firelink Shrine?
            ["Doors.ArchivesStart.Opened"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x22631), 7), // Grand Archives Main Door?
            ["Doors.PreTwinPrinces.Opened"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x22637), 3), // Twin Princes? Or is this the elevator

            // Elevators
            ["Elevators.Pontiff.Activated"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x23A3A), 3), // Pontiff's Shortcut?

            // Misc
            ["Misc.CoiledSword.Embedded"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A0F), 2),

            // Bonfires
            ["Bonfires.CemeteryOfAsh.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A03), 6),
            ["Bonfires.IudexGundyr.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A03), 5),
            ["Bonfires.FirelinkShrine.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A03), 7),
            ["Bonfires.UntendedGraves.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A03), 4),
            ["Bonfires.ChampionGundyr.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5A03), 3),
            ["Bonfires.HighWallOfLothric.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF02), 6),
            ["Bonfires.TowerOnTheWall.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF03), 2),
            ["Bonfires.VordtOfTheBorealValley.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF03), 5),
            ["Bonfires.DancerOfTheBorealValley.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF03), 3),
            ["Bonfires.OceirosTheConsumedKing.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0xF03), 6),
            ["Bonfires.FootOfTheHighWall.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1903), 3),
            ["Bonfires.UndeadSettlement.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1903), 7),
            ["Bonfires.DilapidatedBridge.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1903), 4),
            ["Bonfires.CliffUnderside.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1903), 5),
            ["Bonfires.RoadOfSacrifices.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D03), 1),
            ["Bonfires.HalfwayFortress.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D03), 7),
            ["Bonfires.CrucifixionWoods.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D03), 0),
            ["Bonfires.FarronKeep.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D03), 4),
            ["Bonfires.KeepRuins.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D03), 3),
            ["Bonfires.OldWolfOfFarron.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2D03), 2),
            ["Bonfires.CathedralOfTheDeep.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3C03), 4),
            ["Bonfires.CleansingChapel.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3C03), 7),
            ["Bonfires.RosariasBedChamber.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3C03), 5),
            ["Bonfires.CatacombsOfCarthus.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5003), 1),
            ["Bonfires.AbandonedTomb.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5003), 6),
            ["Bonfires.DemonRuins.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5003), 4),
            ["Bonfires.OldKingsAntechamber.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5003), 5),
            ["Bonfires.IrithyllOfTheBorealValley.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B03), 0),
            ["Bonfires.CentralIrithyll.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B03), 3),
            ["Bonfires.ChurchOfYorshka.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B03), 7),
            ["Bonfires.DistantManor.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B03), 2),
            ["Bonfires.WaterReserve.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B03), 1),
            ["Bonfires.AnorLondo.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B03), 4),
            ["Bonfires.PrisonTower.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x4B02), 7),
            ["Bonfires.IrithyllDungeon.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5503), 7),
            ["Bonfires.ProfanedCapital.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5503), 5),
            ["Bonfires.ArchdragonPeak.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2303), 7),
            ["Bonfires.DragonkinMausoleum.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2303), 4),
            ["Bonfires.GreatBelfry.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x2303), 5),
            ["Bonfires.LothricCastle.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1403), 7),
            ["Bonfires.DragonBarracks.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x1403), 5),
            ["Bonfires.GrandArchives.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x3703), 6),
            ["Bonfires.FlamelessShrine.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5F03), 7),
            ["Bonfires.KilnOfTheFirstFlame.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x5F03), 6),
            ["Bonfires.Snowfield.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6403), 6),
            ["Bonfires.RopeBridgeCave.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6403), 5),
            ["Bonfires.AriandelChapel.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6403), 2),
            ["Bonfires.CorvianSettlement.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6403), 4),
            ["Bonfires.SnowyMountainPass.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6403), 3),
            ["Bonfires.DepthsOfThePainting.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x6403), 0),
            ["Bonfires.TheDregHeap.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7303), 6),
            ["Bonfires.EarthenPeakRuins.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7303), 5),
            ["Bonfires.WithinTheEarthenPeakRuins.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7303), 4),
            ["Bonfires.TheDemonPrince.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7303), 7),
            ["Bonfires.MausoleumLookout.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7803), 5),
            ["Bonfires.RingedInnerWall.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7803), 4),
            ["Bonfires.RingedCityStreets.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7803), 3),
            ["Bonfires.SharedGrave.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7803), 2),
            ["Bonfires.ChurchOfFilianore.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7803), 7),
            ["Bonfires.DarkeaterMidir.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7803), 6),
            ["Bonfires.FilianoresRest.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7D03), 6),
            ["Bonfires.SlaveKnightGael.Lit"] = new BoolMemoryValue(new MemoryAddress(GameFlagData, 0, 0x7D03), 7),
        };

        private static Dictionary<string, MemoryValue> VirtualMemoryValues = new Dictionary<string, MemoryValue>() {
            ["Items.SmallDoll.Held"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.Deacons.Defeated"]),
            ["Items.GrandArchivesKey.Held"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.AbyssWatchers.Defeated"], RealMemoryValues["Bosses.Aldrich.Defeated"], RealMemoryValues["Bosses.YhormTheGiant.Defeated"]),

            ["Bonfires.ChampionsGravetender.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.ChampionsGravetender.Defeated"]),
            ["Bonfires.CrystalSage.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.CrystalSage.Defeated"]),
            ["Bonfires.DeaconsOfTheDeep.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.Deacons.Defeated"]),
            ["Bonfires.DragonslayerArmour.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.DragonslayerArmour.Defeated"]),
            ["Bonfires.HighLordWolnir.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.Wolnir.Defeated"]),
            ["Bonfires.NamelessKing.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.NamelessKing.Defeated"]),
            ["Bonfires.SisterFriede.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.Friede.Defeated"]),
            ["Bonfires.PontiffSulyvahn.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.Pontiff.Defeated"]),
            ["Bonfires.SoulOfCinder.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.SoulOfCinder.Defeated"]),
            ["Bonfires.TwinPrinces.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.TwinPrinces.Defeated"]),
            ["Bonfires.AbyssWatchers.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.AbyssWatchers.Defeated"]),
            ["Bonfires.AldrichDevourerOfGods.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.Aldrich.Defeated"]),
            ["Bonfires.PitOfHollows.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.CurseRottedGreatwood.Defeated"]),
            ["Bonfires.OldDemonKing.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.OldDemonKing.Defeated"]),
            ["Bonfires.YhormTheGiant.Lit"] = new VirtualBoolMemoryValue(RealMemoryValues["Bosses.YhormTheGiant.Defeated"]),
        };

        public static Dictionary<string, MemoryValue> KnownMemoryValues => GetOrCacheMemoryValues();

        public static Dictionary<int, string> OnlineAreas = new Dictionary<int, string>() {{
            300001, "High Wall of Lothric" },{
            300002, "High Wall - Darkwraith Chamber" },{
            300003, "High Wall - Bonfire 2" },{
            300004, "High Wall - Lower High Wall" },{
            300006, "High Wall - Dancer of the Boreal Valley" },{
            300007, "High Wall - Vordt of the Boreal Valley" },{
            300009, "High Wall - Post-Vordt" },{
            300008, "High Wall - Post-Dancer" },{
            300020, "Consumed King's Garden" },{
            300021, "King's Garden - Main Area" },{
            300022, "King's Garden - Lift/Shortcut Pre-Oceiros" },{
            300023, "King's Garden - Oceiros, the Consumed King" },{
            300024, "King's Garden - Post-Oceiros" },{
            301000, "Lothric Castle" },{
            301001, "Lothric Castle - Dragon Barracks" },{
            301002, "Lothric Castle - Lower Barracks" },{
            301003, "Lothric Castle - Altar of Sunlight" },{
            301010, "Lothric Castle - Dragonslayer Armor" },{
            310000, "Undead Settlement - Foot of the High Wall" },{
            310001, "Undead Settlement - Dilapidated Bridge" },{
            310002, "Undead Settlement - Cliff Underside" },{
            310003, "Undead Settlement - Irina" },{
            310020, "Undead Settlement - Curse-rotted Greatwood" },{
            310021, "Undead Settlement - Before Road of Sacrifices" },{
            320000, "Archdragon Peak - Nameless King Boss" },{
            320001, "Archdragon Peak" },{
            320002, "Archdragon Peak - Ancient Wyvern" },{
            320010, "Archdragon Peak - Dragon-kin Mausoleum" },{
            320011, "Archdragon Peak - Nameless King Bonfire" },{
            320012, "Archdragon Peak - Second Wyvern" },{
            320013, "Archdragon Peak - Great Belfry" },{
            320020, "Archdragon Peak - Mausoleum Lift" },{
            330000, "Crucifixion Woods - Crystal Sage" },{
            330001, "Crucifixion Woods - Halfway fortress" },{
            330010, "Farron Keep - Abyss Watchers" },{
            330011, "Farron Keep - Keep Ruins" },{
            330012, "Farron Keep - Swamp" },{
            330015, "Farron Keep - Old Wolf of Farron" },{
            330013, "Farron Keep - Pre-Abyss Watchers" },{
            330014, "Farron Keep Perimeter" },{
            330020, "Crucifixion Woods - Road of Sacrifices" },{
            330021, "Crucifixion Woods - After Crystal Sage" },{
            341000, "Grand Archives" },{
            341001, "Grand Archives - Archive Rooftops" },{
            341002, "Grand Archives - Post-Trio" },{
            341003, "Grand Archives - Grand Rooftop" },{
            341010, "Grand Archives - Twin Princes" },{
            350001, "Cathedral - Cleansing Chapel" },{
            350002, "Cathedral - Below Chapel" },{
            350003, "Cathedral - Outside Cathedral Door" },{
            350004, "Cathedral of the Deep" },{
            350005, "Cathedral - Pre-Deacons" },{
            350000, "Cathedral - Deacons of the Deep" },{
            350010, "Cathedral - Rosaria" },{
            350011, "Cathedral - Pre-Rosaria" },{
            350020, "Cathedral - Pre-Cleansing Chapel" },{
            370000, "Irithyll of the Boreal Valley" },{
            370001, "Irithyll - Central Irithyll" },{
            370002, "Irithyll - Church of Yorshka" },{
            370003, "Irithyll - Distant Manor" },{
            370004, "Irithyll - Siegward's Fireplace" },{
            370005, "Irithyll - Pre-Pontiff" },{
            370006, "Irithyll - Pontiff Sullyvahn" },{
            370007, "Irithyll - Bridge Entrance" },{
            370008, "Irithyll - Tower of Yorshka" },{
            370010, "Irithyll - Pontiff Hotspot" },{
            370011, "Irithyll - Darkmoon Tomb" },{
            370012, "Irithyll - Anor Londo" },{
            380000, "Catacombs - High Lord Wolnir" },{
            380001, "Catacombs - Entrance" },{
            380002, "Catacombs - Past Boulder Stairs" },{
            380020, "Catacombs - Abyss Watchers" },{
            380021, "Catacombs - Irithyll" },{
            380010, "Demon Ruins - Old Demon King" },{
            380012, "Demon Ruins - Abandoned Tomb" },{
            380013, "Demon Ruins - Horace's cave" },{
            380014, "Demon Ruins - Giant Avelyn" },{
            380015, "Demon Ruins - Old King's Antechamber" },{
            390000, "Irithyll Dungeon" },{
            390001, "Irithyll Dungeon - Sleeping Giant" },{
            390002, "Irithyll Dungeon - Lower Dungeon" },{
            390003, "Propaned Capital" },{
            390004, "Propaned Capital - Pre-Yhorm" },{
            390005, "Propaned Capital - Yhorm the Giant" },{
            400000, "Untended Graves - Champion Gundyr" },{
            400001, "Untended Graves" },{
            400002, "Dark Firelink Shrine" },{
            400010, "Untended Graves - Post-Oceiros" },{
            400100, "Cemetary of Ash - Iudex Gundyr" },{
            400101, "Cemetary of Ash" },{
            400102, "Firelink Shrine" },{
            410000, "Kiln of Flame - Soul of Cinder" },{
            410001, "Kiln - Pre-boss" },{
            410002, "Kiln - Flameless Shrine" },{
            450000, "AoA - Sister Friede" },{
            450001, "AoA - Snowfield" },{
            450002, "AoA - Corvian Settlement" },{
            450003, "AoA - Snowy Mountain Pass" },{
            450005, "AoA - Ariandel Chapel" },{
            450010, "AoA - Champion's Gravetender" },{
            450011, "AoA - Depths of the Painting" },{
            450020, "AoA - Priscilla's Arena" },{
            460000, "Arena - Grand Rooftop" },{
            470000, "Arena - Kiln of Flame" },{
            500000, "TRC - Demon Prince" },{
            500001, "TRC - The Dreg Heap" },{
            500002, "TRC - Earthen Peak Ruins" },{
            500003, "TRC - Within the Earthen Peak Ruins" },{
            510000, "TRC - Spear of the Church Boss" },{
            510001, "TRC - Mausoleum Lookout" },{
            510002, "TRC - Ringed Inner Wall" },{
            510003, "TRC - Ringed City Streets" },{
            510004, "TRC - Shared Grave" },{
            510005, "TRC - After Bridge" },{
            510010, "TRC - Darkeater Midir" },{
            510011, "TRC - Pre-Midir Boss" },{
            510020, "TRC - Church of Filianore" },{
            511000, "TRC - Slave Knight Gael" },{
            511001, "TRC - Filianore's Rest" },{
            511010, "TRC - Broken Church" },{
            530000, "Arena - Dragon Ruins" },{
            540000, "Arena - Round Plaza" }
        };
    }
}
