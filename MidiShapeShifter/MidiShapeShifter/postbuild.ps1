# The following variables should be set in Visual Studio:
# set ProjectDir=$(ProjectDir)
# set TargetDir=$(TargetDir)
# set TargetFileName=$(TargetFileName)

$dist="$env:ProjectDir/bin/vst-dist/"

if (Test-Path "$dist") {
    rm -r -Force "$dist"
}
mkdir "$dist"

# Copy all files except dlls
ls -r "$env:TargetDir" -Exclude *.dll,*.pdb | cp -r -Force -Destination "$dist"

# Copy the main VST dll
cp "$env:TargetDir/Jacobi.Vst.Interop.dll" "$dist/$env:TargetFileName"

# Copy VST.net dlls
cp "$env:TargetDir/Jacobi.Vst.Core.dll" "$dist"
cp "$env:TargetDir/Jacobi.Vst.Framework.dll" "$dist"

# Debugging symbols
cp "$env:TargetDir/MidiShapeShifter.net.pdb" "$dist"

ls "$dist"
