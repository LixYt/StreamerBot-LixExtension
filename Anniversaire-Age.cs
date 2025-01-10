using System;
using System.Collections.Generic;
using System.Linq;

public class CPHInline
{
	public bool Execute()
	{
		string value = args["rawInput"].ToString().Replace("h", ":");
		string msgId = args["msgId"].ToString();
		string userName = args["userName"].ToString();
		string toUserName = args["input0"].ToString();
		
		DateTime v = CPH.GetTwitchUserVar<DateTime>(toUserName == "" ? userName : toUserName, "BirthDate", true);
		if (v != new DateTime(1,1,1,0,0,0))
		{
			var Age = CalculateDateDifference(v);
			string AddMessage = "";
			
			var AllBirthDate = CPH.GetTwitchUsersVar<DateTime>("BirthDate", true).OrderByDescending(x => x.Value);
			var Vieux = AllBirthDate.Last();
			
			if(Vieux.UserName == (toUserName == "" ? userName : toUserName))
			{
				AddMessage = "C'est notre doyen ! Sa simple présence impose un respect éternel !";
			}
			
			var Jeune = AllBirthDate.First();
			{
				AddMessage = "Tu es le bidou de la chaine... enfin, aussi longtemps que quelqu'un de plus jeune ne déclare son !anniv";
			}
			
			CPH.TwitchReplyToMessage($"{(toUserName == "" ? userName : toUserName)} a {Age.years} ans, {Age.months} mois, {Age.days} jours. " + AddMessage , msgId, true);
					
			return true;
		}
		else 
		{
			CPH.SendMessage("Il faut d'abord me dire ton anniversaire avec la commande !anniv JJ/MM/AAAA et tu peux préciser l'heure en ajoutant HH:MM.", true, true);
		}
		
		// your main code goes here
		return false;
	}
	
	static (int years, int months, int days) CalculateDateDifference(DateTime startDate)
    {
        DateTime endDate = DateTime.Now; // Utiliser DateTime.Now comme date la plus récente
        int years = 0, months = 0, days = 0;

        // Si la date de début est après aujourd'hui, échangez-les
        if (startDate > endDate)
        {
            var temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        // Calcul des années
        years = endDate.Year - startDate.Year;

        // Calcul des mois
        months = endDate.Month - startDate.Month;
        if (months < 0)
        {
            years--;
            months += 12;
        }

        // Calcul des jours
        days = endDate.Day - startDate.Day;
        if (days < 0)
        {
            months--;
            var previousMonth = endDate.AddMonths(-1);
            days += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
        }

        return (years, months, days);
    }
}
