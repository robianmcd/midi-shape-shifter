## MSS Dev Environment Setup ##
  1. Download and install MIDI Shape Shifter. This will make sure that all the required dependencies are install on your computer.
  1. Install VisualStudio 2010 or later
  1. Install Git
  1. Clone the MSS repo with git into a folder:
    * git clone https://code.google.com/p/midi-shape-shifter/
  1. Open MidiShapeShifter.sln from the root folder of the repo
    * Note: you'll probably get some error message about not being able to import the .isproj project file. You can just ignore that for now.
  1. In the solution explorer right click the MidiShapeShifter project and select properties
  1. Go to the debug tab, select "Start external program" and pick a VST host to test MSS from. Note: The 32-bit version of mss will run by default.
  1. Press F5 to start debugging MSS
  1. In the VST host that pops up set your VST folder to midi-shape-shifter/MidiShapeShifter/MidiShapeShifter/bin/vst-dist
  1. Now you should be all set up. To verify that your debugger is working Set a breakpoint in the constructor of MssComponentHub. It should get hit when you load MSS into your Host. Don't worry if you get an error message saying that the module wasn't built with debugging information.

### TODO ###
The following documentation still needs to be written
  * Importing .isproj project files.
  * Setting up credentials so you can commit.
  * Running unit tests