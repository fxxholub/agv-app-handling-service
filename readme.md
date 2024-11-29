# Leuze AGV Handling Service

TODO:

# Demo

`docker compose up --build --no-start`
`docker compose up app`

# Process configuration

## What is a process?

When `Session` is started, the actual things that starts are configurable background processes.

Those background processes are meant to be ROS2 preconfigured packages started by `ros2 run`, `ros2 launch`, or similar technique.

## How is a process started?

### HandlingMode
Every handling mode, such as `Autonomous` or `Manual`, has can have its own set of processes.

Also, `Common` processes can be defined, those will run for any handling mode selected.

### Driver

There are two ways to run the processes:
- Docker
- SSH

Docker based driver can be used to run an already existing containers.

SSH driver can be used to run any linux command.

## Where is a processes started?

The process can be started on the local machine or a remote computer.

- Docker - to run a container, only its ID is needed
- SSH - one just needs an existing SSH key configuration

## How to configure the processes?

JSON for the win!

```json
{
  "common": [
    {
      "name": "some_process",
      "hostName": "my_local_docker_desktop", // yes, works crossplatform
      "driver": {
        "type": "docker",
        "address": "unix:///var/run/docker.sock",
        "containerId": "ab50ae8e47ac"
      }
    },
    {
      "name": "dummy_ros_process",
      "hostName": "my_local_pc",
      "driver": {
        "type": "ssh", // works only with linux targets
        "address": "172.17.0.1", // since this app is dockerized
        "auth": {
          "username": "my_pc_username",
          "privateKeyFile": "ssh_private_key_from_my_pc_to_my_pc.pk"
        },
        "commands": [
          "source /opt/ros/humble/setup.bash", // start ROS on my PC
          "ros2 topic echo /joy"
        ]
      }
    },
  ],
  "manual": [
    {
      "name": "some_other_containerized_process",
      "hostName": "local_linux_docker_engine",
      "driver": {
        "type": "docker",
        "address": "unix:///var/run/docker.sock",
        "containerId": "ya50ae1e67ad"
      }
    }
  ]
}
```

How to:
1. create dir in the root of the project, e.g. `MyProcessConfig`
2. create `config.json` file with your configuration
3. add your private key files (if any)
4. set the volume in the `docker-compose.yml` to register your config dir
5. `docker compose up`

A few rules apply:
- Docker supports both Desktop and raw Engine (Win, Mac, Linux)
- Docker works only locally now
- SSH works only if both host and target are linux
    - host - because SSH from dockerized app to the host machine only works with Docker Engine
    - target - because SSH driver uses unix commands to controll the remote shell

A few notes:
- SSH is not really a safe option
- Docker currently uses unix socket, not that safe either

## Environment variables

Make `.env` file in the project root. This file is already __gitignored__.

example file with all used variables:

```
none for now
```