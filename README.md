# 1. Description of the Project
Game project using virtual reality

# 2. References


## 2.1 Produced scripts and assets created by the team

### Tutos room
### Climb corridor
### Lab room
#### Scripts:
- [PourDetector](Assets/LaboAsset/Scripts/PourDetector.cs/): If the current object (erlenmyer) is rotated by more than 90° in absolute value, the watering effect is activated, including activation of the water stream animation.
- [Fill](Assets/LaboAsset/Scripts/Fill.cs/):  If the collision box of the current object (erlenmeyer_final) comes into contact with the collision box of the water stream animation of an erlenmeyer of the correct colour, the erlenmeyer_final begins to fill and has changed colour from the original colour of the erlenmeyer_final to the colour of the target mixture. If the colour of the erlenmeyer in contact is not the one expected, the liquid turns black and black smoke appears. If the water flow collision box of the erlenmeyer_final is in contact with the collision box of the sink, the liquid starts to flow until it is completely emptied. If the colour of the erlenmeyer_final is the right colour (dark_green), a green smoke appears with a number 4.
- [GlassButtoncorrosion](Assets/LaboAsset/Scripts/GlassButtoncorrosion.cs/): If the box collider of the current object (box_glass) is in contact with the box collider of the water stream animation of the erlenmeyer_final of the correct colour (dark_green), then the box_glass is inactivated.
#### Assets:
- [Lab instructions Images UI](<Assets/LaboAsset/Image UI/Labo/>): Images created to help the player
- [Box Glass](Assets/LaboAsset/Models/box_glass.fbx/): Box with a transparent glass created on Blender used to protect the door opening button.
### Server room
### Bots corridor
#### Scripts:
- [BotDetectionArea](Assets/BotCorridorAsset/Scripts2/): *The part of the code responsible for reloading the scene and restarting the player position was developed with the help of ChatGPT*. If the current object's box collider is active and the Player object comes into contact with it, Gameover is activated and the scene is reload.
- [BotDetectionArea](Assets/BotCorridorAsset/Scripts2/): If the current object's box collider, it means the light switch, is active and one of the "Light_switch" tag object comes into contact with it, then the light is switch off and the box collider of the corresponding bot is desactivate.
#### Assets:
- [Ball and FootBall](Assets/BotCorridorAsset/prefabs2/): Grab balls to throw at the switch.
### Prof's Desk
### General
#### Scripts:
- [SoundTrigger](Assets/Scripts/SoundTrigger.cs/): Trigger the activation of the voice if in contact with some collider.
#### Assets:

## 2.2 Adapted scripts and assets from external sources

### Tutos room
### Climb corridor
### Lab room
#### Assets:
- [Erlenmeyer](Assets/LaboAsset/prefabs/): Erlenmeyer from [3D Laboratory Environment with Appratus](Assets/LaboAsset/) were modified with Blender to increase the size of the liquid flask to make the wooble effect more realistic: [link](https://assetstore.unity.com/packages/3d/environments/chemistry-lab-items-pack-220212)
- [Shader VFX](Assets/LaboAsset/Shader): The Liquid shader material was created following an online tutorial and was adapted to the project: [link](https://youtu.be/tI3USKIbnh0)
- [WaterAnimator and VFX graph](Assets/LaboAsset/prefabs/vfxgraph_AnimateWater.vfx/): The water stream animation was created following an online tutorial and was modified (remove of some 3D models and change of colours) to adapted to the project : [link](https://youtu.be/_H8gBKGKbnU)
### Server room
### Bots corridor
#### Assets:
- [Lockers](Assets/BotCorridorAsset/prefabs2/): The lockers come from the basic assets used for school objects. They were modified to add shelves inside to store objects. This was done using Blender: [link](https://styloo.itch.io/classroom-asset-pack)
### Prof's Desk
### General

## 2.3 Unmodified scripts and assets from external sources

### Tutos room
### Climb corridor
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
### Bots corridor
#### Assets:
- [BodyGuards and BotsAnimation](Assets/BotCorridorAsset/): [Mixamo](https://www.mixamo.com/#/?page=1&query=alex&type=Character)
- [ClassroomObjects](Assets/BotCorridorAsset/prefabs2/): [link](https://styloo.itch.io/classroom-asset-pack)
- [SocketsAndSwitches](Assets/BotCorridorAsset/): [link](https://assetstore.unity.com/packages/3d/props/interior/free-sockets-and-switches-233085)
- [Switch Light Sound Effect](Assets/BotCorridorAsset/Sounds/): [link](https://pixabay.com/sound-effects/projector-button-push-6258/)
### Prof's Desk
### General
#### Assets:
- [AI Voices](Assets/): [link](TODO)

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
### Simon:

## 3.2 Movement interaction

### Tyrolien

# 4. How to use the code
