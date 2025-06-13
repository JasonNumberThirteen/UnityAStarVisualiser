# A* Visualiser

> A visual editor application presenting working of A* algorithm made in "Unity" game engine.

## Table of Contents
* [General information](#general-information)
* [Used technologies](#used-technologies)
* [Features](#features)
* [Screenshots](#screenshots)
* [Usage](#usage)
* [Project status](#project-status)
* [Credits](#credits)

## General information
- This is a "Unity" project of a visual editor designed to present how A* algorithm works on a grid map.
- The project presents my programming skills and how I write code taking care of several optimisations.

## Used technologies
- [Unity](https://unity.com/ "Unity Real-Time Development Platform | 3D, 2D, VR &amp; AR Engine") game engine (version **6000.0.47f1**),
- [Visual Studio Code](https://code.visualstudio.com/ "Visual Studio Code - Code Editing. Redefined") IDE for writing code,
- [GIMP](https://www.gimp.org/ "GIMP - GNU Image Manipulation Program") & [Paint.NET](https://www.getpaint.net/ "Paint.NET - Free Software for Digital Photo Editing") for making graphics,
- [DOTween](https://dotween.demigiant.com/ "DOTween (HOTween v2)") plugin for using tweens.

## Features
- **MAP GENERATION**
	- Generation of a map by the given dimensions in tiles (from **3x3** to **50x50**)
	- Adjusting dimensions (extending & shrinking) by the given size in the input field
- **MAP EDITING**
	- Changing position of map tiles by dragging with a mouse
	- Adjusting weight to each map tile by using a mouse scroll wheel
	- Changing tiles type (**passable**/**impassable**) depending on their weight (if it is below 0, then tile is impassable)
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
	- Zooming in/out by using a mouse scroll wheel

## Screenshots
![A* Visualiser](./Screenshots/AStarVisualiser.png?raw=true)
![A* Visualiser](./Screenshots/AStarVisualiser2.png?raw=true)

## Usage
- W / S / A / D or arrow keys - **Navigation**,
- Left mouse button (press & hold) - **Dragging hovered tile**,
- Mouse scroll wheel - **Zooming in and out** (if none tile is hovered) / **Changing weight of a tile** (if any tile is hovered).

## Project status
<p align = "center"><b>COMPLETED</b></p>
<p align = "center"><img src="https://upload.wikimedia.org/wikipedia/commons/f/f3/Gasr100percent.png"/></p>

---
<p align = "center"><b>The project is complete and ready to build</b>.</p>

## Credits
This project was made **entirely** by [Stanisław "Jason" Popowski](https://jasonxiii.pl "Jason. Cała informatyka w jednym miejscu! Oficjalna strona internetowa! Setki artykułów na różne tematy! Wszystko stworzone przez jedną osobę!").