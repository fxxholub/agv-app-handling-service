source /opt/ros/humble/setup.bash
ros2 topic pub /dummy std_msgs/msg/String "data: 'something'" --rate 1