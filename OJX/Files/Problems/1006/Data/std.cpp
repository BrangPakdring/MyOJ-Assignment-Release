//
// Created by brang on 12/14/18.
//

/**
 * Maze solution.
 * Input data format:
 * '.' means empty room, '#' means wall.
 * 'S' is starting point, 'T' is terminating point.
 * e.g.
 * 4 4
 * ####
 * #S.#
 * #.T#
 * ####
 */

#include <iostream>
#include <queue>
#include <map>
#include <cstring>
#include <set>
#include <stack>
#include <unordered_set>
#include <unordered_map>

using namespace std;

using pii = pair<int, int>;

pii operator+(const pii &a, const pii &b)
{
	return pii(a.first + b.first, a.second + b.second);
}

struct pairHash
{
	template <typename T1, typename T2>
	size_t operator()(const pair<T1, T2>&p)const
	{
		return hash<T1>{}(p.first) ^ hash<T2>{}(p.second);
	}

	size_t operator()(const pii&p)const
	{
		return p.first + ((size_t)p.second << 32);
	}
};

const size_t maxSize = 1005;
char maze[maxSize][maxSize];
size_t r, c;
pii s, t;
unordered_map<pii, pii, pairHash> last;
unordered_set<pii, pairHash> vis;
deque<pii> ans;
queue<pii> que;
stack<pii> stk;

constexpr const pii dir[] = {{0,  1},
                             {1,  0},
                             {-1, 0},
                             {0, -1}};

/**
 * Determine if a move to p is valid.
 * @param p The point moving to.
 * @return True if valid and false otherwise.
 */
bool isPointValid(const pii &p)
{
	auto &x = p.first, &y = p.second;
	return x >= 0 && y >= 0 && x < r && y < c && maze[x][y] != '#';
}

void noSolution()
{
	cout << "-1" << endl;
}

void initContainers()
{
	last.clear();
	vis.clear();
	ans.clear();
	while (!que.empty())que.pop();
	while (!stk.empty())stk.pop();
}

void printAnswer()
{
	cout << ans.size() - 1 << endl;
	cerr << ans.size() - 1 << endl;
}

/**
 * Use a queue to implement BFS in the maze to find a valid shortest path from s to t.
 * Time complexity: O(m * n)
 */
void mazeQueueBFS()
{
	initContainers();
	que.push(s);

	while (!que.empty())
	{
		auto p = que.front();
		que.pop();
		if (p == t)
		{
			do
			{
				ans.push_front(p);
				p = last[p];
			} while (p != s);
			ans.push_front(p);
			printAnswer();
			return;
		}
		for (auto &vec : dir)
		{
			pii newp = p + vec;
			if (!isPointValid(newp))continue;
			if (last.count(newp))continue;
			last[newp] = p;
			que.push(newp);
		}
	}

	noSolution();
}

bool dfs(const pii &p)
{
	if (p == t)
	{
		ans.push_front(p);
		return true;
	}
	vis.insert(p);
	for (auto &vec : dir)
	{
		pii newp = p + vec;
		if (!isPointValid(newp))continue;
		if (vis.count(newp))continue;
		if (dfs(newp))
		{
			ans.push_front(p);
			return true;
		}
	}
	return false;
}

/**
 * Use recursion DFS to find a valid path, not necessarily a shortest one.
 * Time complexity: O(m * n)
 */
void mazeRecursionDFS()
{
	initContainers();
	if (dfs(s))printAnswer();
	else noSolution();
}

/**
 * Use a stack to implement DFS in a maze to find a valid path, not necessarily a shortest one.
 * Time complexity: O(m * n)
 */
void mazeStackDFS()
{
	initContainers();
	stk.push(s);
	map<pii, size_t> curDir;
	while (!stk.empty())
	{
		auto &p = stk.top();
		if (p == t)
		{
			do
			{
				ans.push_front(p);
				p = last[p];
			} while (p != s);
			ans.push_front(p);
			printAnswer();
			return;
		}
		if (curDir[p] == sizeof(dir) / sizeof(dir[0]))
		{
			stk.pop();
			continue;
		}
		pii newp = p + dir[curDir[p]];
		if (isPointValid(p) && last.count(newp) == 0)
		{
			last[newp] = p;
			stk.push(newp);
		}
		++curDir[p];
	}
	noSolution();
}

/**
 * Read input as described above.
 */
void readMaze()
{
	cin >> r >> c;
	for (size_t i = 0; i < r; ++i)
		for (size_t j = 0; j < c; ++j)
		{
			cin >> maze[i][j];
			if (maze[i][j] == 'S')s = {i, j};
			if (maze[i][j] == 'T')t = {i, j};
		}
}

int main(int argc, const char *argv[])
{
freopen("1007.in", "r", stdin);
freopen("1007.out", "w+", stdout);
	readMaze();
	mazeQueueBFS();

//	readMaze();
//	mazeRecursionDFS();

//	readMaze();
//	mazeStackDFS();

	return 0;
}
