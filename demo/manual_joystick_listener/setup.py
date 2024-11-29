from setuptools import find_packages, setup

package_name = 'manual_joystick_listener'

setup(
    name=package_name,
    version='0.0.0',
    packages=find_packages(exclude=['test']),
    data_files=[
        ('share/ament_index/resource_index/packages',
            ['resource/' + package_name]),
        ('share/' + package_name, ['package.xml']),
    ],
    install_requires=['setuptools'],
    zip_safe=True,
    maintainer='Jan Holub',
    maintainer_email='your_email@example.com',
    description='Simple joystick Twist listener',
    license='Apache-2.0',
    tests_require=['pytest'],
    entry_points={
        'console_scripts': [
            'joystick_sub = manual_joystick_listener.joystick_sub:main'
        ],
    },
)