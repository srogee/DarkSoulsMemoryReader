# DS3 Memory Reader
A simple console application for reading player coordinates in real time from a Dark Souls III process. Updates values at ~60hz and continually searches for DS3 so it works fine if DS3 closes or crashes.

Example output:
```
Attached to process 12924
Player found

Character Name: Surprise!!
X: 340.418600
Y: -723.959100
Z: -319.479100
Angle: 1.937141
Online Area: Demon Ruins - Abandoned Tomb (380012)
```

Offsets and addresses are taken from [this DS3 Cheat Table](https://github.com/igromanru/Dark-Souls-III-Cheat-Engine-Guide). It looks like most of the values displayed in the table can be read using the code in this repository. For example, here's the relevant cheat table code for defining the player angle:
```
define(BaseB,DarkSoulsIII.exe+4768E78)
...
<CheatEntry>
    <ID>72361</ID>
    <Description>"Angle"</Description>
    <LastState Value="-1.482927561" RealAddress="7FF44D0666D4"/>
    <VariableType>Float</VariableType>
    <Address>BaseB</Address>
    <Offsets>
        <Offset>74</Offset>
        <Offset>28</Offset>
        <Offset>40</Offset>
    </Offsets>
</CheatEntry>
```

And here's the code for reading the player angle using code from this repository. Note that Cheat Engine's offsets are backwards.
```
int baseB = 0x4768E78;
var playerAngle = new DS3MemoryValue<float>(processInfo, new DS3MemoryAddress(baseB, new int[] { 0x40, 0x28, 0x74 }));
Console.WriteLine(playerAngle.Value)
```

I named the classes after DS3, but really this can be used for any game that has a cheat table.
