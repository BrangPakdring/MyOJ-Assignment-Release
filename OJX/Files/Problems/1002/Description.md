Given an array of \( n \) numbers \( a_ 1 , a_ 2 , ... , a_ n \), each of which represents the length of a segment. Your task is to determine whether all the segments form a polygon. 

## Input

The first line contains a positive integer \( n \), \(n \leq 100\), the length of the given array.

Next line contains exactly \( n \) positive integers 
\( a_ 1 , a_ 2 , ... , a_ n \),  the lengths of segments where \(a_ i \leq 10^9\) for \(1 \leq i \leq n\). 

## Output

If the given segments form a polygon, print `Yes`, otherwise print `No`.

## Examples
### Input
```
3
1 1 1
```
### Output
```
Yes
```
### Input
```
3
1 1 9
```
### Output
```
No
```