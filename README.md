# DataFusionKF

![DataFusion](https://github.com/mplantady/DataFusionKF/blob/master/Preview.gif)

## Environment
Simulation of the position tracking of an object, using an IMU sensor and a depth camera (red path).
Both devices give noisy measurement and can experience dropout for a significant duration.

## Prediction and filtering
Using Kalman filter, the goal is to try to reconstruct (blue path) in realtime the real trajectory and position (green path).
