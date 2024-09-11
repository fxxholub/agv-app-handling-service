cd ros2_ws

source /opt/ros/humble/setup.bash
source install/local_setup.bash

ros2 launch leuze_bringup leuze_bringup_rsl200.launch.py sensor_ip:=192.168.20.5 port:=9991 topic:=scan1