# 1. Description of the Project
In this VR stealth-puzzle game, the player embodies a desperate student on a night-time mission to change his bad grade. Armed only with a flashlight and quick thinking, the player must infiltrate the university building, solve environmental puzzles, avoid security, find a way to the professor's office, and eventually hack the professor's computer, allowing him to change his grade.

# 2. References
## 2.1 Produced scripts and assets created by the team

### Climb corridor
- Same scripts/assets as Bots Corridor


### Lab room
#### Scripts:
- [PourDetector](Assets/LaboAsset/Scripts/PourDetector.cs/): If the current object (erlenmyer) is rotated by more than 90° in absolute value, the watering effect is activated, including activation of the water stream animation.
- [Fill](Assets/LaboAsset/Scripts/Fill.cs/):  If the collision box of the current object (erlenmeyer_final) comes into contact with the collision box of the water stream animation of an erlenmeyer of the correct colour, the erlenmeyer_final begins to fill and has changed colour from the original colour of the erlenmeyer_final to the colour of the target mixture. If the colour of the erlenmeyer in contact is not the one expected, the liquid turns black and black smoke appears. If the water flow collision box of the erlenmeyer_final is in contact with the collision box of the sink, the liquid starts to flow until it is completely emptied. If the colour of the erlenmeyer_final is the right colour (dark_green), a green smoke appears with a number 4.
- [GlassButtoncorrosion](Assets/LaboAsset/Scripts/GlassButtoncorrosion.cs/): If the box collider of the current object (box_glass) is in contact with the box collider of the water stream animation of the erlenmeyer_final of the correct colour (dark_green), then the box_glass is inactivated.
#### Assets:
- [Lab instructions Images UI](<Assets/LaboAsset/Image UI/Labo/>): Images created to help the player
- [Box Glass](Assets/LaboAsset/Models/box_glass.fbx/): Box with a transparent glass created on Blender used to protect the door opening button.


### Server room
#### Scripts:

- [SocketInteractionSlider](<Assets/Rayan_assets/Scripts_Server/>): Controls the sliding motion of an object when a plug is inserted into or removed from a socket interactor. Slides the object down on plug insertion and slides it back up when the user grabs the plug.
- [SocketInteractionExecutor](<Assets/Rayan_assets/Scripts_Server/>): Executes predefined UnityEvents when an object is inserted into or removed from a socket. Useful for triggering actions like animations, sounds, or material changes in response to socket interaction.
- [ScreenMaterialSequence](<Assets/Rayan_assets/Scripts_Server/>): Switches the screen material from a loading material to a final one after a set delay when triggered, and reverts to an off material when the plug is removed.
- [MultiSocketChecker](<Assets/Rayan_assets/Scripts_Server/>): Monitors multiple socket interactors and triggers an action only when all sockets are occupied. Reverses the action when any socket becomes empty. Useful for coordinated multi-plug mechanics. Code was designed with the help of ChatGPT.
- [FreeGrabOverride](<Assets/Rayan_assets/Scripts_Server/>): Overrides the attach transform of an XRGrabInteractable to allow free-hand grabbing without snapping to a predefined attach point, while maintaining proper behavior when placed in sockets. Code was designed with the help of ChatGPT.
- [DoorSlider](<Assets/Rayan_assets/Scripts_Server/>): Moves a door between open and closed positions by sliding it along a vector direction. Used to represent mechanical door movement triggered by plug insertion or removal through the ISocketAction interface.
- [ActivateWallsOnGrab](<Assets/Rayan_assets/Scripts_Server/>): Activates a group of wall GameObjects when the player grabs a specific interactable, and deactivates them upon release — while ignoring interactions triggered by sockets.
- [ScreenMaterialChanger](<Assets/Rayan_assets/Scripts_Server/>): Changes the material of a screen object between an "active" material and a "black/off" material. Useful for visually turning a screen on or off based on external events or logic.
- [ISocketAction](<Assets/Rayan_assets/Scripts_Server/>): An interface that defines two methods — ExecuteAction() and UndoAction() — used to standardize plug-in/plug-out behavior for components reacting to XR socket events. Code was designed with the help of ChatGPT.
- [DoorRotator](<Assets/Rayan_assets/Scripts_Server/>): Rotates a door around a specified axis by a given angle using smooth interpolation. The door can be opened or closed by triggering OpenDoor() or CloseDoor(), making it suitable for animated mechanical responses.
- [CableFollowOnGrab](<Assets/Rayan_assets/Scripts_Server/>): Updates an ACC_Trail cable in real time while the object is being grabbed via XRGrabInteractable. Ensures the visual cable follows the grab point dynamically during interaction. Requires the NOT_Lonely Advanced Cable Creator (ACC) asset. Code was designed with the help of ChatGPT.
- [CableFollow](<Assets/Rayan_assets/Scripts_Server/>): Continuously updates an ACC_Trail cable when the isActive flag is true. Used to manually control the cable update process, typically when grab-based logic is handled externally. Requires the NOT_Lonely Advanced Cable Creator (ACC) asset. Code was designed with the help of ChatGPT.

  
### Bots corridor
#### Scripts:
- [BotDetectionArea](Assets/BotCorridorAsset/Scripts2/): *The part of the code responsible for reloading the scene and restarting the player position was developed with the help of ChatGPT*. If the current object's box collider is active and the Player object comes into contact with it, Gameover is activated and the scene is reload.
- [BotDetectionArea](Assets/BotCorridorAsset/Scripts2/): If the current object's box collider, it means the light switch, is active and one of the "Light_switch" tag object comes into contact with it, then the light is switch off and the box collider of the corresponding bot is desactivate.
#### Assets:
- [Ball and FootBall](Assets/BotCorridorAsset/prefabs2/): Grab balls to throw at the switch.

  
### Prof's Desk
#### Scripts:
- 
#### Assets:
- 


