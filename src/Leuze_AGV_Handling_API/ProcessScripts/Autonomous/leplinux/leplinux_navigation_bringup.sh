cd ros2_ws

source /opt/ros/humble/setup.bash
source install/local_setup.bash

ros2 launch nav2_bringup my_navigation_loc_launch.py use_sim_time:=False params_file:=nav2_params_neo2.yaml