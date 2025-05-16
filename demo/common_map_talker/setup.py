from setuptools import find_packages, setup

package_name = 'common_map_talker'

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
    description='Simple map talker',
    license='Apache-2.0',
    tests_require=['pytest'],
    entry_points={
        'console_scripts': [
            'map_pub = common_map_talker.map_pub:main'
        ],
    },
)