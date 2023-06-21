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
ls -r "$env:TargetDir" -Exclude *.dll | cp -r -Force -Destination "$dist"

# Copy the main VST dll
cp "$env:TargetDir/Jacobi.Vst.Interop.dll" "$dist/$env:TargetFileName"

ls "$dist"
