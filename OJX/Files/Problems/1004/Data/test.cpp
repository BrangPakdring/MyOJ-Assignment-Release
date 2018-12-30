#include<bits/stdc++.h>
using namespace std;
#define int long
int mat[5005][5005];
int dp[5005][5005];
main()
{
    srand(time(0));
    for (int pid = 1002; pid < 1050; ++pid)
    {
        string pids = to_string(pid) + ".in";
        fstream fs;
        fs.open(pids, ios::out|ios::trunc);
        int r, c;
        r = (int)(rand() * (1e9 + 7)) % 50 + 1;
        c = (int)(rand() * (1e9 + 7)) % 50 + 1;
        if (pid > 1040)r += 500, c += 500;
        if (pid > 1046)r = 1000, c = 1000;
        fs << r << ' ' << c << endl;
        //        memset(mat, 0x10, sizeof mat);
//        memset(dp, 0x10, sizeof dp);
        for (int i = 1; i <= r; ++i)
            for (int j = 1; j <= c; ++j)
            {   fs << (mat[i][j] = (int)rand() % (pid < 1040 ? 10001 : 101)) << " \n"[j == c ];
                dp[i][j] = 0x3f3f3f3f3f3f3f3fL;
                
//                dp[i][j] = 0;
            }
        pids = to_string(pid) + ".out";
        fs.close();
        fs.open(pids, ios::out|ios::trunc);
        for (int i = 1; i <= r; ++i)dp[i][1] = mat[i][1];      
        for (int j = 2; j <= c; ++j)
            for (int i = 1; i <= r; ++i)
            {
                dp[i][j] = min({dp[i][j - 1], dp[i - 1][j - 1], dp[i + 1][j - 1]}) + mat[i][j];
            }
        int ans = 0x7fffffffffffffffL;
        for (int i = 1; i <= r; ++i)
            ans = min(ans, dp[i][c]);
        fs << ans;
        fs.close();
    }
}
