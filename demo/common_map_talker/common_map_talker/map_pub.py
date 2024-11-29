import rclpy
from rclpy.node import Node
from nav_msgs.msg import OccupancyGrid
import numpy as np

class MapTalkerNode(Node):
    def __init__(self):
        super().__init__('map_talker')
        self.publisher = self.create_publisher(OccupancyGrid, '/map', 10)
        self.timer = self.create_timer(0.1, self.publish_map)  # Publish every 0.1 seconds
        self.get_logger().info(f"Started node 'map_talker' publishing to '/map'.")

    def publish_map(self):
        # Create a sample OccupancyGrid message
        msg = OccupancyGrid()
        msg.header.stamp = self.get_clock().now().to_msg()
        msg.header.frame_id = 'map'

        # Define map metadata
        msg.info.resolution = 0.05  # 5 cm per cell
        msg.info.width = 100  # 100 cells wide
        msg.info.height = 100  # 100 cells tall
        msg.info.origin.position.x = -2.5  # Map origin (bottom-left corner)
        msg.info.origin.position.y = -2.5
        msg.info.origin.position.z = 0.0
        msg.info.origin.orientation.w = 1.0  # No rotation

        # Fill the map with data (e.g., random for demonstration)
        map_data = np.random.randint(0, 100, size=(msg.info.width * msg.info.height))
        msg.data = map_data.tolist()

        self.publisher.publish(msg)
        self.get_logger().info("Published OccupancyGrid message to '/map'.")

def main(args=None):
    rclpy.init(args=args)
    node = MapTalkerNode()
    rclpy.spin(node)
    node.destroy_node()
    rclpy.shutdown()

if __name__ == '__main__':
    main()
