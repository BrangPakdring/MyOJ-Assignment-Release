Cirno is unfortunately stuck in a maze which is full of magic. Now the only thing she has to do is to escape from it as soon as possible. 

The maze is an \(m \times n\) matrix and each cell in it has different construction that it takes her different time to walk through different cells. 
Initially Cirno is in the left side of the maze and she can choose one of the \(m\) cells in the left side to start. Going through one cell, say \((i, j)\), she can choose one of the three adjacent cells \((i - 1, j + 1)\) (if exists), \((i, j + 1)\) and \((i + 1, j + 1)\) (if exists) to move on. Cirno must go through the maze and reach the right side of it to escape.
Though Cirno is good at math but she can only count up to ⑨. Therefore, you have to help her find out a path that costs a minimal time to escape as she is almost hungry and dying. 

## Input 
The first line contains two positive integers \(m\) and \(n\), \(1 \leq m, n \leq 1000\), the number of rows and columns of the maze as a matrix.

Then \(m\) lines follow, each containing exactly \(n\) space-separated non-negative integers. Each integer \(x \leq 10000\) indicates the time it costs Cirno to go through respective.  
  
## Output
Print a single integer, the minimal time it takes Cirno to escape from the maze. 

## Examples
### Input
```
3 4
1 2 3 4
1 1 2 1
1 1 1 3
```
### Output
```
4
```
### Input
```
3 3
8 1 0
1 1 4
5 1 4
```
### Output
```
2
```

## Note
In the first example, one possible path is \((1, 1) \to (2, 2) \to (3, 3) \to (2, 4)\), with a total time of 4. Another path \((3, 1) \to (3, 2) \to (3, 3) \to (2, 4)\) is also correct. 