### General
#### Scripts:
- [SoundTrigger](Assets/Scripts/SoundTrigger.cs/): Trigger the activation of the voice if in contact with some collider.
- [VoiceOverTrigger](Assets/Scripts/VoiceOverTrigger.cs): Other way of activation of the voice overs, works with the VoiceOverManager and also allows to play a sound when a collider is hitted with the flashlight beam
- [VoiceOverManager](Assets/Scripts/VoiceOverManager.cs): Attached to the player, stores all the Audio clips needed for the voice over and the audio source, play the clips when asked by an event or by a VoiceOverTrigger
- [SceneTeleporter](Assets/Scripts/SceneTeleporter.cs): Attached to the teleporter collider, it manages the teleportation with the other scenes
- [SphereIntroScene](Assets/Scripts/SphereIntroScene.cs): Manage the white environnement and text at the beginning and end of each scenes (represented as a white inverse sphere around the camera), works with the SceneTeleporter to detect its presence and fade in depending on the distance to the teleporter.
- [FlashLightController](Assets/Scripts/FlashLightController.cs): Controls the flashlight of the player, allowing the player to turn it on or off
- [DoorHandler](Assets/Scripts/DoorHandler.cs): Controls the opening of a door, the rotation part was written with the help of OpenAI ChatGPT
#### Assets:
- /
## 2.2 Adapted scripts and assets from external sources
### Classroom
#### Scripts:
- [Keypad](Assets/Keypad/Scripts/Keypad.cs)
- [KeypadButton](Assets/Keypad/Scripts/KeypadButton.cs)
- [KeypadInteractionFPV](Assets/Keypad/Scripts/KeypadInteractionFPV.cs)
These three scripts manage the digicode interaction and were adapted from the original asset [Keypad](Assets/Keypad)
#### Assets: 
- [Keypad](Assets/Keypad) : The digicode
- [Sticky_note_blue](Assets/Office%20Supplies%20Low%20Poly/Assets/Prefabs/Sticky_note_blue.prefab/)
- [Sticky_note_red](Assets/Office%20Supplies%20Low%20Poly/Assets/Prefabs/Sticky_note_red.prefab)
- [Sticky_note_yellow](Assets/Office%20Supplies%20Low%20Poly/Assets/Prefabs/Sticky_note_yellow.prefab)
- [Sticky_note_green](Assets/Office%20Supplies%20Low%20Poly/Assets/Prefabs/Sticky_note_green.prefab)
Four created prefab to represent sticky notes where the code number are present. The texture was created using OpenAI ChatGPT image generation


