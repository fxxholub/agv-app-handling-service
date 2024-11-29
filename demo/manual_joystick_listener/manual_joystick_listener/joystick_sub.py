import rclpy
from rclpy.node import Node
from geometry_msgs.msg import Twist

class ListenerNode(Node):
    def __init__(self):
        super().__init__('joystick_sub')
        self.subscription = self.create_subscription(
            Twist,
            '/cmd_vel',
            self.listener_callback,
            10
        )
        self.get_logger().info(
            f"Started node 'joystick_sub' listening to '/cmd_vel':"
        )
        self.subscription  # prevent unused variable warning

    def listener_callback(self, msg):
        self.get_logger().info(
            f"Received Twist: linear={msg.linear}, angular={msg.angular}"
        )

def main(args=None):
    rclpy.init(args=args)
    node = ListenerNode()
    rclpy.spin(node)
    node.destroy_node()
    rclpy.shutdown()

if __name__ == '__main__':
    main()