We have **regular string** defined as follows:
1. Empty string is a regular string.
2. If \(S\) is a regular string, then \((S)\), \([S]\), and \(\{S\}\) are also regular strings.
3. If both \(S\) and \(T\) are regular strings, then \(ST\) is also a regular string.

Given a string, determine whether it is a regular string. 

## Input
A string \(S\) that can only contains following characters:
 `(`, `)`, `[`, `]`, `{` and `}`.

## Output

Print `Yes` if a given string is a regular string, and `No` otherwise.
 
## Examples
### Input
```
([])
```
### Output
```
Yes
```

### Input
```
)(
```

### Output
```
No
```