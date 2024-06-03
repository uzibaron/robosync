# robosync .net

## Description
This is a simple tool to sync files between local and remote directories.

## Installation
* .NET build to create the executable eg. `dotnet build -c Release`

## Config
* Create a `config.json` file in the same directory as the executable
* Example config:
```json
{
    "LocalFolders": [
        "C:\\LocalFolder\\folder1",
        "C:\\LocalFolder\\folder2"
    ],
    "ServerSharedRoot": "\\Share\\here",
    "Servers": [
        "192.168.1.1",
        "192.168.1.2",
        "192.168.1.3"
    ]
}
```

## Running
* Run robosync-app.exe