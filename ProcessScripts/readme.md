This directory contains shell scripts, which will be run as processes under a session.

The rules:

- every host (or rather host connection) is represented as its own folder in this directory.
- host configuration is done with `config.json` file with keys: `hostName`, `hostAddr` and `hostUser`.
- ssh private key (per host) must be stored in `.priate_key` file, since this file is gitignored.
- first, `Common` subfolder scripts will be run when new session is created and started
- then, `<HandlingMode>` subfolder scripts will be run, based on the the `HandlingMode` of the session
- every `Common`/`<HandlingMode>` subfolder must directly contain the `.sh` files
- `.sh` files will be read line after line, those lines will be contatenated with `&&` (line will be processed only if line before were process succesfully)
- last line is meant to contain some long running looping process - it will be run on background and its PID will be returned and processed/monitored by the system 

example file structure:

```
.
└── ProcessScripts/
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
