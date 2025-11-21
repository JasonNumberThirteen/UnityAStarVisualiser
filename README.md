# A* Visualiser
<p align = "center"><img src="./Assets/Project/Sprites/ApplicationLogo.png?raw=true" alt = "A* Visualiser"/></p>

> A visual editor application presenting working of A* algorithm made in "Unity" game engine.

## Table of Contents
* [General information](#general-information)
* [Used technologies](#used-technologies)
* [Features](#features)
* [Scenes](#scenes)
* [Usage](#usage)
* [Project status](#project-status)
* [Credits](#credits)

## General information
- This is a "Unity" project of a visual editor designed to present how A* algorithm works on a grid map.
- The project presents my programming skills and how I write code taking care of several optimisations.

## Used technologies
- [Unity](https://unity.com/ "Unity Real-Time Development Platform | 3D, 2D, VR &amp; AR Engine") game engine (version **6000.0.58f2**),
- [Visual Studio Code](https://code.visualstudio.com/ "Visual Studio Code - Code Editing. Redefined") IDE for writing code,
- [GIMP](https://www.gimp.org/ "GIMP - GNU Image Manipulation Program") for making graphics,
- [DOTween](https://dotween.demigiant.com/ "DOTween (HOTween v2)") plugin for using tweens,
- [Unity Native Gallery](https://github.com/yasirkula/UnityNativeGallery "Unity Native Gallery") plugin for taking screenshots on Android.

## Features
- **FLEXIBLE INPUT**
	- Moving the camera around the map
	- Zooming in/out the camera
	- Adjusting movement speed of the camera
	- Adjusting zooming in/out sensitivity of the camera
- **MAP GENERATION**
	- Generation of a map by the given dimensions in tiles (from **3x3** to **30x30**)
	- Adjusting dimensions (extending & shrinking) by the given size in the input fields
- **MAP EDITING**
	- Changing position of map tiles
	- Adjusting weight to each map tile
	- Changing tiles type (**passable**/**impassable**) depending on their weight (if it is below 0, then the tile is impassable)
- **ALGORITHM**
	- Finding a path between the start and destination tiles
	- Support of both **4-way** and **8-way** (diagonal) movement
	- Coloring on the map which define a path along with those that were set as visited
	- Drawing an animated path trail
- **HEURISTIC**
	- 3 types: **Manhattan**, **Euclidean** and **Chebyshev** distance metrics
	- Adjusting weight (multiplier) by the input field
- **SIMULATION MODE**
	- Showing how the algorithm works step by step
	- 2 types: **Timed** (step between interval in seconds) and **Stepwise** (step after clicking a button)
- **MISCELLANEOUS**
	- Taking screenshots of the map
	- Switching to fullscreen/windowed mode
	- Displaying of the map area along with additional offset around it in tiles (can be disabled)
	- Localization support: **Polish** and **English**
	- Multiplatform support: **Standalone**, **WebGL** and **Android**

## Scenes
### Language Selection
A starting scene with buttons for selecting desired language.

![A* Visualiser (Language Selection)](./Screenshots/LanguageSelection.png?raw=true)

### Visualiser
A scene wherein the visualiser is shown.

![A* Visualiser (Visualiser)](./Screenshots/Visualiser.png?raw=true)
![A* Visualiser (Visualiser 2)](./Screenshots/Visualiser2.png?raw=true)

## Usage

| Platform | Navigation | Hovering a tile | Dragging hovered tile | Changing tile weight | Zooming in/out the camera
| :---: | :---: | :---: | :---: | :---: | :---: |
| Standalone & WebGL | W / S / A / D or arrow keys | Hovering a tile with a mouse cursor | Left mouse button (**press & hold**) | Mouse scroll wheel (**if any tile is hovered**) | Mouse scroll wheel (**if none tile is hovered**)
| Android | Swipe (**outside of UI panels**) | Tapping a tile | Tapping already hovered tile | Adjusting by the UI slider shown on top of the screen | Pinch-to-zoom

## Project status
<p align = "center"><b>COMPLETED</b></p>
<p align = "center"><img src="https://upload.wikimedia.org/wikipedia/commons/f/f3/Gasr100percent.png"/></p>

---
<p align = "center"><b>The project is complete and ready to build</b>.</p>

## Credits
This project was made **entirely** by [Stanisław "Jason" Popowski](https://jasonxiii.pl "Jason. Cała informatyka w jednym miejscu! Oficjalna strona internetowa! Setki artykułów na różne tematy! Wszystko stworzone przez jedną osobę!").