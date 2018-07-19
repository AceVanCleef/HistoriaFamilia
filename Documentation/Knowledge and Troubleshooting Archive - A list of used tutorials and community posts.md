# Knowledge and Troubleshooting Archive - A list of used tutorials and community posts for technical problem solving

## Trouble Shooting (How to do / fix X) in Unity

### How to install multiple versions of Unity
- [Installing multiple versions of Unity, to have different versions for different projects.](https://forum.unity.com/threads/installing-multiple-versions-of-unity-to-have-different-versions-for-different-projects.505556/)

### Object movement: LERP
- [Unity Tutorial 1 - Using "Vector3.Lerp" to move object from Point A to Point B](https://www.youtube.com/watch?v=k9FvVwd5pR4&t=175s&list=PLW5H12krTaY3XhCyksjj8WvtMIvYL5Tzq&index=7)

### Unit Movement and Attack Range
- BFS for getting all tiles within walking or attack range: [How do I highlight all available paths with Dijkstra's algorithm on a tile based map?](https://answers.unity.com/questions/1063687/how-do-i-highlight-all-available-paths-with-dijkst.html?sort=oldest)

### Unit States
- [The State Design Pattern vs. State Machine](https://www.codeproject.com/Articles/509234/The-State-Design-Pattern-vs-State-Machine)
- [Don’t Re-invent Finite State Machines: How to Repurpose Unity’s Animator](https://medium.com/the-unity-developers-handbook/dont-re-invent-finite-state-machines-how-to-repurpose-unity-s-animator-7c6c421e5785)

## AI Programming
- [Programming NPC Behaviour with Finite State Machines in Unity Part 1](https://www.youtube.com/watch?v=NEvdyefORBo)
- [Programming NPC Behaviour with Finite State Machines in Unity Part 2](https://www.youtube.com/watch?v=tdYsq96kCYI)
- [Programming NPC Behaviour with Finite State Machines in Unity Part 3](https://www.youtube.com/watch?v=5qDadIloxvU)
- [Combat AI for Action-Adventure Games Tutorial (Unity/C#) GOAP](https://www.youtube.com/watch?v=n6vn7d5R_2c)

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

### Make two Sliders in Inspector dependent on each other
- [Make 2 sliders dependent on each other](https://answers.unity.com/questions/1112804/make-2-sliders-dependent-on-each-other.html)

### Screen resolution and Aspect Ratio
- [Managing Screen Resolution and Aspect Ratio in Unity 3D](http://www.aclockworkberry.com/managing-screen-resolution-and-aspect-ratio-in-unity-3d/)

### Get width and height of a canvas
- [Unity 4.6 UI Canvas width & height](https://answers.unity.com/questions/889220/unity-46-ui-canvas-width-height.html)

### Animate UI
- Formula: currentvalue += (finalvalue - currentvalue) * slidespeed: [Animation in 2D Unity Games: In-Depth Starter Guide](https://www.gamasutra.com/blogs/AlexRose/20130905/199662/Animation_in_2D_Unity_Games_InDepth_Starter_Guide.php)

### Coroutines
```C#
void Update () {
    ...
    if (_wait) {
		StartCoroutine( SlideOutAfter( 1.25 ) );
	}
    ...
}

private IEnumerator  SlideOutAfter(float seconds) {
		_wait = false;	//prevent subsequent call of this coroutine.
		//pause Coroutine for x seconds.
        yield return new WaitForSeconds(seconds);
		_slideOut = true;
}
```
- [MonoBehaviour.StartCoroutine](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html)

#### IEnumerator - C#'s Iterator
``` C#
private IEnumerator ...():  
```
The return type is basically an iterator (Java). We can say: Iterator (Java) == IEnumerator (C#)  
  
* ienumerator.Current returns the current item.  
* ienumerator.MoveNext() moves to the next item and returns true if it moved forward sucessfully.  
* There is also a ienumerator.Reset() method, which sets the iterator to its original position, right infront of the first element in the collection.  
- [IEnumerator Interface](https://msdn.microsoft.com/en-us/library/system.collections.ienumerator(v=vs.110).aspx)


## Git

### Never type username and password again when pushing by https
- [Setup command - Trying to understand wincred with git for windows - confused](https://stackoverflow.com/questions/38333752/trying-to-understand-wincred-with-git-for-windows-confused)
- [Git documentation: 7.14 Git Tools - Credential Storage](https://git-scm.com/book/en/v2/Git-Tools-Credential-Storage)


## Lessons learned for prototype Version 2

### Sprite dimensions
Tile and unit sprites should be 100 x 100 pixels in order to close up the gaps in-between if the sprite is imported using the default settings. When clicking on a sprite, the inspector shows a property called "Pixels per unit". this defines how many pixels equal 1 Unit in world space. The default value is 100.
[What is the pixels to units property in Unity sprites used for?](https://gamedev.stackexchange.com/questions/83433/what-is-the-pixels-to-units-property-in-unity-sprites-used-for)