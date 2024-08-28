using KosarkaTurnir;
using System.Text.RegularExpressions;

List<TeamClass> teams = new List<TeamClass>
{
    new TeamClass("Kanada", "CAN", 7, 0, "A"),
    new TeamClass("Australia", "AUS", 5, 0, "A"),
    new TeamClass("Grcka", "GRE", 14, 0, "B"),
    new TeamClass("Spanija", "ESP", 2, 0, "B"),
    new TeamClass("Nemačka", "GER", 3, 0, "C"),
    new TeamClass("Francuska", "FRA", 9, 0, "C"),
    new TeamClass("Brazil", "BRA", 12, 0, "D"),
    new TeamClass("Japan", "JPN", 26, 0, "D"),
    new TeamClass("Sjedinjene Države", "USA", 1, 0, "A"),
    new TeamClass("Srbija", "SRB", 4, 0, "B"),
    new TeamClass("Juzni Sudan", "SSD", 34, 0, "C"),
    new TeamClass("Puerto Riko", "PRI", 16, 0, "D")
};


foreach (var a in teams){
    a.DisplayTeamInfo();
}

Random random = new Random();
List<string> groupNames = new List<string> { "A", "B", "C", "D" };

AssignAndDisplayGroups(teams, groupNames, random);

DisplayGroupsWithWinners(teams, groupNames);

Console.WriteLine("\n--- Quarter-Finals ---");
QuarterFinals(teams);

Console.WriteLine("\n--- Semi-Finals ---");
List<TeamClass> semiFinalWinners = SemiFinals(teams);

Console.WriteLine("\n--- Final ---");
TeamClass finalWinner = FinalRound(semiFinalWinners);

Console.WriteLine($"\nThe overall winner is: {finalWinner.GetTeamName()}");

static void AssignAndDisplayGroups(List<TeamClass> teams, List<string> groupNames, Random random)
{
    foreach (var a in teams)
    {
        // Randomly assign a group
        string assignedGroup = groupNames[random.Next(groupNames.Count)];
        Console.WriteLine($"{a.GetTeamName()}, Assigned Group: {assignedGroup}");
    }
}

static List<TeamClass> DisplayGroupsWithWinners(List<TeamClass> teams, List<string> groupNames)
{
    List<TeamClass> winners = new List<TeamClass>();

    foreach (var groupName in groupNames)
    {
        var groupTeams = teams.Where(t => t.GetAssignedGroup() == groupName).ToList();

        Console.WriteLine($"\nGroup {groupName}:");
        foreach (var team in groupTeams)
        {
            Console.WriteLine(team.GetTeamName());
        }

        if (groupTeams.Count > 0)
        {
            var winner = groupTeams.OrderByDescending(t => t.GetFIBARanking()).First();
            Console.WriteLine($"Winner of Group {groupName}: {winner.GetTeamName()}");
            winners.Add(winner);
        }
    }

    return winners;
}
static List<TeamClass> QuarterFinals(List<TeamClass> groupWinners)
{
    // Group winners by their original groups
    var groupWinnersByGroup = groupWinners
        .GroupBy(t => t.GetAssignedGroup())
        .ToDictionary(g => g.Key, g => g.Last());

    // Define pairs of groups to match in the quarter-finals
    var groupPairs = new List<(string, string)>
    {
        ("A", "B"), // Group A winner vs Group B winner
        ("C", "D"), // Group C winner vs Group D winner
        // Add more pairs if there are more groups
    };

    var quarterFinalWinners = new List<TeamClass>();

    // Determine winners for each quarter-final
    for (int i = 0; i < groupPairs.Count; i++)
    {
        var pair = groupPairs[i];
        var teamA = groupWinnersByGroup[pair.Item1];
        var teamB = groupWinnersByGroup[pair.Item2];

        var winner = teamA.GetFIBARanking() > teamB.GetFIBARanking() ? teamA : teamB;

        Console.WriteLine($"\nQuarter-Final {i + 1}:");
        Console.WriteLine($"Team from Group {pair.Item1}: {teamA.GetTeamName()}, FIBA Ranking: {teamA.GetFIBARanking()}");
        Console.WriteLine($"Team from Group {pair.Item2}: {teamB.GetTeamName()}, FIBA Ranking: {teamB.GetFIBARanking()}");
        Console.WriteLine($"Winner of Quarter-Final {i + 1}: {winner.GetTeamName()}");

        quarterFinalWinners.Add(winner);
    }

    return quarterFinalWinners;
}

static List<TeamClass> SemiFinals(List<TeamClass> quarterFinalWinners)
{
    //Ne znam
}
static TeamClass FinalRound(List<TeamClass> semiFinalists)
{

    var finalWinner = semiFinalists.OrderByDescending(s => s.GetFIBARanking()).First();

    Console.WriteLine($"\nFinal:");
    foreach (var team in semiFinalists)
    {
        Console.WriteLine($"Team Name: {team.GetTeamName()}, FIBA Ranking: {team.GetFIBARanking()}");
    }
    Console.WriteLine($"Winner of the Final: {finalWinner.GetTeamName()}");

    return finalWinner;
}
