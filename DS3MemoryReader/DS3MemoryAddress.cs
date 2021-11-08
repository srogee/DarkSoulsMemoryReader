using System.Collections.Generic;
using System.Linq;

namespace DS3MemoryReader
{
    public struct DS3MemoryAddress
    {
        // Base addresses for lots of information in DS3
        public const int BaseA = 0x4740178;
        public const int BaseB = 0x4768E78;
        public const int GameFlagData = 0x473BE28;

        public int BaseAddress;
        public int[] Offsets;
        public object ExtraInfo;

        public DS3MemoryAddress(object extraInfo, int baseAddress, params int[] offsets) {
            BaseAddress = baseAddress;
            Offsets = offsets;
            ExtraInfo = extraInfo;
        }

        public DS3MemoryAddress AddOffsets(int[] offsets) {
            var newOffsets = Offsets.Concat(offsets).ToArray();
            return new DS3MemoryAddress(ExtraInfo, BaseAddress, newOffsets);
        }

        public DS3MemoryAddress AddOffset(int offset) {
            return AddOffsets(new int[] { offset });
        }

        public static Dictionary<string, DS3MemoryAddress> KnownAddresses = new Dictionary<string, DS3MemoryAddress>() {
            // Player data
            ["Player.Angle"] = new DS3MemoryAddress(null, BaseB, 0x40, 0x28, 0x74),
            ["Player.X"] = new DS3MemoryAddress(null, BaseB, 0x40, 0x28, 0x80),
            ["Player.Y"] = new DS3MemoryAddress(null, BaseB, 0x40, 0x28, 0x84),
            ["Player.Z"] = new DS3MemoryAddress(null, BaseB, 0x40, 0x28, 0x88),
            ["Player.OnlineArea"] = new DS3MemoryAddress(null, BaseB, 0x80, 0x1ABC),

            // Boss flags
            ["Bosses.IudexGundyr.PulledSwordOut"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x5A67),
            ["Bosses.IudexGundyr.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5A67),
            ["Bosses.IudexGundyr.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5A67),
            ["Bosses.Vordt.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0xF67),
            ["Bosses.Vordt.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0xF67),
            ["Bosses.CurseRottedGreatwood.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x1967),
            ["Bosses.CurseRottedGreatwood.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x1967),
            ["Bosses.CrystalSage.Encountered"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x2D69),
            ["Bosses.CrystalSage.Defeated"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x2D69),
            ["Bosses.Deacons.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x3C67),
            ["Bosses.Deacons.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x3C67),
            ["Bosses.AbyssWatchers.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x2D67),
            ["Bosses.AbyssWatchers.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x2D67),
            ["Bosses.Wolnir.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5067),
            ["Bosses.Wolnir.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5067),
            ["Bosses.OldDemonKing.Defeated"] = new DS3MemoryAddress(1, GameFlagData, 0, 0x5064),
            ["Bosses.YhormTheGiant.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5567),
            ["Bosses.YhormTheGiant.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5567),
            ["Bosses.Pontiff.Defeated"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x4B69),
            ["Bosses.Aldrich.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x4B67),
            ["Bosses.Dancer.Defeated"] = new DS3MemoryAddress(5, GameFlagData, 0, 0xF6C),
            ["Bosses.Oceiros.Encountered"] = new DS3MemoryAddress(1, GameFlagData, 0, 0xF6B),
            ["Bosses.Oceiros.Defeated"] = new DS3MemoryAddress(1, GameFlagData, 0, 0xF64),
            ["Bosses.ChampionGundyr.Encountered"] = new DS3MemoryAddress(1, GameFlagData, 0, 0x5A64), // TODO: Is this correct?
            ["Bosses.ChampionGundyr.Defeated"] = new DS3MemoryAddress(0, GameFlagData, 0, 0x5A64),
            ["Bosses.NamelessKing.Defeated"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x2369),
            ["Bosses.DragonslayerArmour.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x1467),
            ["Bosses.TwinPrinces.Encountered"] = new DS3MemoryAddress(0, GameFlagData, 0, 0x3764),
            ["Bosses.TwinPrinces.Defeated"] = new DS3MemoryAddress(1, GameFlagData, 0, 0x3764),
            ["Bosses.SoulOfCinder.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5F67),
            ["Bosses.SoulOfCinder.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5F67),
            ["Bosses.ChampionsGravetender.Encountered"] = new DS3MemoryAddress(2, GameFlagData, 0, 0x6468),
            ["Bosses.ChampionsGravetender.Defeated"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x6468),
            ["Bosses.Friede.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x6467),
            ["Bosses.Friede.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x6467),
            ["Bosses.Halflight.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x7867),
            ["Bosses.Halflight.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x7867),
            ["Bosses.Midir.Encountered"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x7869),
            ["Bosses.Midir.Defeated"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x7869),
            ["Bosses.Gael.Encountered"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x7D67),
            ["Bosses.Gael.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x7D67),
            ["Bosses.DemonPrince.Defeated"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x7367),
            // TODO: Missing Ancient Wyvern?

            // Doors and Shortcuts
            //["Doors.PostIudexGundyr.Opened"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x24923), // Gates to Firelink Shrine?
            //["Doors.ArchivesStart.Opened"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x22631), // Grand Archives Main Door?
            //["Doors.PreTwinPrinces.Opened"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x22637), // Twin Princes? Or is this the elevator

            //// Elevators
            //["Elevators.Pontiff.Activated"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x23A3A), // Pontiff's Shortcut?

            // Misc
            ["Misc.CoiledSword.Embedded"] = new DS3MemoryAddress(2, GameFlagData, 0, 0x5A0F),

            // Bonfires
            ["Bonfires.CemeteryOfAsh.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5A03),
            ["Bonfires.IudexGundyr.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x5A03),
            ["Bonfires.FirelinkShrine.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5A03),
            ["Bonfires.UntendedGraves.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x5A03),
            ["Bonfires.ChampionGundyr.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x5A03),
            ["Bonfires.HighWallOfLothric.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0xF02),
            ["Bonfires.TowerOnTheWall.Lit"] = new DS3MemoryAddress(2, GameFlagData, 0, 0xF03),
            ["Bonfires.VordtOfTheBorealValley.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0xF03),
            ["Bonfires.DancerOfTheBorealValley.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0xF03),
            ["Bonfires.OceirosTheConsumedKing.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0xF03),
            ["Bonfires.FootOfTheHighWall.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x1903),
            ["Bonfires.UndeadSettlement.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x1903),
            ["Bonfires.DilapidatedBridge.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x1903),
            ["Bonfires.CliffUnderside.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x1903),
            ["Bonfires.RoadOfSacrifices.Lit"] = new DS3MemoryAddress(1, GameFlagData, 0, 0x2D03),
            ["Bonfires.HalfwayFortress.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x2D03),
            ["Bonfires.CrucifixionWoods.Lit"] = new DS3MemoryAddress(0, GameFlagData, 0, 0x2D03),
            ["Bonfires.FarronKeep.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x2D03),
            ["Bonfires.KeepRuins.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x2D03),
            ["Bonfires.OldWolfOfFarron.Lit"] = new DS3MemoryAddress(2, GameFlagData, 0, 0x2D03),
            ["Bonfires.CathedralOfTheDeep.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x3C03),
            ["Bonfires.CleansingChapel.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x3C03),
            ["Bonfires.RosariasBedChamber.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x3C03),
            ["Bonfires.CatacombsOfCarthus.Lit"] = new DS3MemoryAddress(1, GameFlagData, 0, 0x5003),
            ["Bonfires.AbandonedTomb.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5003),
            ["Bonfires.DemonRuins.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x5003),
            ["Bonfires.OldKingsAntechamber.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x5003),
            ["Bonfires.IrithyllOfTheBorealValley.Lit"] = new DS3MemoryAddress(0, GameFlagData, 0, 0x4B03),
            ["Bonfires.CentralIrithyll.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x4B03),
            ["Bonfires.ChurchOfYorshka.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x4B03),
            ["Bonfires.DistantManor.Lit"] = new DS3MemoryAddress(2, GameFlagData, 0, 0x4B03),
            ["Bonfires.WaterReserve.Lit"] = new DS3MemoryAddress(1, GameFlagData, 0, 0x4B03),
            ["Bonfires.AnorLondo.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x4B03),
            ["Bonfires.PrisonTower.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x4B02),
            ["Bonfires.IrithyllDungeon.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5503),
            ["Bonfires.ProfanedCapital.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x5503),
            ["Bonfires.ArchdragonPeak.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x2303),
            ["Bonfires.DragonkinMausoleum.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x2303),
            ["Bonfires.GreatBelfry.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x2303),
            ["Bonfires.LothricCastle.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x1403),
            ["Bonfires.DragonBarracks.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x1403),
            ["Bonfires.GrandArchives.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x3703),
            ["Bonfires.FlamelessShrine.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x5F03),
            ["Bonfires.KilnOfTheFirstFlame.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x5F03),
            ["Bonfires.Snowfield.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x6403),
            ["Bonfires.RopeBridgeCave.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x6403),
            ["Bonfires.AriandelChapel.Lit"] = new DS3MemoryAddress(2, GameFlagData, 0, 0x6403),
            ["Bonfires.CorvianSettlement.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x6403),
            ["Bonfires.SnowyMountainPass.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x6403),
            ["Bonfires.DepthsOfThePainting.Lit"] = new DS3MemoryAddress(0, GameFlagData, 0, 0x6403),
            ["Bonfires.TheDregHeap.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x7303),
            ["Bonfires.EarthenPeakRuins.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x7303),
            ["Bonfires.WithinTheEarthenPeakRuins.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x7303),
            ["Bonfires.TheDemonPrince.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x7303),
            ["Bonfires.MausoleumLookout.Lit"] = new DS3MemoryAddress(5, GameFlagData, 0, 0x7803),
            ["Bonfires.RingedInnerWall.Lit"] = new DS3MemoryAddress(4, GameFlagData, 0, 0x7803),
            ["Bonfires.RingedCityStreets.Lit"] = new DS3MemoryAddress(3, GameFlagData, 0, 0x7803),
            ["Bonfires.SharedGrave.Lit"] = new DS3MemoryAddress(2, GameFlagData, 0, 0x7803),
            ["Bonfires.ChurchOfFilianore.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x7803),
            ["Bonfires.DarkeaterMidir.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x7803),
            ["Bonfires.FilianoresRest.Lit"] = new DS3MemoryAddress(6, GameFlagData, 0, 0x7D03),
            ["Bonfires.SlaveKnightGael.Lit"] = new DS3MemoryAddress(7, GameFlagData, 0, 0x7D03),
        };
    }
}
