# Message Types #
MSS let's you map to and from a wide range of MIDI messages but it also uses some of its own types of messages. All messages in MSS have up to 3 pieces of data and the following table details how this data is used for each type of message.

| **Type** | **Data1** | **Data2** | **Data3** |
|:---------|:----------|:----------|:----------|
| Note     | Channel   | Note number | Velocity  |
| Note On  | Channel   | Note number | Velocity  |
| Note Off | Channel   | Note number | Velocity  |
| CC       | Channel   | CC number | CC value  |
| Pitch Bend | Channel   | N/A       | Pitch bend value |
| Poly Aftertouch | Channel   | Note number | Pressure  |
| Channel Aftertouch | Channel   | N/A       | Pressure  |
| Generator | Generator Name | N/A       | Generator value |
| Generator Modify | Generator Name | Operation | Operation parameter (see below for more details) |
| Parameter | Parameter ID | N/A       | Relative parameter value |

## Custom Message Types ##

### Generator ###
If you create a generator then a generator message will be created for it about a hundred times a second. The generator value will equal the result of the generator transformation at the current time. So if the equation for the generator is "1 - input" then the generator value will start at 1 and decrease to 0 before repeating.

### Generator Modify ###
Generator modify messages are used to modify generators that you have created. When you create a mapping to the generator modify message type you can set the generator it will effect and you can also set its operation to "On/Off", "Play/Pause", or "SetPosition". When one of these messages to sent it will do the following based on its generator operation:

**On/Off**
If the operation parameter is 0 then this disables the generator and sets the position in it's period back to 0 (so it will start from the beginning when it is enabled). Otherwise this will enable the generator.

**Play/Pause**
Disables the generator if the operation parameter is 0 and enables the generator otherwise.

**SetPosition**
Sets the position in the generators period to the value of the operation parameter. So if the operation parameter is 0.5 then the generator's position will jump to half way through its period.

### Parameter ###
Whenever a knob is modified in MSS a parameter message will be created. The relative parameter value will range from 0 to 1 regardless of what the actual value of the parameter is.