Given a maze together with an entrance \(S\) and an exit \(T\), find a shortest path from \(S\) to \(T\). In this maze, you are allow to go to one of four adjacent cells on each move. Moreover you are not allow to go through those broken cells represented as `#`. For example, if you are currently at cell \((i, j)\), you can move to \((i, j - 1)\), \((i, j + 1)\), \((i - 1, j)\) or \((i + 1, j)\) if the move is valid.

## Input
The first line gives two integers \(m\) and \(n\), \(2 \leq m, n \leq 1000\), the number of rows and the columns of the maze, respective.

Next \(m\) lines gives the maze consisting of characters `.`, `#`, `S` and `T`, where `.` means a valid cell, `#` means a broken cell, `S` means the entrance and `T` the exit.

## Output
Print a single integer, the shortest path from the entrance to the exit. If no such path exists, print `-1`.
 
## Examples
### Input
```
3 3
S..
...
..T
```
### Output
```
4
```

### Input
```
5 5
....S
.....
...##
.....
....T
```

### Output
```
8
```

### Input
```
1 3
S#T
```

### Output
```
-1
```