public static class NameScreeningService
{
    public static (bool IsFlagged, string? MatchedName, int Score) Screen(string beneficiaryName)
    {
        string bestMatch = "";
        int bestScore = 0;

        foreach (var watchlistName in SanctionsWatchlist.Names)
        {
            int score = SimilarityScore(beneficiaryName.ToLower(), watchlistName.ToLower());
            if (score > bestScore)
            {
                bestScore = score;
                bestMatch = watchlistName;
            }
        }

        bool isFlagged = bestScore >= 70; // threshold: 70% ya usse zyada similarity pe flag

        return (isFlagged, isFlagged ? bestMatch : null, bestScore);
    }

    // Simple similarity % based on Levenshtein distance
    private static int SimilarityScore(string a, string b)
    {
        int distance = LevenshteinDistance(a, b);
        int maxLen = Math.Max(a.Length, b.Length);
        if (maxLen == 0) return 100;

        double similarity = 1.0 - (double)distance / maxLen;
        return (int)(similarity * 100);
    }

    private static int LevenshteinDistance(string s, string t)
    {
        int[,] d = new int[s.Length + 1, t.Length + 1];

        for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
        for (int j = 0; j <= t.Length; j++) d[0, j] = j;

        for (int i = 1; i <= s.Length; i++)
        {
            for (int j = 1; j <= t.Length; j++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[s.Length, t.Length];
    }
}