#include<bits/stdc++.h>
using namespace std;
#define int long
int mat[1005][1005];
int dp[1005][1005];
main()
{
	    memset(dp, 0x3f, sizeof dp);
        int r, c;
        cin >> r >> c;
       
        for (int i = 1; i <= r; ++i)
            for (int j = 1; j <= c; ++j)
            {   cin >> mat[i][j];
            }
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
        cout << ans << endl;
}
