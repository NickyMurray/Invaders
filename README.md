# Invaders
### by Nicholas Murray


## Introduction 

This assignment was created to demonstrate the use and importance of AI elements and Finite State Machines in game development to create fun and responsive gameplay. 
In the game you control a futuristic tank that you must use to clear all the the emergency droppods from a friendly space ship of the enemy robots. You can use the laser canons on your tank to fight the enemy and when you score enough points (200) you can call in a missile barrage to destroy all enemies in your vicinity.
The enemy are a race of alien robots that destroy anything they encounter. They have multiple different weapons to attack you with like lasers and bullets that track your movements so you can not escape them.

## AI Elements

Below is a list of the AI elements hat I have implemented into my game
- Line of Sight chasing / Evasion
- Player Interception / Evasion
- Pattern movement / waypoint movement

All of the enemies in my game use interception to chase the player. I also used the same method for evasion as I found it is effective at evading the player. The code I used to achieve this can be seen in the image below.

![Enemy Chase](Assignment/Assignment%20Images/EnemyChaseCode.PNG)

When the enemies are not chasing the player they move between a set of waypoints. This is the enemies default state when the game starts and they revert to it when the player moves out of there line of sight. The code I used to achieve this can be seen in the image below.

![Enemy Waypoints](Assignment/Assignment%20Images/EnemyWaypointCode.PNG)

The players bullets and some of the enemy bullets (red ones) use basic Vector math to move to a static point on the screen. The code used to achieve this can be seen in the Move method in the image below.

(**NB:** the variable targetPos in the Move method is set when the bullet is instantiated and does not change unlike the target.position in the Track method which is the current targets position)

![EnemyBullets](Assignment/Assignment%20Images/EnemyBulletMovement.PNG)

There is some enemy bullets (purple ones) that use line of sight chasing to target the player and chase you until they collide with you or another in the level. The code used to achieve this can be seen in the Track method in the image above.
The players missiles also use Line of Sight to reach their targets. 

![Missile Movement](Assignment/Assignment%20Images/MissileMovement.PNG)

## Finite State Machines (FSM)

I have used FSMs for both the enmies and the player below you can see diagrams showing how they work

### Enemy States
![Enemy States](Assignment/Assignment%20Images/EnemyStates.png)

![Enemy States Code](Assignment/Assignment%20Images/EnemyStatesCode.png)

### Player States
![Player States](Assignment/Assignment%20Images/PlayerStates.png)

![Player States Code](Assignment/Assignment%20Images/PlayerStatesCode.PNG)

**NB:** The player FSM shows that the player could transition to dead from the paused state however this cannot happen because all enemies and bullets stop when the game is paused, however the transition could still theoretically happen.
