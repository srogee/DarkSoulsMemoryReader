﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DS3MemoryReader
{
    class DS3OnlineAreas
    {
        // These names don't really seem that accurate? I lifted these directly from the cheat engine table.
        public static Dictionary<int, string> IdToName = new Dictionary<int, string>() {{
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
            //370012, "Irithyll - Aldrich, Devourer of Gods" },{
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
