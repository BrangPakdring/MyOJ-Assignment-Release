#include<bits/stdc++.h>
using namespace std;
char mat[1005][1005];
int main()
{
srand(time(0));
freopen("1006.in", "w+", stdout);
	cout << "1000 1000\n";
	for (int i = 0; i < 1000; ++i)
	for (int j = 0; j < 1000; ++j)
	mat[i][j] = (rand() % 16 ? '.' : '#');
	mat[0][0] = 'S';
	mat[999][999] = 'T';
		for (int i = 0; i < 1000; ++i)
		cout << mat[i] << endl;;
}
