using System;

public class CPHInline
{
	public bool Execute()
	{
		// your main code goes here
		string User = args["user"].ToString();
		string Command = args["command"].ToString();
		
		int total = 0;
		string rolled = "";
			
		if (Command == "!roll")
		{
			string Dice = args["rawInput"].ToString();
			string nbDice = Dice.Split('d')[0];
			string DiceFaces = Dice.Split('d')[1];
		
			int nb = Convert.ToInt32(nbDice);
			int max = Convert.ToInt32(DiceFaces);
			
			Random rnd = new Random();
			for (int i = 0; i < nb; i++)
			{
				int n = rnd.Next(1, max);
				total += n;
				rolled += $" {n},";
			}
			
			rolled = rolled.Substring(0, rolled.Length - 1);
			CPH.SendMessage($"{User} a lancé {nb}d{max} obtenu : {total} avec les lancés suivants : {rolled}.");
		}
		else if (Command == "!vsroll")
		{
			string DiceAndCompare = args["rawInput"].ToString();
			string Dice = DiceAndCompare.Split(' ')[0];
			string nbDice = Dice.Split('d')[0];
			string DiceFaces = Dice.Split('d')[1];
			
			int nb = Convert.ToInt32(nbDice);
			int max = Convert.ToInt32(DiceFaces);
			
			string Cref = DiceAndCompare.Split(' ')[1];
			int CompareTo = Convert.ToInt32(DiceAndCompare.Split(' ')[1]);
			
			CPH.SendMessage($"{User} a lancé {nb} dès à {max} faces");
			
			Random rnd = new Random();
			for (int i = 0; i < nb; i++)
			{
				int n = rnd.Next(1, max);
				total += n;
				rolled += $"{n},";
			}
			
			rolled = nb == 1 ? "" : "("+rolled.Substring(0, rolled.Length - 1)+")";
			
			string Result = "";
			if (total >= 95)
			{
				Result = $"ECHEC CRITIQUE <= {total} {rolled}";
			}
			else if (total <= 5)
			{
				Result = $"REUSSITE CRITIQUE <= {total} {rolled}";
			}
			else if (total <= CompareTo)
			{
				Result = $"REUSSITE <= {total} {rolled}";
			}
			else
			{
				Result = $"ECHEC <= {total} {rolled}";
			}
			
			CPH.SendMessage(Result);
		}
		
		return true;
	}
	
}
