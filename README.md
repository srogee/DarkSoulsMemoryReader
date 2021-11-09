# Dark Souls Memory Reader
A simple framework for reading game data from a Dark Souls III process in real time. Potential use cases could be displaying where players died most frequently, what items were skipped, listing nearby enemies, etc.

Offsets and addresses were manually taken from [this DS3 Cheat Table](https://github.com/igromanru/Dark-Souls-III-Cheat-Engine-Guide). It looks like most of the values displayed in the table can be read using the code in this repository. For example, here's the relevant Cheat Engine definition for the player angle:
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

And the relevant memory value definition (note that Cheat Engine's offsets are listed backwards):
```
["Player.Angle"] = new FloatMemoryValue(new MemoryAddress(BaseB, 0x40, 0x28, 0x74)),
```
