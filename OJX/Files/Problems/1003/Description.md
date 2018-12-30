As we know, 55-kai loves playing games and always wins. One day, one of his friends called Squirrel wants to play a special game with 55-kai. The rule is rather simple: Initially they pick up \(n\) integers: \(1, 2, ..., n\). They start the game and take turns to pick up one number and some related others to be removed. If a player picks up a number \(x\) then all remained divisors of \(x\) will be removed as well. The player who cannot make a move loses. 55-kai thinks that the game rule is so simpile that he always lets Squirrel to make the first move. Your task is to help determine whether 55-kai has the chance to win the game if both of them play optimal. 

## Input

The first line contains only one positive integer \(n\), \(1 \leq n \leq 10^9\), meaning there are \(n\) numbers \(1, 2, ..., n\) initially. 

## Output

With both players playing optimal, if 55-kai can win the game, print `lbwnb`, otherwise print `lbwgb`.

## Examples
### Input
```
2
```
### Output
```
lbwgb
```

## Note
In the example they have initially two integers: \(1, 2\). Squirrel plays first and he picks up number \(2\) and removes its divisors \(1\) as well. Then no numbers left for 55-kai to remove any more thus 55-kai loses.
