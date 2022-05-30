# Variables #
Variables can be used in equations and give you an easy way to modify an equation just by turning a knob in MSS or in your host. To use a variable in an equation just enter the variables name. For example if you have a mapping that outputs notes and the equation is set to "input + A" then you could boost the volume by turning up the A knob.

The preset knobs found at the bottom of the Transformation section of MSS are varialbe similar to the variable knobs so unless otherwise noted, this documentation applies to both. The main difference between the two is that preset parameters are associated with a single mapping/generator while variables are global. This makes variables a good place to store anything you want to share between mappings or control from your host.

In addition to manually turning a preset/variable knob to modify a parameter, you can also create a mapping that will set its value. For example if you map a note to variable "A" then "A" will be set to the velocity of that note every time you hit it.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Variables.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Variables.png)


## Edit Variable Dialog ##

You can right click on a variable or preset knob to open the parameter editor.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Edit-Variable.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Edit-Variable.png)

  1. **Parameter Name**: Renaming a parameter can help you remember what it does and your custom name can also be used directly in an equation. The default name for a variable/parameter will still refer to the variable in an equation (e.g. A, B, C, P1, P2, P3, etc). You can also add "`_`rel" to the end of a variable name in an equation (e.g. A\_rel). This will cause its value to range from 0 to 1 regardless of what its min and max value are.
  1. **Parameter Type**: Setting the parameter type to Number, Integer, or Waveform will allow you to restrict the parameter's value in different ways.
    * Number: The value can be any number between the min and max value.
    * Integer: The value can be any whole number between the min and max value.
    * Waveform: This will let the value take on 5 different values that represent different the waveforms: Sin, Square, Triangle, Ramp, and Saw. Technically these values are just the integers 0 through 4 but this parameter type gives you a good to to keep track of which  waveform the variable is representing. These wave form values are intended to be used with the lfo() or waveform() functions in an equation. See the [Equations](Equations.md) page for more info or take a look at the "LFO" preset in MSS for an example of this.
  1. **Min/Max Value**: Specifies the minimum and maximum values that a variable can have.
  1. **Value**: Specifies the variables current value.