# Transformation #

There is a transformation associated with each mapping and each generator. All messages are split up into 3 pieces of data and the transformation determines how the mapping/generator will output the third piece of data in a message. See the [Message Types](MessageTypes.md) page for more details on what kind of data is stored in a message.

For example, if you create a mapping from note messages to CC messages with the input set to "velocity" and the equation set to "input `*` 2" then the CC messages that are output will have a CC value that is double the velocity of the incoming notes.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Transform.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Transform.png)

  1. **Presets:** This drop down menu contains a number of preset transformations. The presets are a good way to get started with MSS and will save you from having to write any of the equations your self. You can also use the presets as a starting point and modify them as much as you want.
  1. **Open/Save Preset:** If you want to keep the changes you have made to a transformation you can save them to a preset. By default you will be able to select any preset that is in your User Presets folder from the drop down menu but if you have a preset somewhere else on your computer you can use the open button to open it.
  1. **Graph:** The transformation graph shows you how the data from the input message will be transformed into the third piece of data for the output message. The x-axis represents input and the y-axis represents output.
  1. **Control Points:** If you click the graph you can add control points. Adding a control point will break the transformation into multiple segments that can each have their own equation. You can also just drag and drop control points to create the shape of the graph without having to worry about the equations.
  1. **Input Selector:** This dropdown menu lets you select which part of an incoming MIDI message you want to use as the input. For example if you wanted to map an incoming note number to a CC value you could select "Note Num" from this menu.
  1. **Segment Selectors:** When there are control points on the graph creating multiple segments you can use the segment selectors to select which segment you want to view the equation for. This includes line segments and control points. The active segment is highlighted in dark blue.
  1. **Reset:** This button gets rid of any control points and sets the equation to "input".
  1. **Equation**: This lets you specify the equation used to transform an incoming value to an outgoing value. For example if you wanted to increase the volume by a constant amount you could create a mapping from Notes to Notes and set the equation to something like "input + 0.1". For more information on writing equations see the [Equations](Equations.md) page.
  1. **Parameter Knobs:** The values from these knobs can be used in the equation if you just put the name of a knob into the equation. e.g. "input + p1". See the [Variables](Variables.md) page for more details on how knobs can be used in MSS. The main different between the parameter knobs and the variable knobs is that there are a separate set of parameter knobs for each mapping/generator while there is only one global set of variable knobs. Many of the presets make use of these knobs so you don't have to worry about the equation.

## See Also ##
  * [Equations](Equations.md)