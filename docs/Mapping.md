# Mapping #

To accomplish anything in MSS you'll need to create a mapping. Mappings take incoming MIDI messages and convert them into output. You can also use mappings to map to and from internal messages like generator messages. See the [Message Types](MessageTypes.md) page for more information on the types of messages you can use in a mapping.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Mapping.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Mapping.png)
  1. **Mapping List**: This list gives you a summary of all of your existing mappings. By selecting a mapping in this list you will be able to see its [Transformation](Transformation.md). All of the buttons below will apply to the selected mapping.
  1. **Add**: Opens the "Mapping Details Dialog" for creating a new mapping. This dialog is documented below.
  1. **Delete**: Deletes the selected mapping.
  1. **Edit**: Opens the "Mapping Details Dialog" for editing the selected mapping. This dialog is documented below.
  1. **Move Up/Down**: Moves the selected mapping up or down on the list. The order of mappings does not usually matter however if you select the "Override Duplicates" option for multiple mappings with the same input range then only the first one in the list will be used.

## Mapping Details Dialog ##

This dialog lets you specify the input and output range for a mapping. Any MIDI message being sent to MSS will pass through it unaffected unless the message falls inside the input range on a mapping. A range can either be a single number, a range of numbers separated by a dash (e.g. 1-16) or the word "all" which will create a range that includes everything.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Add-Mapping.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Add-Mapping.png)

> a. **Message Type**: Specifies the type of message that the input/output range applies to. <br />
> b. **Input 1**: The first input field is usually used to specify the range of MIDI channels that will match your input/output range. It can also be used to select the parameter or generator name. <br />
> c. **Input 2**: The secod input field will determine how your range matches the second piece of data in a message. See the Data2 column in the [Message Types](MessageTypes.md) table for more info. <br />
> d. **Override Duplicates**: If you have multiple mappings with input ranges that overlap then you can select this option to specify which mapping should be used. <br />
> e. **Same as Input**: Makes the output range the same as the input range. <br />
> f. **Learn**: If you press this button and send MSS some MIDI it will try to automatically fill in the input/output range for you. <br />