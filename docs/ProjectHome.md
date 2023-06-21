# midi-shape-shifter

![logo](https://github.com/aminya/midi-shape-shifter/raw/master/Graphics/Logo/MSS%20Logo%20Balloon%20-%20Square.png)

MIDI VST for mapping, generating, and transforming MIDI data.

## Overview

MIDI Shape Shifter (MSS) is a VST MIDI plugin for mapping, generating and transforming MIDI data. This plugin's simple and flexible interface is capable of satisfying a wide range of MIDI needs. For example, MIDI Shape Shifter could be used to:

* Apply a velocity curve
* Generate an LFO
* Create mappings between ranges of MIDI data
* Filter out specified MIDI data
* Apply midi compression or gate effects
* Automatically harmonize notes
* Create split/dual keyboard
* Transpose
* Control multiple parameters with a single knob
* etc


Checkout this Youtube Video:

[![Youtube Video](https://github.com/robianmcd/midi-shape-shifter/assets/16418197/21ac3a5d-9b5a-4081-9815-0c84f6964680)](http://www.youtube.com/watch?v=B8kptILs7vw)
___

## News

**June 21 2023**: Released [Midi Shape Shifter 1.1.0](https://github.com/robianmcd/midi-shape-shifter/releases/download/v1.1.0/MidiShapeShifter-v1.1.0.zip). :tada: This fixes the loading issue because of dependencies. It updates the dependencies and revamps the build system to install and merge them automatically.

___

## Support

| **Forum**  | [MSS User Forum](https://groups.google.com/d/forum/midi-shape-shifter) - Post your questions and comments here.  |
|---|---|
|  **Contact**  | Email: `mss-support@googlegroups.com`  |

___

## Contribute

If you like this project and want to help out there is lots you can do.

**Give your Feedback**

> Visit the [User Forum](https://groups.google.com/d/forum/midi-shape-shifter) and let us know if you need help with something, what your using MSS for, and what you wish it could do. This will help prioritise what features get added in the future. If run into a bug then don't hesitate to report it either in the [User Forum](https://groups.google.com/d/forum/midi-shape-shifter) or on the [Issues Page](http://code.google.com/p/midi-shape-shifter/issues/list).

**Documentation**

> This project has very little in the way of documentation. If your up for writing a guide/tutorial/user manual then let me know. I'll be happy to supply you with what any information you need to make this happen.

**Develop**

> This is an open source project so if your a C# developer and your up for contributing some code then let me know. As an added bonus I can give you a free open-source license for `ReShaper` if you contribute.

**Donate**

🤝 The program is free and open-source. You can show your support through `Patreon`:

https://www.patreon.com/aminya

or through `GitHub Sponsors`:

https://github.com/sponsors/aminya

___

## Features

There are 3 main components to the MIDI Shape Shifter interface.

**Mapping**

The mapping component allows users to map a given range of MIDI messages to any other range of MIDI messages. MIDI Shape Shifter also has some of its own message types that can be used in mappings. For example, a user could map a CC message to a "Generator Toggle" message which would cause a generator in the application to be toggled on or off when the CC message is received.

**Generating**

This component is essentially used to create highly customizable LFOs. These LFOs can be synced to the tempo of the host or set to repeat at a constant frequency. The shape of each LFO is then specified in the Transformation component.

**Transforming**

Each mapping and each generator will have a transformation associated with it. Transformations determine how messages will be modified. For example a transformation could apply a velocity curve to a mapping of note on messages. Transformations can either be manually input or created from one of the presets.
