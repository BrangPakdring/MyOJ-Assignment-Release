Given two integers \(a\) and \(b\), \(0 \leq a, b \leq 1000\), calculate \(a + b\).

## Input

Two integers \(a\) and \(b\) as described above.

## Output

A single integer indicates the answer.

## Examples
### Input
```
1 1
```
### Output
```
2
```

## Note

Examples for each language:

C
```c
#include <stdio.h>
int main()
{
	int a, b;
	scanf("%d%d", &a, &b);
	printf("%d\n", a + b);
}
```

C++
```cpp
#include <iostream>
using namespace std;
int main()
{
	int a, b;
	cin >> a >> b;
	cout << a + b << endl;
}
```

Java (The main class should always be `Main`)
```java
import java.io.*;
import java.util.*;
class Main
{
	public static void main(String[]args)
	{
		var in = new Scanner(System.in);
		System.out.println(in.nextInt() + in.nextInt());
	}
}
```

Python 2
```python
print sum(int(n) for n in raw_input().split())
```

Python 3
```python
print(sum(int(n) for n in input().split()))
```


C#
```cs
using System;
using System.Linq;
class Program
{
	static void Main()
		=> Console.WriteLine(Array.ConvertAll(Console.ReadLine().Split(" \t\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), int.Parse).Sum());
}
```
