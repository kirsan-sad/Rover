# Rover

Implementation of the search algorithm A *  
As part of the terms of reference for the training as a C# developer in Syberry

The rover always moves from the top left point [0] [0] to the bottom right point [N - 1] [M - 1], where N and M are the length and width of the matrix.

Your rover has several limitations:

1. Traffic

From any point, the rover can move in any direction, including diagonally. The rover cannot return to the point at which it already was.

2. Charge

The rover runs on a charge. You know that it is very expensive for a rover to get up and down. He spends a unit of charge on the movement itself, and additional units on ascent and descent. 

3. Charge consumption

The charge is consumed according to the rule:  
For 1 step, the rover always spends 1 unit of charge. The rover uses a charge proportional to the difficulty of the ascent or descent to climb or descend. The difficulty of climbing or descending is the difference between the heights. When moving diagonally, spend every first movement two charge units and one unit every second movement. Altitude can be positive or negative (below sea level).

4. Obstacles  

On the way, there may be insurmountable obstacles. They are marked with the letter "X". The rover should fly around them.

You need to calculate the rover's path from the top left [0] [0] point to the bottom right [N - 1] [M - 1] point with the least amount of charge.
You do not know in advance the size of the photo that you will process. The array consists of strings. Where N and M are arbitrary numbers or "X". 

## Exceptions
If the starting or finishing point of the route begins with an obstacle "X", then throw the exception "Error in start or finish" and write the text of the error to the file path-plan.txt.  
If an obstacle prevents you from reaching the end, throws an exception.  
If the input array contains something other than numbers and the letter "X", throw an exception.  

## Plan

Make a route plan and projected expense in a txt file. Name the file path-plan.txt  
For matrix  
0 4  
1 3  

the plan would be like this:
path-plan.txt  
[0][0]->[1][0]->[1][1]  
steps: 2  
fuel: 5  

The rover goes from 0 to 1 to 3, takes two steps, spends 5 charges. If he went first at 4, then at 3, he would have taken the same number of steps, but would have spent 7 charges. Optimal path: 2 steps and 5 charges.

## Requirements  

Implement the calculateRoverPath (map) function as required.  
public static void CalculateRoverPath(string[,] map)  
Write the result to a txt file.  
Use ready-made files for the solution. Do not change the name of the file, the name of the methods and parameters.  
Write a .gitignor for yourself. Do not add .idea, src, venv, DS_STORE and similar files to the repository. The repository should contain only the file with the solution and README.md

Class name: Rover  
The calculateRoverPath method must be static  
Don't add packages  
C # version 8.0+  
Do not use Console.Write * and any other methods to output data to the console  

## Limitations  

Libraries cannot be used to implement the algorithm. You can only implement the algorithm on your own.  
You can use libraries to write to a file.
