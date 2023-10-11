# evertkart

Game name: Evert Kart
Made by: Evert Andersson
Unity 2022.3.9f1

My goal was to make a fun racing game with some simple 3d car physics. The way the game mechanics works is pretty straight forward, you are two players who are going to race against each other and finish the lap first. Since the maps are very long I decided that you only have to go for one lap instead of three. There are three maps in total. Each one gets a little harder. The first one is completely surrounded by walls so it's impossible to fall out, while the last map takes place in space and is barely protected by any walls, so it's easy to fall down. I've added a respawn system though, so if you fall down or land your car upside down or on the side the car explodes and you respawn either at the start line or the checkpoint depending on if you've crossed it or not. The game keeps track of the players score, and it's the best out of three. 

To play the game, the first scene you have to load is the Main Menu. In the menu you can choose either to play all three maps in a row (press PLAY), and you can choose individual maps on (CHOOSE LEVEL). 
In case the scenes are not in the build settings it should be in this order: MainMenu, Level01, PauseMenu, Level02, Level03.

Player 1 Controllers:
W - accelerate
S - reverse
S/D - turn left/right
Space - handbrake

Player 2 Controllers:
Up arrow - accelerate
Down arrow - reverse
Left/Right arrow - turn left/right
Right shift - handbrake

Escape - pause

Car controller script - this one is definitely the largest script of them all, it keeps track of the player controllers, when the car is supposed to respawn and where the respawn location should be, and when the different sound effect are supposed to play as well. For the controllers I got help from a Youtube video. The way it works is that there are three action maps in total, one for acceleration, one for turning and one for handbrake. Each one of these either return 1 or 0 or -1 depending on what you click. So if you press the accelerate button it returns 1. Then it multiplies the 1 with the variable "max torque" and it applies the speed to the wheels of the car. The same goes for turning, when you press right it returns 1 and then multiples with maxAngle and the front wheels then turn in the direction you are pressing. I decided to use wheel colliders for the car since I found that to be the most efficient way to make your car physics look natural. I tried to make a script where I made the physics all by myself, which actually went well, the problem was though that I couldn't find a good way to implement the controls to both players without duplicating the scripts and at the same time use Unity's new input system. So I landed on wheel colliders instead. What it does is that it basically spawns in four wheels for each car when you start the game using a "for (int i)" that runs 4 times. The wheel colliders are already placed and the wheels spawn in on every location of each of those wheel colliders. Then the torque variable causes the wheels to spin in the speed that the variable is set. I've set up three variables, one for maxTorque, one for maxAngle and one for handBrake. Then there are three active variables that causes the car to do things, for example when you turn the float angle is set to maxAngle (30). Then to make the wheels follow the car I've made one Quaternion named "q" for rotation and one Vector3 named "p" for position. Then I used a GetWorldPose to make the wheels follow but still stay in the same place of the car. To make the front wheels turn when turn key is pressed I just multiplied the "q" with another Quaternion. 
So thats as simple as I could explain the car controls, on to the respawn system. I made two variables called SpawnPos and SpawnRot. The variables are set at the start of each round, and each time the player has crossed the checkpoint. So if you die before the checkpoint is reached you respawn at the start line, but if you've crossed the checkpoint the variables are updated and then if you die you will respawn at the exact same position and rotation as when you hit the checkpoint-line. To check if the player is upside down, I used a box collider that surrounds the roof and the side of the cars and if the collider touches the tag "Ground" it waits one second and then calls the function to respawn the car. I got help from this from a Youtube video, but in the video he used a Sphere collider so I changed it to a box instead because that was more fitting for a car. 

Camera follow script - I watched a video from Brackeys to make my camera follow two players at the same time. The first thing I did was to make a list for each of the targets that camera should follow. One for Player1, one for Player2 and one for an empty gameObject called "center" which I placed in the middle of the map so the camera sort of focuses on the center as well. Then I made a Vector3 called "centerPoint" that is always going to be in the center of Player1, Player2 and "center". To do this I used something called Bounds. It basically makes a square around all the listed objects and resizes depending on the position of each player. The camera always is in the center of this square. Using a for-loop for every object of the list the bound encapsulates and updates the size. 
In the Move function there is first a Vector3 that calls the GetCenterPoint which returns the center point of the bound which I just described. After that there is another Vector3 called newPosition which updates the position to the centerpoint + offset. The offset is variable where you can adjust the distance from the camera to the car. Then to smooth the camera I used Vector3.SmoothDamp. 
In the Zoom function it uses a float called newZoom, that uses a Math.Lerp which goes between maxZoom and minZoom depending on GetGreatestDistance. The GetGreatestDistance is similar to GetCenterPoint, but instead of returning the center point it returns the x-size of the bound. Then the script uses the camera component and changes the depth of field depending on what value the newZoom is.

Finish Line script and Checkpoint script - I've set up two booleans "readyToFinish" and "touchingFinishLine". The readyToFinish variable is in the playerController script, and if it collides with the checkpoint it is set to true. That means that the next time the car touches the finishline it calls a function in the FinishLine scripts called PlayerScored. The PlayerScored function sets the touchingFinishLine variable to true, and the finishLine Object can then detect if it's colliding with Player1 or Player2. If it's colliding with Player 1 then he/she get a score, and the same goes for the second one. 

Main Menu/Pause scripts - these ones are pretty straight forward. I had to watch a video of how to UI works and how to make buttons. In the script for the menu there are two functions called Play and Quit, and the functions are called when the button in the menu is pressed. When you press the "Choose level"-button it disables the main menu and activates another menu where you can see the different maps. There are three other functions in the mainmenu script called Level01, Level02 and Level03 which loads the levels you have clicked on. The pause menu works in a similar way, the only difference is that it freezes the time so the game doesn't run in the background. I did this by using Time.timescale = 0. And when the resume-button is clicked the timescale is set to 1 again.

BoostCar script - Very simple script that calls a function in the player script called BoostCar. In the boostcar function I used an AddForce to the player. I used Add.Force(transform.forward * boostPower). Transform.forward to apply the boost in the direction the player is facing, and multiplied with a variable called boostPower which is set to 5000000f.

Videos used: 
Making a main menu - https://www.youtube.com/watch?v=zc8ac_qUXQY
Car controller - https://www.youtube.com/watch?v=WCA_cXRNmtU&t=67s
Check if roof collides with floor - https://www.youtube.com/watch?v=Ekfio0gfn-Y&t=1211s
Load next scene - https://www.youtube.com/watch?v=Iv7A8TzreY4&t=149s
Make camera follow two players - https://www.youtube.com/watch?v=d1irG1lE2Ns&t=4s


