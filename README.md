# HelmetConvincer
Project Repository for Helmet Convincer project


<h3>Items included in this repository:</h3>

- "Ardunio Code" - This code is used specifically for sending values to the computer or phone it is hooked up to. It does <b>NOT</b> calculate the values that are being sent out. It is merely a messenger.

- "Unity Code" - The project code with the levels, scripts, and assets needed to build and deploy the project. It also contains a folder for successful builds that have been created and can be downloaded to the phone.

<h3>Technologies used</h3>

- Unity version 2017.2.0f3 - downloadable at https://unity3d.com/get-unity/download/archive
- Java SE Development Kit 10 downloadable at http://www.oracle.com/technetwork/java/javase/downloads/jdk10-downloads-4416644.html
- Android SDK - downloadable at https://developer.android.com/studio/
- Biycycles Asset Package - downloadable at https://assetstore.unity.com/packages/3d/bicycles-61572
- Oculust Integration Package - downloadable at https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022
- Simple Animals Asset Package - downloadable at https://assetstore.unity.com/packages/3d/characters/animals/simple-animals-56188
- Simple People Cartoon Characters Package - downloadable at https://assetstore.unity.com/packages/3d/characters/humanoids/simple-people-cartoon-characters-15126
- Simple Sky Cartoon Assets Package - downloadable at https://assetstore.unity.com/packages/3d/simple-sky-cartoon-assets-42373
- Simple Town Cartoon Assets Package - downloadable at https://assetstore.unity.com/packages/3d/environments/urban/simple-town-cartoon-assets-43500

<h3>Downloading the Android SDK</h3>

1. Using the link above, download Android Studio
2. Once install, run Android studio and open the "SDK Manager" in the lower left corner of the Android Studio start screen
3. This application requires a minimal API level of 19, though higher API levels may be required if running on a device newer than a Samsung Galaxy S7 Edge
4. It is recommended that you install all API's of level 19 and above, though just API level 19 would suffice

<h3>Opening the project</h3>

1. Clone this repository to a folder on your computer
2. Open Unity and click the "Open" button
3. Navigate to the <em>Unity Code</em> folder you cloned from this repository
4. Open this folder as a project in Unity

<h3>Building the project to run on a computer</h3>

1. In the Unity Editor, navigate to the <em>Assets/Maps/Worldy.untiy</em> file
2. Double click to open the scene
3. Navigate to <em>File -> Build Settings</em>
4. Under the "Scenes in Build" portion, make sure that the <em>Maps/World</em> scene has been added. If it has not been added, click "Add Open Scenes"
5. Select platform "PC, Mac, & Linux Standalone" and click the "Switch Platform" button
6. Click the "Build" button to build the project or click "Build and Run" to build and then immediately begin the game
7. Save the project in the <em>UnityCode/Builds</em> folder
8. This creats a .exe application file that runs the game

<h3>Building the project to run on an Android phone</h3>

1. In the Unity Editor, navigate to the <em>Assets/Maps/Worldy.untiy</em> file
2. Double click to open the scene
3. Navigate to <em>File -> Build Settings</em>
4. Under the "Scenes in Build" portion, make sure that the <em>Maps/World</em> scene has been added. If it has not been added, click "Add Open Scenes"
5. Select platform "Android"
6. If no Android module is loaded, click the "Open Download Page" and follow Unity's instructions on setting up Android support
7. Switch platform to "Android"
8. Navigate to <em>Edit -> Preference -> External Tools</em>
9. Click "Browse" for the JDK option and select the folder where you have stored the most recent version of the JDK. Do this for the Android SDK as well.
10. If you don't have the most recent JDK or SDK they can been found using the links at the top of this page
11. Navigate back to <em>File -> Build Settings </em>
12. Change "Texture Compression" to ASTC
13. Click the "Player Settings" button
14. Open the "Other Settings" tab on the right
15. Make sure the "Virtual Reality Supported" box is checked and the Virtual Reality SDK's listed include Oculus
16. The Package Name should be com.CompanyName.ProductName (in this case, the project is currently labeled com.TallGrassINC.HelmetConvincer)
17. The "Minimum API Level" should be level 19 (KitKat)
18. The "Target API Level" should be "Highest Installed"
19. The "Script Running Version" should be "Stable (.NET 3.5 Equivalent)"
20. The "Scripting Backend" should be "Mono"
21. The "API Compatibility Level" should be ".NET 2.0"
22. The "Android Gamepad Support Level" should show "Works with D-pad"
23. Once all settings are correct, click the "Build" button to bulid the project or "Build and Run" to build the project and deliver to an Android device connect to the computer through USB
24. Save the build in the <em>UnityCode/Builds</em> folder
25. This will create a .apk file that can be downloaded to an Android device and run using an Oculus VR device, such as the Gear VR headset 
