# Use ROS 2 Humble base image
FROM ghcr.io/sloretz/ros:humble-ros-base

# Install dependencies
RUN apt-get update && apt-get install -y \
    python3-pip python3-colcon-common-extensions \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /ros2_ws/src/common_map_talker

# Copy the package files
COPY . /ros2_ws/src/common_map_talker

# Build the Python package
WORKDIR /ros2_ws
RUN bash -c "source /opt/ros/humble/setup.bash && colcon build"

# Source the workspace and run the node
CMD ["bash", "-c", "source /opt/ros/humble/setup.bash && source install/setup.bash && ros2 run common_map_talker map_pub"]
