# Equations #

Equations are what really gives MSS it's power. You could be able to cover most of the basic things you want to do with MSS just using the presets but if you want to do something more advanced you'll need to learn to write some equations. This page provides a reference to the various operators, variables, constants, and functions available to you while creating an equation.

## Operators ##
| **Operator** | **Name** | **Description** | **Example** | **Result** |
|:-------------|:---------|:----------------|:------------|:-----------|
| a + b        | Addition |                 | 1 + 2       | 3          |
| a - b        | Subtraction |                 | 3 - 1       | 2          |
| a `*` b      | Multiplication |                 | 2 `*` 3     | 6          |
| a / b        | Division |                 | 3 / 2       | 1.5        |
| a % b        | Modulo   | Returns the remainder of a/b. | 123 % 10    | 3          |
| a or b, a `|``|` b | Or       | Returns 1 if either a or b are non-zero. Returns 0 otherwise. | 3 or 0      | 1          |
| a and b, a && b | And      | Returns 1 if both a and b are non-zero. Returns 0 otherwise. | 3 and 0     | 0          |
| a = b, a == b | Equals   | Returns 1 if a equals b. Returns 0 otherwise | 2 == 2      | 1          |
| a != b, a <> b | Not Equal | Returns 1 of a is not equal to b. Returns 0 otherwise | 2 != 2      | 0          |
| a < b        | Less Than |                 | 2 < 3       | 1          |
| a <= b       | Less Than or Equal to |                 | 3 <= 3      | 1          |
| a > b        | Greater Than |                 | 2 > 3       | 0          |
| a >= b       | Greater Than or Equal to |                 | 3 >= 3      | 1          |
| !a, not a    | Not      | Returns 1 if a is 0. Returns 0 otherwise. | !0          | 1          |

## Variables/Constants ##
| **Name** | **Description** |
|:---------|:----------------|
| x, input | These variables represent the input value from the an incoming message. The value taken from the incoming message is specified by the Input dropdown menu under the transformation graph. For example if the input message type is Note and the "velocity" is selected in the input dropdown then "x" or "input" will be the velocity of an incoming note. This value will be between 0 and 1. |
| d1, chan, channel | The first piece of data in an incoming message. For a MIDI message this will be the channel. You should use the "input" variable instead of this unless you need to use multiple types of input (e.g. velocity and channel). This value will be between 0 and 1. |
| d2, notenum, ccnum| The second piece of data in an incoming message. For a MIDI note message this will be the note number. You should use the "input" variable instead of this unless you need to use multiple types of input (e.g. velocity and note number). This value will be between 0 and 1. |
| d3, vel, velocity, ccval| The third piece of data in an incoming message. For a MIDI note message this will be the note's valocity. You should use the "input" variable instead of this unless you need to use multiple types of input (e.g. velocity and note number). This value will be between 0 and 1. |
| ignore   | Nothing will be output when you use an equation of "ignore". This is very useful when you want to filter out in coming messages. |
| semitone | This is literally equal to 1/127. This is because note numbers range from 0 to 127 and when you are dealing with them in an equation the range from 0 to 1. So you transpose all notes up an octave you could select "Note Number" as the input type and set the equation to "input + semitone" |
| octave   | Octave is equal to "semitone `*` 12" |
| pi       | 3.14159...      |

## Custom Functions ##
| **Name** | **Description** | **Example** | **Result** |
|:---------|:----------------|:------------|:-----------|
| in(x, s1, s2, ...) | Returns 1 if the first parameter is the same as any of the other parameters. Returns 0 otherwise | in(1+2, 2, 3, 4) | 1          |
|          |                 | in(1+2, 2, 4) | 0          |
| if(expression, onTrue, onFalse) | Returns the onFalse parameter if expression evaluates to 0. Otherwise returns the onTrue parameter. | if(2 > 3, 0.9, 0.1) | 0.1        |
|          |                 | if(4 > 3, 0.9, 0.1) | 0.9        |
| limit(x) | Limits x to fall between 0 and 1. This is applied by default to all of your curve and control point equations. | limit(-1.2) | 0          |
| lfo(input, attack, shape, phase, cycles, amount) | The lfo() function lets you create a wide range of LFOs and waveforms. It needs to be called with the following parameters. **Input**: the point at which the waveform should be evaluated. This will usually just be the word "input". **attack**: ranges from 0 to 1 and allows you to ease into an lfo. e.g. if the attack is set to 0.5 then the lfo will not reach its full amplitude until the input is 0.5 or greater. **shape**: an integer from 0 to 4 that represents the shape of the waveform. See the waveform parameter type on the [Variables](Variables.md) page for more info. **phase**: allows you to shift the waveform left or right by specifying the number of degrees to offset it by. e.g. if you set phase to 180 then the wave form will be shifted to the right by half of a cycle. **cycles**: The number of cycles in the waveform. **amount**: ranges from 0 to 1 and sets the size of the range of amplitudes. e.g. if you set this to 1 then the amplitudes will range from 0 to 1. If you set this to 0.5 then the amplitudes will range from 0.25 to 0.75. | See the lfo preset in MSS for an example of how the lfo() function works. |            |
| waveform(input, shape) | The waveform() function is just a simplified version of the lfo() function which only takes the "input" and "shape" parameters. | waveform(input, 0)| creates a sine wave |


## Math Functions ##

| **Name** | **Description** | **Example** | **Result** |
|:---------|:----------------|:------------|:-----------|
|Abs(x)    |Returns the absolute value of a specified number. |Abs(-2)      |2           |
|Acos(x)   |Returns the angle whose cosine is the specified number. |Acos(1)      |0           |
|Asin(x)   |Returns the angle whose sine is the specified number. |Asin(0)      |0           |
|Atan(x)   |Returns the angle whose tangent is the specified number. |Atan(0)      |0           |
|Ceiling(x) |Returns the smallest integer greater than or equal to the specified number. |Ceiling(1.5) |2           |
|Cos(x)    |Returns the cosine of the specified angle. |Cos(0)       |1           |
|Exp(x)    |Returns e raised to the specified power. |Exp(0)       |1           |
|Floor(x)  |Returns the largest integer less than or equal to the specified number. |Floor(1.5)   |1           |
|IEEERemainder(a,b) |Returns the remainder resulting from the division of a specified number by another specified number. |IEEERemainder(3, 2) |-1          |
|Log(x, base) |Returns the logarithm of a specified number. |Log(1, 10)   |0           |
|Log10(x)  |Returns the base 10 logarithm of a specified number. |Log10(1)     |0           |
|Max(a, b) |Returns the larger of two specified numbers. |Max(1, 2)    |2           |
|Min(a, b) |Returns the smaller of two numbers. |Min(1, 2)    |1           |
|Pow(x, exp) |Returns a specified number raised to the specified power. |Pow(2, 3)    |8           |
|Round(x, numDigits) |Rounds a value to the nearest integer or specified number of decimal places. |Round(3.222, 2) |3.22        |
|Sign(x)   |Returns a value indicating the sign of a number. |Sign(-10)    |-1          |
|Sin(x)    |Returns the sine of the specified angle. |Sin(0)       |0           |
|Sqrt(x)   |Returns the square root of a specified number. |Sqrt(4)      |2           |
|Tan(x)    |Returns the tangent of the specified angle. |Tan(0)       |0           |
|Truncate(x) |Calculates the integral part of a number. |Truncate(1.7) |1           |