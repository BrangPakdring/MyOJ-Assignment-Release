#include<bits/stdc++.h>
using namespace std;
#define int long
int mat[1005][1005];
int dp[1005][1005];
main()
{
    srand(time(0));
    for (int pid = 1000; pid < 1054; ++pid)
    {
	    memset(dp, 0x3f, sizeof dp);
    cout << pid << endl;
		string        pids = to_string(pid) + ".in";
		fstream fs;
		fs.open(pids, ios::in);
        int r, c;
        fs >> r >> c;
        cout << r  << ' ' << c << endl;
        for (int i = 1; i <= r; ++i)
            for (int j = 1; j <= c; ++j)
            {   fs >> mat[i][j];
            }
        pids = to_string(pid) + ".out";
        fs.close();
        fs.open(pids, ios::out);
        int ans = 0x7fffffffffffffffL;
        for (int i = 1; i <= r; ++i)dp[i][1] = mat[i][1];      
        for (int j = 2; j <= c; ++j)
            for (int i = 1; i <= r; ++i)
            {
                dp[i][j] = min({dp[i][j - 1], dp[i - 1][j - 1], dp[i + 1][j - 1]}) + mat[i][j];
  //              ans = min(ans, dp[i][j]);
            }
        for (int i = 1; i <= r; ++i)
            ans = min(ans, dp[i][c]);
        fs << ans;
        cout << ans << endl;
        fs.close();
    }
}
