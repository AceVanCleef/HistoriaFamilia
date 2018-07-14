# Knowledge and Troubleshooting Archive - A list of used tutorials and community posts for technical problem solving

## Trouble Shooting (How to do / fix X)

#3# How to install multiple versions of Unity
- [Installing multiple versions of Unity, to have different versions for different projects.](https://forum.unity.com/threads/installing-multiple-versions-of-unity-to-have-different-versions-for-different-projects.505556/)

### Object movement: LERP
- [Unity Tutorial 1 - Using "Vector3.Lerp" to move object from Point A to Point B](https://www.youtube.com/watch?v=k9FvVwd5pR4&t=175s&list=PLW5H12krTaY3XhCyksjj8WvtMIvYL5Tzq&index=7)

### Unit States
- [The State Design Pattern vs. State Machine](https://www.codeproject.com/Articles/509234/The-State-Design-Pattern-vs-State-Machine)
- [Don’t Re-invent Finite State Machines: How to Repurpose Unity’s Animator](https://medium.com/the-unity-developers-handbook/dont-re-invent-finite-state-machines-how-to-repurpose-unity-s-animator-7c6c421e5785)

### Colouring sprites
- [Unity Tutorial How To Change Gameobjects Sprite Color With C# Script Modifying Color Renderer Option](https://www.youtube.com/watch?v=J66UkLJHzCY&t=0s&list=PLW5H12krTaY3XhCyksjj8WvtMIvYL5Tzq&index=10)
- [changing sprite color makes sprite disappear](https://answers.unity.com/questions/1144563/changing-sprite-color-makes-sprite-disappear.html)  

### Random Number Generation in Unity
- [Random.Range (Documentation)](https://docs.unity3d.com/ScriptReference/Random.html)  

### Unity ClickEvent handling system
- [How to make gameplay ignore clicks on UI Button in Unity3D?](https://stackoverflow.com/questions/35529940/how-to-make-gameplay-ignore-clicks-on-ui-button-in-unity3d)
- [Unity UI - Blocking clicks](https://www.youtube.com/watch?v=EVZiv7DLU6E)
- "...make sure the Z index of the object is on top, even if you are making 2D games. The sorting layer is ignored and Z index take precedence instead." [Detecting Mouse Click on 2D Sprite](https://answers.unity.com/questions/574830/detecting-mouse-click-on-2d-sprite.html)

### Logical XOR operation
- [Logical XOR operator in C#: ^](http://becdetat.com/logical-xor-operator-in-c.html)

### Mouse Cursor on Tile Map
- [Creating Tools in Unity - Grid Cursor & Painting Tiles](https://www.youtube.com/watch?v=B2s7QNAvrcU)


## Lessons learned for prototype Version 2

### Sprite dimensions
Tile and unit sprites should be 100 x 100 pixels in order to close up the gaps in-between if the sprite is imported using the default settings. When clicking on a sprite, the inspector shows a property called "Pixels per unit". this defines how many pixels equal 1 Unit in world space. The default value is 100.
[What is the pixels to units property in Unity sprites used for?](https://gamedev.stackexchange.com/questions/83433/what-is-the-pixels-to-units-property-in-unity-sprites-used-for)