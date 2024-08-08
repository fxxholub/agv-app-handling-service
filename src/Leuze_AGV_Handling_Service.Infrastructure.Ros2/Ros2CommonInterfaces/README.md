# ROS2 Common Interfaces

A set of packages which contain common interface files (.msg and .srv) used furing development of package/node/rcl.

Is made out of two repositories, see source.

## Purpose

Leuze_AGV_Handling_Service.Infrastructure.Ros2 uses these to generate RosNet messages.

Isolating the messages to communicate between stacks in a shared dependency allows nodes in dependent stacks to communicate without requiring dependencies upon each other.
This repository has been designed to contain the most common messages used between multiple packages to provide a shared dependency which will eliminate a problematic circular dependency.

## Source

https://github.com/ros2/common_interfaces

https://github.com/ros2/rcl_interfaces/tree/humble

https://github.com/ros2/unique_identifier_msgs/tree/humble/