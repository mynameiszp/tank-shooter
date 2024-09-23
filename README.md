# Tank Battle Game

## Overview
This is a Unity-based tank battle game where the player controls a tank, and AI tanks serve as enemies. The game takes place in a rectangular arena where the player must destroy AI-controlled tanks while avoiding collisions with them. The AI tanks move randomly and respawn after being destroyed, creating a dynamic and challenging gameplay experience.

## Game Objective
The player's goal is to eliminate all AI tanks using projectile shots while dodging both enemy tanks and their movements. When all AI tanks are destroyed, they respawn again on the game field. The player can move their tank within the defined boundaries of the arena, but any attempt to move beyond the field results in the player hitting an invisible "wall." When the player's tank is destroyed by a collision with an AI tank, it respawns in one of the corners.

## Features

### Player Tank Control:
- Use `W` and `S` keys to move forward and backward.
- Use `A` and `D` keys to rotate the tank counterclockwise or clockwise.
- The tank fires projectiles by clicking the mouse, with shots moving in the direction the tank was facing at the moment of firing.

### AI Tank Behavior:
- AI tanks move at random angles on the plane.
- AI tanks change their direction at set intervals, and collisions with walls or the player's tank cause them to adjust their direction.
- AI tanks respawn in positions near borders after being destroyed.

### Projectile System:
- Player projectiles move in a straight line and are destroyed when they hit a wall or an AI tank.
- When an AI tank is hit, both the projectile and the tank are destroyed.

## Design Principles

### Modular AI
The game architecture allows for easy modification of the AI module, enabling different behaviors for AI tanks.

### Customizable Tank Control
The control scheme can be swapped to alternative methods, such as individual track control for the player's tank.

## Advanced Features

### Object Pool Pattern
The project uses the object pool design pattern for efficient management of frequently instantiated and destroyed objects like tanks and projectiles.

### State Machine
AI tanks are implemented using a state machine to manage their behavior and transitions between different states.

### Dependency Injection with Zenject
The game utilizes the Zenject framework for dependency injection, ensuring better separation of concerns and testability of components.

## Save and Load Feature
The game supports saving and loading the current state in either `.json` or `.xml` formats. This allows for persistent gameplay across sessions and gives players the ability to save their progress and load it when they start the game.

## Spawn Conflict Solution
There is a version of the game that introduces a spawn conflict solution. In this version, when a spawn conflict (e.g., two tanks spawning in the same location) occurs, one of the tanks is relocated. This feature requires further testing and development but is an early attempt at solving potential spawn-related issues. The solution can be found in the `feature/enemySpawnBugFix` branch.

## Gameplay Demo
![Tank Battle Gameplay](Media/tank-battle-demo.gif)
