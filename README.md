# Historia Familia  
Historia Familia is an educational project made by two computer science students during the spring semester 2018.
After two days of designing and two weeks of programming a turn based strategy game for the first time, we finally reached the stage of an intractive, but not yet really playable prototype. Some major functionalities such as a turn manager or AI acting as your enemy are still missing. The lessons learned while working on this project are invaluable towards future endeavors.

## Story:
"Your recently deceased grandfather left behind a heirloom: Your family’s history documented in
an old looking book. Your curiosity sparks and you start to read. While reading, you slowly fall
asleep and suddenly, you are in the middle of that battle your grand, grand, grand father had
fought: The defense of the kingdom you live in even today against a mysterious aggressor.""

## Game Concept Document
Read the complete game idea: https://drive.google.com/file/d/1BZwx1qvV4ick40_amCCZrsIhYkeCyngm/view?usp=sharing


# Dev Log
See videos about the development process, how the game progresses over time and what it has turned into until we had to hand it in at the end of the semester. This game might never be finished but it is interesting to see how much effort it goes into making it. Everything was programmed by ourselves, tutorials and guides consulted about how to do X or Y in Unity and the web browsed for place holder assets.


### Generate units from Unity's inspector
In Unity it is possible to define units (of your army) from the inspector. This simplifies the creation of more unit types in the future.

<a href="http://www.youtube.com/watch?feature=player_embedded&v=xPgY7SaGWKs
" target="_blank"><img src="http://img.youtube.com/vi/xPgY7SaGWKs/0.jpg" 
alt="game development on the go" width="240" height="180" border="10" /></a>


### Boolean flag based unit state machine in action
A unit follows a state cycle that regulates what it can do in any moment.

<a href="http://www.youtube.com/watch?feature=player_embedded&v=Q6GGvhfCCB4
" target="_blank"><img src="http://img.youtube.com/vi/Q6GGvhfCCB4/0.jpg" 
alt="game development on the go" width="240" height="180" border="10" /></a>


# Known Issues (TBD)
- A selected unit can't attack without moving from the spot (Fix implemented. Requires testing).
- Missing avoidance: Units are moving through other units
- Ranged units should have a minimum range parameter meaning that they can't attack closeby themselves.
- Ranged unity can't counter attack at all (like in AW)
- Ranged unit's state cycle has to be ajusted so that they either have to attack or move, but when moved they can no longer attack.
- Improve map tile highlighting when moving, attacking or hovering with mouse, making them more distinct.
- Adding a marker to currently selected unit or alternatively a cursor where the mouse is currently hovering. When a unit is selected, it will stay there.
- Fire range algorithm isn't always deterministic in the sense of not always consistently having a range of max range. Sometimes it is lower at individual spots or when e.g. max range = 3, anomalies can also be observed.
