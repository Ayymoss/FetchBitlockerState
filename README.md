# FetchBitlockerState

Client CLI Application to send its BitLocker state to centralised host.
Useful because Microsoft in their infinite wisdom made it non-trivial to get "Suspended" BitLocker state. 

My usecase will be to filter out Suspended state machines and unsuspend it.

Server will write out a .json file with data in the application's root directory. 

***
### Server Information:
Can be ran on Linux or Windows systems.
`FETCH_BL_API_KEY` Environment variable needs to be set (string). Note, it must be in the `User` env vars if hosting on Windows.

### Client Information:
Commandline execution - must be ran as Administrator as starts child process `manage-bde.exe`.
Arguments:
```
-a, --api (Required) Shared server API Key
-h, --host (Required) Server IP or Domain Name
```

***
### Compiling:
Required: .NET Core 6

Provided excutables are packed with runtime dependencies so hosts do not require .NET Core Runtimes. (Hence why they're quite inflated for the project's size)
