using System.Collections.Generic;

namespace TextChat.Extensions
{
	public static class String
	{
		public static string Sanitize(this string stringToSanitize, IEnumerable<string> badWords, char badWordsChar)
		{
			foreach (string badWord in badWords) stringToSanitize.Replace(badWord, new string(badWordsChar, badWord.Length));

			return stringToSanitize;
		}
	}
}
