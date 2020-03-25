using System.Collections.Generic;
using System.Linq;

namespace TextChat.Extensions
{
	public static class String
	{
		public static string Sanitize(this string stringToSanitize, IEnumerable<string> badWords, char badWordsChar)
		{
			foreach (string badWord in badWords) stringToSanitize.Replace(badWord, new string(badWordsChar, badWord.Length));

			return stringToSanitize;
		}

		public static string GetMessage(this string[] args, int skips = 0, string separator = " ")
		{
			return string.Join(separator, skips == 0 ? args : args.Skip(skips).Take(args.Length - skips));
		}
	}
}
