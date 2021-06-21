using System;

namespace ApiCore
{
	public class LogService
	{
		public static void Logar(string mensagem)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(mensagem);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}