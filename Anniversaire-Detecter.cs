using System;

public class CPHInline
{
	public bool Execute()
	{
		string userName = args["userName"].ToString();
		int Delta = 2; //set this to allow the birthDay event to be triggered at + or - Delta days.
		
		DateTime v = CPH.GetTwitchUserVar<DateTime>(userName, "BirthDate", true);
		if (v != new DateTime(1,1,1,0,0,0))
		{
			if ( 	 v.Month == DateTime.Now.Month 
					&& 
					(v.Day <= (DateTime.Now.Day + Delta) && v.Day >= (DateTime.Now.Day - Delta))	)
			{
				return true;
			}
			else 
			{
				return false;
			}
		}
		
		// your main code goes here
		return false;
	}
}
