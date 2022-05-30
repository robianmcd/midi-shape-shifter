You must follow the following steps to release a new version of MSS

**Increment the Version Number**
  1. Right click on the MidiShapeShifter project and select properties.
  1. On the Application tab click Assembly Information
  1. Increment the Assembly version
  1. In the MidiShapeShifterDeploy project go to the General Information view and increment the Product Version

**Build the Installers**
  1. Open the Configuration Manager and select "Deploy" for the configuration and x86 for the platform.
  1. Go to the files view in the MidiShapeShifterDeploy project and under the WindowsFolder/System add the msvcr110.dll from MidiShapeShifterDeploy\install-dist\x86
  1. Build the installer
  1. Do steps 1-3 for x64

**Upload Installer**
  1. Upload the installers you build to the downloads page