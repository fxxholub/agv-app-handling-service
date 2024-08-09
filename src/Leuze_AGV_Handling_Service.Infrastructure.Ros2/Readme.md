# Leuze_AGV_Handling_Service.Infrastructure.Ros2

## Docker

This package is meant to be run in docker as part of the whole app.

## Messages

Messages were imported from Ros2CommonInterfaces using the ros2cs tool, see below.
Ros2CommonInterfaces contains most of the frequently used messages used during package/node dev.

Messages are generated into Ros2CommonMessages.

Those were used as input to ros2cs, running from Ros2 directory:

`ros2cs`

For detailed ros2cs configuration see `ros2cs.spec`.

## RCL NET template usage (this is inherited from template source):

Edit `ros2cs.spec` to include ROS 2 interface packages you wish to use.

Install `ros2cs` utility with the following command if not yet installed:
```
dotnet tool install -g ros2cs
```
After installation, simply run `ros2cs` in the project root to generate messages.

This project can also be built as a colcon package. Feel free to modify `package.xml`
and `CMakeLists.txt` to specify package dependencies and add other resources
you would like to include in the package.

See https://github.com/noelex/rclnet for more information.