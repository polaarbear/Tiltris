# Tiltris
This is a work-in progress Tetris clone that I've been mulling over for awhile.

The ultimate goal is to port it to mobile devices to use the gyroscope and accelerometer as an input device.  The initial plan was to use the Unity physics engine to move pieces around and apply gravity based on the applied forces.  That option is probably feasible but was problematic due to a lot of "edge sticking" even with all the friction co-efficients zeroed out, floating point imprecision was enough to get pieces snagged.

Instead I wrote a manual collision detector using a simple HashSet<Vector2> of all the occupied squares and I compare positions against the currently active piece's location.  This is working really well aside from some known issues with edge/rotation detection that I still need to resolve.

Feel free to borrow anything you want, there are no real "assets", just basic Unity 2D Sprites for now.
