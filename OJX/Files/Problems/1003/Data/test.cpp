#include<bits/stdc++.h>
using namespace std;

main()
{
    int f = 1000;
    for (int i = 1; i < 100; ++i)
    {
        string ff = to_string(f)+".in";
        freopen(ff.c_str(), "w+", stdout);
       if(i < 50) cout << i;
       else        if (i < 75)cout << i * i;
       else if (i < 100)cout << i * i * i;

        string fff = to_string(f)+".out";
        freopen(fff.c_str(), "w+", stdout);
        cout << "lbwgb";
        ++f;
    }
}