### Climb corridor
- Same Scripts/Assets as Bots Corridor


### Lab room
#### Assets:
- [Erlenmeyer](Assets/LaboAsset/prefabs/): Erlenmeyer from [3D Laboratory Environment with Appratus](Assets/LaboAsset/) were modified with Blender to increase the size of the liquid flask to make the wooble effect more realistic: [link](https://assetstore.unity.com/packages/3d/environments/chemistry-lab-items-pack-220212)
- [Shader VFX](Assets/LaboAsset/Shader): The Liquid shader material was created following an online tutorial and was adapted to the project: [link](https://youtu.be/tI3USKIbnh0)
- [WaterAnimator and VFX graph](Assets/LaboAsset/prefabs/vfxgraph_AnimateWater.vfx/): The water stream animation was created following an online tutorial and was modified (remove of some 3D models and change of colours) to adapted to the project : [link](https://youtu.be/_H8gBKGKbnU)


### Bots corridor
#### Assets:
- [Lockers](Assets/BotCorridorAsset/prefabs2/): The lockers come from the basic assets used for school objects. They were modified to add shelves inside to store objects. This was done using Blender: [link](https://styloo.itch.io/classroom-asset-pack)


### Prof's Desk
#### Scripts:
- 
#### Assets:
- 

  
### General
#### Scripts:
- /
#### Assets:
- [DoorPackFree](Assets/01_AssetStore/DoorPackFree): Prefab adapted to create the majority of the doors in the game.
  
## 2.3 Unmodified scripts and assets from external sources
### Classroom
#### Assets:
- [Office supply](Assets/Office%20Supplies%20Low%20Poly) Fornitures for the classroom 

### Climb corridor: 
- Same as Bots Corridor

### Lab room
#### Scripts:
- [Wobble](Assets/LaboAsset/Scripts/Wooble.cs/): Wobble effect of the liquid in the erlenmeyer flask. *This script was used as is from an online tutorial:*: [link](https://youtu.be/tI3USKIbnh0)
#### Assets:
- [Classroom and Laboratory assets](Assets/Rayan_assets/Importing%Assets/): [link](https://styloo.itch.io/classroom-asset-pack)
- [3D Laboratory Environment with Appratus](Assets/LaboAsset/): [link](https://assetstore.unity.com/packages/3d/environments/chemistry-lab-items-pack-220212)
- [Water Texture](Assets/LaboAsset/Textures/): [link](https://youtu.be/_H8gBKGKbnU)
- [PushButton](Assets/Samples/XR%20Interaction%20Toolkit/3.0.8/Starter%20Assets/DemoSceneAssets/Prefabs/Interactables/Push%20Button.prefab): Push button from the VR demo scene from unity.
- [Water Pour Sound Effect](Assets/LaboAsset/Sounds/): [link](https://pixabay.com/sound-effects/water-tap-93502/)
- [Smoke explosion Sound Effect](Assets/LaboAsset/Sounds/): [link](https://pixabay.com/sound-effects/smoke-bomb-6761/)


### Server room
#### Scripts:
- [wirebuilder](<Assets/>): Allows the creation of cables and plugs for interactive setups.
- [Ipoly3D](<Assets/>): Contains most of the server prefabs used in the scene.
- [NOT_LONELY](<Assets/>): Provides the Advanced Cable Creator system used to visually simulate realistic cables.
- [XLR_male, XLR_female](<Assets/>): Models of XLR plugs and sockets used for visual and interactive connection points.
- [ProBuilder](<Assets/>): Unity tool used to modify and prototype custom 3D geometry directly in the editor.
#### Assets:
- [wirebuilder](<Assets/>): Allows the creation of cables and plugs for interactive setups.
- [Ipoly3D](<Assets/>): Contains most of the server prefabs used in the scene.
- [NOT_LONELY](<Assets/>): Provides the Advanced Cable Creator system used to visually simulate realistic cables.
- [XLR_male, XLR_female](<Assets/>): Models of XLR plugs and sockets used for visual and interactive connection points.
- [ProBuilder](<Assets/>): Unity tool used to modify and prototype custom 3D geometry directly in the editor.

### Bots corridor
#### Assets:
- [BodyGuards and BotsAnimation](Assets/BotCorridorAsset/): [Mixamo](https://www.mixamo.com/#/?page=1&query=alex&type=Character)
- [ClassroomObjects](Assets/BotCorridorAsset/prefabs2/): [link](https://styloo.itch.io/classroom-asset-pack)
- [SocketsAndSwitches](Assets/BotCorridorAsset/): [link](https://assetstore.unity.com/packages/3d/props/interior/free-sockets-and-switches-233085)
- [Switch Light Sound Effect](Assets/BotCorridorAsset/Sounds/): [link](https://pixabay.com/sound-effects/projector-button-push-6258/)


### Prof's Desk
#### Scripts:
- 
#### Assets:
- 


### General
#### Scripts: 
- XRInteractionToolkit
#### Assets:
- [AI Voices](Assets/)

# 3. Custom interactions
## 3.1 Non-movement interaction
### Potion mix:
#### Scene: [Lab Room](Assets/LaboAsset/)
#### Description:
The player must follow the instructions displayed on the whiteboards to create an acid solution in order to make the glass of the box containing the button to open the door disappear. To create the acid, the player must mix different solutions of different colours in the correct order. A number for the final code appears as smoke when the acid solution is finished.
#### Scripts:
- [PourDetector](Assets/LaboAsset/Scripts/PourDetector.cs/): If the current object (erlenmyer) is rotated by more than 90° in absolute value, the watering effect is activated, including activation of the water stream animation.
- [Fill](Assets/LaboAsset/Scripts/Fill.cs/):  If the collision box of the current object (erlenmeyer_final) comes into contact with the collision box of the water stream animation of an erlenmeyer of the correct colour, the erlenmeyer_final begins to fill and has changed colour from the original colour of the erlenmeyer_final to the colour of the target mixture. If the colour of the erlenmeyer in contact is not the one expected, the liquid turns black and black smoke appears. If the water flow collision box of the erlenmeyer_final is in contact with the collision box of the sink, the liquid starts to flow until it is completely emptied. If the colour of the erlenmeyer_final is the right colour (dark_green), a green smoke appears with a number 4.
- [Wobble](Assets/LaboAsset/Scripts/Wooble.cs/): Wobble effect of the liquid in the erlenmeyer flask. *This script was used as is from an online tutorial:* [link](https://youtu.be/tI3USKIbnh0)
- [GlassButtoncorrosion](Assets/LaboAsset/Scripts/GlassButtoncorrosion.cs/): If the box collider of the current object (box_glass) is in contact with the box collider of the water stream animation of the erlenmeyer_final of the correct colour (dark_green), then the box_glass is inactivated.
#### Assets:
- [Erlenmeyer](Assets/LaboAsset/prefabs/): Erlenmeyer from [3D Laboratory Environment with Appratus](Assets/LaboAsset/) were modified with Blender to increase the size of the liquid flask to make the wooble effect more realistic
- [Shader VFX](Assets/LaboAsset/Shader): The Liquid shader material was created following an online tutorial and was adapted to the project: [link](https://youtu.be/tI3USKIbnh0)
- [WaterAnimator and VFX graph](Assets/LaboAsset/prefabs/vfxgraph_AnimateWater.vfx/): The water stream animation was created following an online tutorial and was modified (remove of some 3D models and change of colours) to adapted to the project : [link](https://youtu.be/_H8gBKGKbnU)

### Hand/Controller:
#### Scripts:
- 
#### Assets:
- 

### Simon:
#### Scripts:
- 
#### Assets:
- 
  
## 3.2 Movement interaction
### Tyrolien
#### Scripts:
- 
#### Assets:
- 
