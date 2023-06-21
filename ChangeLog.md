# Release Notes for v1.1.0

**June 21 2023**: Released [Midi Shape Shifter 1.1.0](https://github.com/robianmcd/midi-shape-shifter/releases/download/v1.1.0/MidiShapeShifter-v1.1.0.zip). :tada: This fixes the loading issue because of dependencies. It updates the dependencies and revamps the build system to install and merge them automatically.

Here is the full list of changes:

[1.1.0](https://github.com/robianmcd/midi-shape-shifter/compare/v1.0.11-beta...v1.1.0) (2023-06-21)

### Features
* merge the dlls using ILRepack ([334910a](https://github.com/robianmcd/midi-shape-shifter/commit/334910a5c2d040be7fbd7c1a9f863eac98dcb166))
* use Nuget to install and build VST.NET v1.1 ([bf16028](https://github.com/robianmcd/midi-shape-shifter/commit/bf16028e3843dcf90a833448230649ca78e13039))
* install the factory settings in the installation ([2b67137](https://github.com/robianmcd/midi-shape-shifter/commit/2b671375381348bb39947559915b72a98f2958d4))
* add authors list to the VST ([c294247](https://github.com/robianmcd/midi-shape-shifter/commit/c294247fb39c356147acfd17438e7ca182e9484a))

### Bug Fixes

* handle null registry key for the settings ([12cfe09](https://github.com/robianmcd/midi-shape-shifter/commit/12cfe098bc8209f1deab1304d18876a5368cb4d7))
* build LBIndustrialCtrls and ZedGraph from the SDK folder ([2ecee34](https://github.com/robianmcd/midi-shape-shifter/commit/2ecee34ba41874ce199a32b0dd36b700971d6bcc))
* create the user/factory settings if it doesn't exist ([3f14442](https://github.com/robianmcd/midi-shape-shifter/commit/3f144425f9e98c97a2d55c14cd9b398b1d9d36f4))
* handle non-existing settings folder ([e379de0](https://github.com/robianmcd/midi-shape-shifter/commit/e379de03c7d724213309d971316703b2435abba9))
* handle unauthorized access errors for settings folder ([0c949f3](https://github.com/robianmcd/midi-shape-shifter/commit/0c949f3892fc00d1543234d73a082f9f7e3bb73b))
* update and build LBIndustrialCtrls from source ([d84759e](https://github.com/robianmcd/midi-shape-shifter/commit/d84759ed1f888b52b27573ed49e98a94dd136e0b))


# Release Notes for v1.0.11
- 🎉  Moved the repository to GitHub by @aminya
- Updated the Dotnet Framework to 4.5.2 by @aminya in  https://github.com/robianmcd/midi-shape-shifter/pull/4
- Updated the installer and the build by @aminya in  https://github.com/robianmcd/midi-shape-shifter/pull/4
- Cleaned up the code to match the new C# changes  by @aminya in https://github.com/robianmcd/midi-shape-shifter/pull/1,  https://github.com/robianmcd/midi-shape-shifter/pull/3
- Dropped the support for Windows 32-bit by @aminya in  https://github.com/robianmcd/midi-shape-shifter/pull/4

# Release Notes for v1.0.8
- Fixed 'detune' error message in Sonar X2
- Added an equation editor dialog so that the equation can be edited in hosts like Sonar and Reaper
- Added 64-bit support.
- Moved default settings folder from program files to Users Folder.

# **July 28 2013**:
Released [Midi Shape Shifter 1.0.10-Beta](http://code.google.com/p/midi-shape-shifter/downloads/list). Several bugs were fixed in this release, the most notable of which was causing most people in continental Europe to not be able to use control points.

# **May 4 2013**:
Released [Midi Shape Shifter 1.0.8-Beta](http://code.google.com/p/midi-shape-shifter/downloads/list). If you want to help make Midi Shape Shifter better let us know what you think at the [MSS User Forum](https://groups.google.com/d/forum/midi-shape-shifter).

# **April 19 2013**:
Fixed an issue with the 64-bit install for MSS v1.0.7Beta. The updated installer is now available for download. **March 10 2013**: Released [Midi Shape Shifter v1.0.7Beta](http://code.google.com/p/midi-shape-shifter/downloads/detail?name=MSS%20Installer%20v1.0.7Beta.exe). Added several bug fixes in this release mostly to do with the mapping dialog.
