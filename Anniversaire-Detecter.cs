using System;

public class CPHInline
{
	public bool Execute()
	{
		string userName = args["userName"].ToString();
		
		DateTime v = CPH.GetTwitchUserVar<DateTime>(userName, "BirthDate", true);
		if (v != new DateTime(1,1,1,0,0,0))
		{
			DateTime AnnivAnnee = new DateTime(DateTime.Now.Year, v.Month, v.Day, v.Hour, v.Minute, 0);
			bool estMoinsDe48h = Math.Abs((DateTime.Now - AnnivAnnee).TotalHours) <= 48;
			return estMoinsDe48h;
		}
		
		// your main code goes here
		return false;
	}
}
