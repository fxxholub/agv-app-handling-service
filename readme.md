# Leuze AGV Handling Service

__TODO:__

## Process scripts SSH connections

Process scripts describe processes, which are managed (started, checked and killed) by the application.

Those processes are in most cases just different running ROS2 nodes.

Different processes are used for different handling mode and for different robots. Also, processes can be run on different computers - not just locally.

Example setup can be found in `tests/ProcessScripts/` directory.

The following guide explains how to setup process scripts if needed.

- Create your own __directory__ - lets call it __MyProcessScripts__ for purpose of this guide.
- Every __host__ (or rather host connection) is represented by its own folder in this directory.
- Host __configuration__ is done with `config.json` file with keys: `hostName`, `hostAddr` and `hostUser`. The file must be placed in the host`s folder.
- SSH __private key__ (per host) must be stored in `.priate_key` file in host`s folder, since this file is __gitignored__.
- As of writing this guide, only __Manual__ and __Autonomous__ handling modes are present in the app. For __each host__, create `Manual` and `Autonomous` subfolders if you want host to run them, plus add `Common` subfolder if needed. __Common__ scripts will be run for every handling mode.
- First, `Common` subfolder scripts will be run when new session is created and started.
- Then, `<HandlingMode>` subfolders scripts will be run, based on the the `HandlingMode` of the started __handling Session__.
- Every `Common`/`<HandlingMode>` subfolder must directly contain the `.sh` process files. Each file descripts one process.
- `.sh` files will be read line after line, those lines will be contatenated with `&&` (line will be processed only if line before were processed succesfully)
- Last line is meant to contain some long running (possibly while looping) process - it will be run on background and its PID will be returned and monitored by the system.
- Finally, add environment variable `PROCESS_SCRIPTS_PATH=/MyProcessScripts` to the `.env` file. App will then register the processes at the docker build time.

example file structure:

```
.
└── MyProcessScripts/
    └── host1/
        ├── Common/
        │   └── 1_some_common_script.sh
        ├── Autonomous/
        │   ├── 2_some_autonomous_process.sh
        │   └── 3_some_other_autonomous_process.sh
        ├── Manual/
        │   └── 4_some_manual_process.sh
        ├── config.json
        └── .private_key
```

__Note:__ Add `MyProcessScripts` to gitignore if you are going to push to the repo.

## Environment variables

Make `.env` file in the project root. This file is already __gitignored__.

example file with all listed variables:

```
PROCESS_SCRIPTS_PATH=/MyProcessScripts
```