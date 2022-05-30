# Generators #

Generators can be used for many different things but the most common use case is to create an LFOs. Unlike most LFOs from other plugins, a generator's shape is not restricted to standard waveforms. After creating a generator you can shape it how ever you want in the [Transformation](Transformation.md) section of MSS by using the provided LFO preset, drawing a shape with control points, or creating your own equation.

On its own a generator will just create generator messages which won't do anything unless you map them to something. For example you may want to create a mapping that maps the generator messages from your generator to a CC value to control a parameter in your host.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Generators.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Generators.png)

  1. **Generator List**: This list gives you a summary of all of your existing generators. By selecting a generator in this list you will be able to see its [Transformation](Transformation.md). All of the buttons below will apply to the selected generator.
  1. **Add**: Opens the "Generator Details Dialog" for creating a new generator. This dialog is documented below.
  1. **Delete**: Deletes the selected generator.
  1. **Edit**: Opens the "Generator Details Dialog" for editing a generator. This dialog is documented below.

## Generator Details Dialog ##

This dialog lets you configure a generator's name, period, lopped status, and enabled status.

![https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Add-Generators.png](https://dl.dropboxusercontent.com/u/20066539/Images/MSS/MSS-Add-Generators.png)

> a. **Generator Name**: You can give a generator a name to easily tell it apart from other generators. <br />
> b/c. **Period Type**/**Period**: The Period Type and Period fields are used to specify the length of the generator (the amount of time before it repeats). You can select "Time" or "Bars" for the period type. If you select "Time" then you will be able to specify the length of the period in milliseconds. If you select "Bars" then you will be able to specify the length of the period in bars or notes. <br />
> d. **Loop**: Specifies whether the generator will start over after reaching the end of one cycle. <br />
> e. **Enabled**: Specifies whether the generator is currently generating anything. Note that if you want to trigger the enabled status of a generator then you can create a mapping to a generator modify message. See the Generator Modify section of the [Message Types](MessageTypes.md) page for more info.<br />