# In MS PowerShell

p4 set P4CLIENT=HXR_Dev_Studio_EXP
cd..
dir
# Use the p4 command-line client to update the workspace files to the revision specified
# You might need to set P4CONFIG or P4PORT environment variables to make it work

p4 sync D:\Perforce\HXR_Dev_Studio_EXP\...#head

# Build the project
dotnet run "C:/Users/RemoteAdmin/Documents/GitHub/robosync/robosync-app/robosync-app.csproj" --configuration Debug --framework net8.0


# Run the application - don't need to build each time
#dotnet run "C:/Users/RemoteAdmin/Documents/GitHub/robosync/robosync-app/bin/Debug/net8.0/robosync-app.exe"