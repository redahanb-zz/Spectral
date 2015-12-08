Level Editor Read-me

Opening the Room Editor
To open the Room Editor, when the Unity Editor is open click on 
Window -> Level Editor -> Room Editor
in the toolbar.
I recommend using fullscreen mode first in Unity if using a Mac.

Creating a new Level
I recommend doing this with a brand new scene open.
When the Room Editor is open make sure nothing is selected in the scene.
The editor should display a reference showing what objects will open different panels in the Room Editor.
To create a new Level object, click on the button at the bottom of the window.
Creating a level will create a Level game object, a canvas, game manager, event system, player character and player camera.

Creating Room Cells
When a level is generated, a single room cell is generated with it.
To select this cell, navigate to Level -> Rooms -> [0,0] in the hierarchy.
With this object selected, the Room Editor will change. There will now be four buttons allowing you to create additional room cells north, south, east or west of the selected cell.

Editing a Room
When editing a room, floor and wall components are edited separately.
To make wall changes, select Level -> Rooms -> [0,0](your current room goes here] ->Walls -> Wall Pillar (Clone). With this selected the Room Editor changes so you can create new wall sections north, south east or west of the current room. Selecting a Wall Section game object in Walls in the hierarchy will bring up a separate window, allowing you to cycle through multiple wall pieces (or doors and windows) and set up coloured hiding places.

Making floor changes is similar to wall changes, select Level -> Rooms -> [0,0](your current room goes here] ->Tiles -> Floor Tile(Clone). A floor tile menu will appear. Here you can create additional tiles north, south, east or west of the current tile. You can also toggle between normal tiles or curved corner tiles.

Setting up the Camera
Creating a room will create a single camera trigger in that room. When the player enters a camera trigger, the players camera perspective will shift to the position and rotation of the child camera object of that trigger in-game. The starting camera trigger is located at Level -> Rooms -> [0,0](your current room goes here] ->Camera Triggers -> Camera Trigger(Clone). Position the child camera of this trigger to your liking and the players camera will automatically shift to this position when the player enters the trigger in-game.

Finishing Touches
When you are finished making changes to the level, select a room object ([0,0], [3,2], etc.). When you see the menu for creating a new room, instead press the button to Remove Unused Components.
*IMPORTANT - Do not press this button until you are completely finished editing all rooms in your level. This combines all of the tiles in each room into a single mesh in order to improve performance during gameplay. It also deletes all unused objects in the Level object in the hierarchy.






