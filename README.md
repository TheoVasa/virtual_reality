# 1. Description of the Project
Game project using virtual reality

# References


## 1.1 Produced scripts and assets created by the team

### Tutos room
### Climb corridor
### Lab room
### Server room
### Bots corridor
### Prof's Desk

## 1.2 Adapted scripts and assets from external sources

### Tutos room
### Climb corridor
### Lab room
### Server room
### Bots corridor
### Prof's Desk

## 1.3 Unmodified scripts and assets from external sources

### Tutos room
### Climb corridor
### Lab room
### Server room
### Bots corridor
### Prof's Desk

# 2. Custom interactions

## 2.1 Non-movement interaction

### Potion mix:

#### Scene: [Lab Room](Assets/LaboAsset/)
#### Scripts:
- [PourDetector](Assets/LaboAsset/Scripts/PourDetector.cs/): If the current object (erlenmyer) is rotated by more than 90° in absolute value, the watering effect is activated, including activation of the water stream.
- [Fill](Assets/LaboAsset/Scripts/Fill.cs/):  If the collision box of the current object (erlenmeyer_final) comes into contact with the collision box of the water stream of an erlenmeyer of the correct color, the erlenmeyer_final begins to fill and has changed color from the original color of the erlenmeyer_final to the color of the target mixture. If the color of the erlenmeyer in contact is not the one expected, the liquid turns black and black smoke appears. If the water flow collision box of the erlenmeyer_final is in contact with the collision box of the sink, the liquid starts to flow until it is completely emptied. If the colour of the erlenmeyer_final is the right color (dark_green), a green smoke appears with a number 4.
- [Wobble](Assets/LaboAsset/Scripts/Wooble.cs/): Wobble effect of the liquid in the erlenmeyer flask. *This script was used as is from an online tutorial.*
- [GlassButtoncorrosion](Assets/LaboAsset/Scripts/GlassButtoncorrosion.cs/): If the box collider of the current object (box_glass) is in contact with the box collider of the water stream of the erlenmeyer_final of the correct colour (dark_green), then the box_glass is inactivated.
#### Assets:
- 

### Interaction 2
### Interaction 3

## 2.2 Movement interaction

### Tyrolien
