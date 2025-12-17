# Hyperspace Cheese Battle

Hyperspace Cheese Battle is a turn-based multiplayer game for 2–4 players set on an 8×8 grid.

Each player controls a rocket ship that starts off the board and enters at the bottom-left corner. On each turn, players roll a dice to move their rocket in the direction indicated by the arrow on their current square. If a move would take a rocket off the board, the rocket does not move. Some squares contain **Cheese Power**. When a player lands on one of these squares, they may either roll the dice again for an extra move or fire a “Cheese Deathray” at another player. A rocket hit by the deathray is sent back to an unoccupied square on the bottom row of the board. Rockets cannot occupy the same square. If a rocket lands on an occupied square, it continues moving in the direction of the arrow until a free square is found.

The first player to reach the top-right square of the board wins the game.

## Features
- Object-oriented C# design using .NET
- Interfaces for game and player behavior
- CSV-driven board configuration
- Rule-based movement, collision handling, and win conditions
- Dice abstraction using a Dice Bag
- Console-based user interaction

## Technologies
C#, .NET, Visual Studio Code, Object-Oriented Programming, File I/O (CSV)
