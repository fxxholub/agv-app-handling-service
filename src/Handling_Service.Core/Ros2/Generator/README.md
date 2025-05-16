# C# class generator

This tool automatically generates C# classes from ROS2 interfaces.

The raw ROS2 Interfaces are stored in Ros2CommonInterfaces.
Ros2CommonInterfaces contains most of the frequently used messages used during package/node dev.

## How to generate C# source code

install nuget dependency `Rosidl.Runtime`

install the generator `dotnet tool install -g ros2cs`.

cd in the `Generator` directory.

Check the settings in the `ros2cs.spec`.

run `ros2cs`.

# How to use the classes

install nuget dependency `Rcl.NET` in every project, where subscriber, publisher, client etc are defined.

See https://github.com/noelex/rclnet for more information.