As of today, people all around the world have put forward no less than 1919810114514 different theories about the what mysterious being called Yjsnpi. Though Yjsnpi once announced that it is 24 years old as a student studying hard its being became a riddle after its last show. Missing Yjsnpi's delicious apples and black tea so much (really) Toono wants Mur to help him find out the real being of Yjsnpi. Mur found out many queries from many of those theories and wrote down a list, each telling a truth \(A\) is \(B\). However, not all of queries are telling a truth. You are to help determine whether some given queries are true.

## Input
The first line contains two numbers \(n\) and \(q\), the number of theories and the number of queries. 

Next \(n\) lines follow, each giving the content of a theory in format: `A is B`. Here \(A\) and \(B\) are alphanumeric strings and \(1 \leq |A|, |B| \leq 20\).

Then \(q\) lines follow, each giving a query in format: `A is B`.
## Output

For a given query \(A\) is \(B\), it is true iff:
- \(A = B\)
- \(B\) is \(A\)
- \(A\) is \(C\) and \(B\) is \(C\)

For each query `A is B`:
- print `Soudayo!` if it is false.
- print `Q.E.D.` if it is true, and \(A \neq B\).
- print `I proved myself!` if it is true, and \(A = B\).
 
## Examples
### Input
```
4 4
Yjsnpi is dust
Newton is dust
Yjsnpi is everything
A is B
Yjsnpi is Newton
Yjsnpi is Yjsnpi
dust is Yjsnpi
A is C
```
### Output
```
Q.E.D.
I proved myself!
Q.E.D.
Soudayo!
```