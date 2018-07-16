# Goals for the first iteration of the prototype

This is the first prototype, which helps us discover how to build a system for a turn based strategy game. In this document, we define what we want to achieve in the first prototype.

## Spotting Code Improvement
Since it is our first try building this type of game, the code will most likely work. But considerations for performance and "good code quality" are left out for now. In a code review, we want to see what can be improved for the sake of maintainability, scalability and so on.

## What a player should be able to do
- Play against an AI opponent 1 on 1.
- Play against another player 1 on 1.
- Command units once per turn.
- Players/AI swap turns.
- Units can move on the board, engage in combat or wait while doing nothing else.
- Ranged and melee units.
- Instead of production facilities for new units (Advance Wars like), players/AI can capture enemy units and place them after e.g. 2 turns on the gameboard as a part of their team. Inspiration: Shogi.
- Topography vary each time a player starts a match.
- Mountains add a MaxAttackRange bonus to ranged units able to climb them.
- A player should be able to see a xx% prediction of how much damage a unit does to an targeted unit.
- Win condition: Either defeat the enemy commander / general or conquer their headquarter.