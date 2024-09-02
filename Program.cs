using KosarkaTurnir;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

List<TeamClass> teams = new()
{
    new ("Kanada", "CAN", 7, 0, "A"),
    new ("Australia", "AUS", 5, 0, "A"),
    new ("Grcka", "GRE", 14, 0, "B"),
    new ("Spanija", "ESP", 2, 0, "B"),
    new ("Nemačka", "GER", 3, 0, "C"),
    new ("Francuska", "FRA", 9, 0, "C"),
    new ("Brazil", "BRA", 12, 0, "D"),
    new ("Japan", "JPN", 26, 0, "D"),
    new ("Sjedinjene Države", "USA", 1, 0, "A"),
    new ("Srbija", "SRB", 4, 0, "B"),
    new ("Juzni Sudan", "SSD", 34, 0, "C"),
    new ("Puerto Riko", "PRI", 16, 0, "D")
};

foreach (var a in teams)
{
    a.DisplayTeamInfo();
}

static TeamClass PlayMatch(TeamClass teamA, TeamClass teamB, Random random)
{
    int teamAScore = 0;
    int teamBScore = 0;

    // Smanjenje šanse za osvajanje poena
    double nerfedProbabilityA = .5 / teamA.GetFIBARanking() % 2;
    double nerfedProbabilityB = .5 / teamB.GetFIBARanking() % 2;

    for (int i = 0; i < 60; i++)
    {
        if (random.NextDouble() < nerfedProbabilityA)
        {
            // Nasumično dodavanje 2 ili 3 poena
            teamAScore += random.Next(2, 4);
            Console.WriteLine("Tim " + teamA.GetTeamName() + "je dobio " + teamAScore + "\n");
        }

        if (random.NextDouble() < nerfedProbabilityB)
        {
            // Nasumično dodavanje 2 ili 3 poena
            teamBScore += random.Next(2, 4);
            Console.WriteLine("Tim " + teamB.GetTeamName() + "je dobio " + teamBScore + "\n");
        }
    }

    Console.WriteLine($"{teamA.GetTeamName()} Score: {teamAScore}");
    Console.WriteLine($"{teamB.GetTeamName()} Score: {teamBScore}");

    return teamAScore > teamBScore ? teamA : teamB;
}

Random random = new();
List<string> groupNames = new List<string> { "A", "B", "C", "D" };

AssignAndDisplayGroups(teams, groupNames, random);

var groupWinners = GroupWinners(teams, groupNames, random);


var semiFinalists = SemiFinals(groupWinners, random);

var finalWinner = FinalRound(semiFinalists, random);

static void AssignAndDisplayGroups(List<TeamClass> teams, List<string> groupNames, Random random)
{
    Dictionary<string, List<TeamClass>> groups = groupNames.ToDictionary(group => group, group => new List<TeamClass>());

    foreach (var team in teams)
    {
        string assignedGroup;
        do
        {
            assignedGroup = groupNames[random.Next(groupNames.Count)];
        } while (groups[assignedGroup].Count >= 3);

        groups[assignedGroup].Add(team);
        team.SetAssignedGroup(assignedGroup);
        Console.WriteLine($"{team.GetTeamName()}, Assigned Group: {assignedGroup}");
    }

    Console.WriteLine("\n--- Group Assignments ---");
    foreach (var group in groups)
    {
        Console.WriteLine($"\nGroup {group.Key}:");
        foreach (var team in group.Value)
        {
            Console.WriteLine(team.GetTeamName());
        }
    }
}

static List<TeamClass> GroupWinners(List<TeamClass> teams, List<string> groupNames, Random random)
{
    List<TeamClass> winners = new List<TeamClass>();  // Correct initialization


    foreach (var groupName in groupNames)
    {
        Console.WriteLine($"\n ---Group {groupName}---\n");

        var groupTeams = teams.Where(t => t.GetAssignedGroup() == groupName).ToList();

        Console.WriteLine($"\nGroup {groupName}:");
        foreach (var team in groupTeams)
        {
            Console.WriteLine(team.GetTeamName());
        }

        if (groupTeams.Count > 0)
        {
            // Each team plays against every other team in the group
            var groupMatchResults = new Dictionary<TeamClass, int>();
            foreach (var team in groupTeams)
            {
                groupMatchResults[team] = 0;  // Initialize the score for each team
            }

            for (int i = 0; i < groupTeams.Count; i++)
            {
                for (int j = i + 1; j < groupTeams.Count; j++)
                {
                    var teamA = groupTeams[i];
                    var teamB = groupTeams[j];

                    var matchWinner = PlayMatch(teamA, teamB, random);
                    groupMatchResults[matchWinner]++;  // Increase the win count for the match winner
                }
            }

            // Determine the group winner
            var groupWinner = groupMatchResults.OrderByDescending(gr => gr.Value).First().Key;
            Console.WriteLine($"Pobednik iz grupe {groupName} je: {groupWinner.GetTeamName()}");
            winners.Add(groupWinner);
        }
    }

    return winners;
}

static List<TeamClass> SemiFinals(List<TeamClass> groupWinners, Random random)
{
    Console.WriteLine("\n--- Polu-Finale ---");

    var groupWinnersByGroup = groupWinners
        .GroupBy(t => t.GetAssignedGroup())
        .ToDictionary(g => g.Key, g => g.Last());

    var groupPairs = new List<(string, string)>
    {
        ("A", "B"),
        ("C", "D"),
    };

    var semiFinalsWinners = new List<TeamClass>();

    for (int i = 0; i < groupPairs.Count; i++)
    {
        var pair = groupPairs[i];
        var teamA = groupWinnersByGroup[pair.Item1];
        var teamB = groupWinnersByGroup[pair.Item2];

        var winner = PlayMatch(teamA, teamB, random);

        Console.WriteLine($"\n Polu-finale {i + 1}:");
        Console.WriteLine($"Tim {teamA.GetTeamName()}, FIBA Ranking: {teamA.GetFIBARanking()}");
        Console.WriteLine($"Tim {teamB.GetTeamName()}, FIBA Ranking: {teamB.GetFIBARanking()}");
        Console.WriteLine($"Pobednik polu-finala је {i + 1}: {winner.GetTeamName()}\n");

        semiFinalsWinners.Add(winner);
    }

    return semiFinalsWinners;
}

static List<TeamClass> FinalRound(List<TeamClass> semiFinalWinners, Random random)
{
    var finalWinner = new List<TeamClass>();

    Console.WriteLine("\n--- Finale ---");

    for (int i = 0; i < semiFinalWinners.Count; i += 3)
    {
        // Make sure there's a pair of teams to match
        if (i + 1 < semiFinalWinners.Count)
        {
            var teamA = semiFinalWinners[i];
            var teamB = semiFinalWinners[i + 1];

            var winner = PlayMatch(teamA, teamB, random);

            Console.WriteLine($"\nFinale:");
            Console.WriteLine($"Tim A: {teamA.GetTeamName()}, FIBA Ranking: {teamA.GetFIBARanking()}");
            Console.WriteLine($"Tim B: {teamB.GetTeamName()}, FIBA Ranking: {teamB.GetFIBARanking()}");
            Console.WriteLine($"Pobednik finala je: {winner.GetTeamName()}");

            finalWinner.Add(winner);
        }

    }

    return finalWinner;
}
