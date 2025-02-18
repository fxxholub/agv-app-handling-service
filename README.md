# Handling Service

Handling service is a ROS2 programs __management__ and __messaging__ application.

- It executes ROS2 services as processes in background
- It routes ROS2 pub/sub messages to the SignalR API

# Contents

- [About](#about)
- [Demo](#demo)
- [Process configuration](#process-configuration)
- [Environment variables](#environment-variables)

# About

TODO

# Demo

Run the app and manage dockerized ROS2 dummy packages. Then exchange joystick and map messages with them.

## Prerequisities

- Linux or Mac
- Docker (Engine or Desktop)

## How to setup

The [`docker-compose.yml`](./docker-compose.yml) is already prepared to run the demo [configuration](#process-configuration) of the app.

a) if you use Docker container name in `config.json` (default)
  1) `docker compose up --build --no-start`
  2) `docker compose up app`

b) if you use Docker container ID
  1) `docker compose up --build --no-start`
  2) `docker ps -a`
  3) paste the __CONTAINER ID__ of __manual_joystick_listener__ and __common_map_talker__ to [`./demo/ProcessConfig/config.json`](./demo/ProcessConfig/config.json)
  4) `docker compose up app`

## How to use

1) create Postman websocket request to `ws://localhost:8080/api/v1/handling/signalr`
2) hit `Connect`
3) send SignalR connection message (including the whitespace after `}`):
    ```json
    {"protocol": "json", "version": 1 }
    ```
4) send `StartSession`:
    ```json
    {
      "type": 1,
      "target": "StartSession",
      "arguments": [
        {
          "handlingMode": "Manual"
        }
      ],
      "invocationId": "1234"
    }
    ```
5) docker services __manual_joystick_listener__ and __common_map_talker__ should be started. Check with `docker ps`.
7) inspect __common_map_talker__. Postman should be receiving `SubscribeMapTopic` message.
6) inspect __manual_joystick_listener__. Do:
    - `docker logs -f leuze_agv_handling_service-demo_ros_joy_listener-1`
    -  send `PublishCmdVel` message with Postman.
        ```json
        {
          "type": 1,
          "target": "PublishCmdVel",
          "arguments": [123.123, 321.321, 231.231]
        }
        ```
    - watch the log in the terminal. It should print something like:
        `[joystick_sub]: Received Twist: linear=geometry_msgs.msg.Vector3(x=-1.2312300205230713, y=-3.213210105895996, z=0.0), angular=geometry_msgs.msg.Vector3(x=0.0, y=0.0, z=-2.312309980392456)`
8) send `EndSession` message:
    ```json
    {
      "type": 1,
      "target": "EndSession",
      "arguments": [],
      "invocationId": "12345"
    }
    ```
9) docker services __manual_joystick_listener__ and __common_map_talker__ should be down. Check with `docker ps`.
10) send `StartSession` again.
11) kill any ROS2 service and see the other come down too. `docker kill <container_id>`. You should be receivng `SessionUnexpectedEnd` message in Postman. Check if the services are down with `docker ps`.

# Process configuration

## What is a process?

When `Session` is started, the actual things that starts are configurable background processes.

Those background processes are meant to be ROS2 preconfigured packages started by `ros2 run`, `ros2 launch`, or similar.

## How is a process started?

### HandlingMode
Every handling mode, such as `Autonomous` or `Manual`, can have its own set of processes.

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
        "containerId": "ab50ae8e47ac" // or container name (which is more persistent)
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
4. set the volume in the [`docker-compose.yml`](./docker-compose.yml) to register your config dir
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