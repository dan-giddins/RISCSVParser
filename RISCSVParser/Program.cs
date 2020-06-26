using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RISCSVParser
{
	class Program
	{
		private const string ReadDir =
			@"C:\Users\uizdg\The University of Nottingham\Official RIS Team - Documents\Dan Giddins RIS Work\";
		private const string WriteDir =
			@"C:\Users\uizdg\The University of Nottingham\Official RIS Team - Documents\Dan Giddins RIS Work\csv\";
		private const string OldFilename = "appointments_before";
		private const string NewFilename = "appointments_rounding";

		static void Main() =>
			CompareFTE();

		private static void CompareFTE()
		{
			var output = new List<string[]> { new string[] { "record_id", "old_fte", "new_fte" } };
			var oldCSV = GetData(OldFilename);
			var newCSV = GetData(NewFilename);
			foreach (var oldLine in oldCSV)
			{
				string[] outputLine;
				var newLine = newCSV.Where(x => x[1] == oldLine[1]).FirstOrDefault();
				var oldFTE = oldLine[12];
				if (newLine == null)
				{
					outputLine = new string[] { oldLine[1], oldFTE, "NO RECORD!" };
					Console.WriteLine(outputLine);
					output.Add(outputLine);
				}
				//else
				//{
				//	var newFTE = newLine[12];
				//	if (oldFTE != newFTE)
				//	{
				//		outputLine = new string[] { oldLine[1], oldFTE, newFTE };
				//		output.Add(outputLine);
				//	}
				//}
			}
			WriteToFile(output);
		}

		private static void WriteToFile(List<string[]> output)
		{
			Directory.CreateDirectory(WriteDir);
			var sw = new StreamWriter($"{WriteDir}{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_csv.csv");
			foreach (var line in output)
			{
				sw.WriteLine(line);
			}
		}

		private static IList<string[]> GetData(string filename)
		{
			var result = new List<string[]>();
			var sr = new StreamReader($"{ReadDir}{filename}.csv");
			string line;
			while ((line = sr.ReadLine()) != null)
			{
				result.Add(line.Split(','));
			}
			return result;
		}
	}
}
