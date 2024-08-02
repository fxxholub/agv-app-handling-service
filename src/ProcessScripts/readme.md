This directory contains shell scripts, which will be run as processes.

The rules:

- first, `Common` scripts will be run when new session is created and started
- then, `<HandlingMode>` scripts will be run, based on the the `HandlingMode` of the session
- every `Common`/`<HandlingMode>` directory (containing some scripts) must have subdirs for different process configurations, those subdirs must directly contain the `.sh` files
- process configuration (subdir name) must be in following format: `<host name>-<host address>-<host user name>`, dont use `-` in the attributes
- `.sh` files will be read line after line, those lines will be contatenated with `&&` (line will be processed only if line before were process succesfully)
- last line is meant to contain some long running looping process - it will be run on background and its PID will be returned and processed/monitored by the system 

example tree structure:

```
ProcessScripts
      Common
        djirpi-192.168.20.10-jtvrz/
          1_battery.sh
          2_bms.sh
      Autonomous
        leplinux-192.168.20.20-lepuser/
      Manual
        djirpi-192.168.20.10-jtvrz/
        leplinux-192.168.20.20-lepuser/
      readme.md
```
