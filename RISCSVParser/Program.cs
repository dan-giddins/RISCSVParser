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
			@"C:\Users\uizdg\The University of Nottingham\Official RIS Team - Documents\Dan Giddins RIS Work\fte_compare\";
		private const string OldFilename = "old_fte_values";
		private const string NewFilename = "future_fte_fix";
		private const string AllPeopleFilename = "2020_06_29_15_21_52_all_people";

		static void Main() =>
			CompareFTE();

		private static void CompareFTE()
		{
			var output = new List<string[]> { new string[] { "appointment_id", "old_fte", "new_fte" } };
			var allPeople = GetData(AllPeopleFilename).Select(x => x[0]).ToList();
			var oldCSV = GetData(OldFilename);//.Where(x => allPeople.Contains(x[0])).ToList();
			var newCSV = GetData(NewFilename);//.Where(x => allPeople.Contains(x[0])).ToList();
			foreach (var oldLine in oldCSV)
			{
				string[] outputLine;
				var newLine = newCSV.Where(x => x[1] == oldLine[1]).FirstOrDefault();
				var oldFTE = oldLine[12];
				if (newLine == null)
				{
					outputLine = new string[] { oldLine[1], oldFTE, "NO RECORD!" };
					output.Add(outputLine);
				}
				else
				{
					var newFTE = newLine[12];
					if (oldFTE != newFTE)
					{
						outputLine = new string[] { oldLine[1], oldFTE, newFTE };
						output.Add(outputLine);
					}
				}
			}
			WriteToFile(output);
		}

		private static void WriteToFile(List<string[]> output)
		{
			Directory.CreateDirectory(WriteDir);
			using (var sw = new StreamWriter($"{WriteDir}{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_fte_compare.csv"))
			{
				foreach (var line in output)
				{
					sw.WriteLine(string.Join(",", line));
				}
			}
		}

		private static IList<string[]> GetData(string filename)
		{
			var result = new List<string[]>();
			var sr = new StreamReader($"{ReadDir}{filename}.csv");
			string line;
			while ((line = sr.ReadLine()) != null)
			{
				result.Add(line.Split(',', StringSplitOptions.None));
			}
			return result;
		}
	}
}
