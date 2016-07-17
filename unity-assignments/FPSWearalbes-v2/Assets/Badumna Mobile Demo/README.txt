                      ReadMe.txt
			-------------------------------
			
This package includes a complete project that demonstrates how to make a multiplayer game for mobile devices (iOS and Android) using Badumna.
You will need the appropriate Unity Pro license - iOS Pro / Android Pro.

To build the game, simply open a new Unity project and import the package and build the game to your preferred platform.
The project is configured to build for Android (you will need the Android SDK installed as per Unity3D requirements).
To target iOS, do the following:
1. Go to the Assemblies folder.
2. Rename the file Badumna.Android.dll to Badumna.Android.dll[Android]
3. Rename the file Dei.Android.dll to Dei.Android.dll[Android]
4. Rename the file Badumna.Unity.iOS.dll[iOS] to Badumna.Unity.iOS.dll
5. Rename the file Dei.Unity.iOS.dll[iOS] to Dei.Unity.iOS.dll
6. Set the following Unity option via Edit > Project settings > Player: AOT compilation options: nimt=trampolines=4096
7. Build the game.

By default, the demo will run on a local LAN. To test the demo on a 3G network, you will need to start a Seed Peer.
For more information on starting a Seed Peer and how to extend this demo to your game requirements, 
please refer to the reference manual included in this package (ReferenceManual/index.html).
