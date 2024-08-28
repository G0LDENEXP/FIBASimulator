using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace KosarkaTurnir
{
    public class TeamClass
    {
       string TeamName { get; set; }
        string TeamISO { get; set; }
        int FIBARanking { get; set; }
        int TeamPoints { get; set; }

        private string assignedGroup { get; set; }

        public int winPoints;

        public TeamClass(string teamName, string teamISO, int fibaRanking, int teamPoints, string assignedGroup)
        {
            TeamName = teamName;
            TeamISO = teamISO;
            FIBARanking = fibaRanking;
            TeamPoints = teamPoints;
            this.assignedGroup = assignedGroup;
        }

        public string GetTeamName()
        {
            return TeamName;
        }
        public void SetTeamName()
        {
            TeamName = "";
        }

        public string GetTeamIso()
        {
            return TeamISO;
        }
        public void SetTeamIso()
        {
            TeamISO = "";
        }

        public int GetFIBARanking()
        {
            return FIBARanking;
        }
        public void SetFIBARanking()
        {
            FIBARanking = 0;
        }

        public int GetTeamPoints()
        {
            return TeamPoints;
        }
        public void SetTeamPoints()
        {
            TeamPoints = 0;
        }

        public string GetAssignedGroup() => assignedGroup;
        public void SetAssignedGroup(string group)
        {
            assignedGroup = group;
        }

        public void DisplayTeamInfo()
        {
            Console.WriteLine($"Team name: {TeamName}");
            Console.WriteLine($"Team ISO: {TeamISO}");
            Console.WriteLine($"FIBA Rank: {FIBARanking}\n");
        }

        public override int GetHashCode()
        {
            return TeamName.GetHashCode() ^ TeamISO.GetHashCode() ^ FIBARanking.GetHashCode() ^ TeamPoints.GetHashCode();
        }
    }
}
