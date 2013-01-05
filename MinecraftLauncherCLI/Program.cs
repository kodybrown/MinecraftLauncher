using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MinecraftLauncher
{
	public class Program
	{
		public static void Main( string[] args )
		{
			string basePath = @"C:\Minecraft";
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			int index;
			int width = 56;
			int indent;

			while (true) {
				dictionary.Clear();
				index = 1;
				foreach (string dir in Directory.GetDirectories(basePath, "client*.*", SearchOption.TopDirectoryOnly)) {
					dictionary.Add(index++, dir);
				}

				indent = dictionary.Count.ToString().Length;

				Console.Clear();
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("       ┌─{0,-" + width + "}─┐", new string('─', width));
				Console.WriteLine("       │ {0,-" + width + "} │", "                   Minecraft Launcher");
				Console.WriteLine("       ├─{0,-" + width + "}─┤", new string('─', width));

				foreach (KeyValuePair<int, string> pair in dictionary) {
					string name = Path.GetFileName(pair.Value);
					string version;
					string desc;
					int pos = name.IndexOf('-');

					if (pos > -1) {
						// name == "client 1.4.5-Something"
						version = name.Substring(6, pos - 6).Trim();
						desc = name.Substring(pos + 1);
					} else {
						// name == "client 1.4.5"
						version = name.Substring(6).Trim();
						desc = string.Empty;
					}

					Console.WriteLine("       │ {0,-" + width + "} │", string.Format("[{2," + indent + "}] {1} ({0})", version, desc.Length > 0 ? desc : "--unnamed--", pair.Key));
				}

				Console.WriteLine("       └─{0,-" + width + "}─┘", new string('─', width));

				Console.WriteLine();
				Console.Write("        Type your selection (or 'q' to quit) then press Enter: ");
				string line = Console.ReadLine();

				if (line.Trim().Equals("q", StringComparison.InvariantCultureIgnoreCase)
						|| line.Trim().Equals("quit", StringComparison.InvariantCultureIgnoreCase)) {
					return;
				} else if (line.Trim().Equals("rename", StringComparison.InvariantCultureIgnoreCase)) {
					Console.Write("Type number of item to rename: ");
					line = Console.ReadLine();
					if (int.TryParse(line, out index)) {
						if (index > 0 && index <= dictionary.Count) {
							Console.WriteLine("Type the new name for item: ");
							line = Console.ReadLine();
							string name = Path.GetFileName(dictionary[index]);
							Directory.Move(dictionary[index],
								Path.Combine(Path.GetDirectoryName(dictionary[index]),
									string.Format("client{0}-{1}",
										name.Substring(6, name.IndexOf('-') - 6), line)));
						}
					}
				} else if (line.Trim().Equals("copy", StringComparison.InvariantCultureIgnoreCase)) {
					Console.Write("Type number of item to copy: ");
					line = Console.ReadLine();
					if (int.TryParse(line, out index)) {
						if (index > 0 && index <= dictionary.Count) {
							Console.WriteLine("Type the new name for item: ");
							line = Console.ReadLine();
							string name = Path.GetFileName(dictionary[index]);
							DirectoryExtensions.Copy(dictionary[index],
								Path.Combine(Path.GetDirectoryName(dictionary[index]),
									string.Format("client{0}-{1}",
										name.Substring(6, name.IndexOf('-') - 6), line)));
						}
					}
				} else if (line.Trim().Equals("delete", StringComparison.InvariantCultureIgnoreCase)) {
					Console.Write("Type number of item to delete: ");
					line = Console.ReadLine();
					if (int.TryParse(line, out index)) {
						if (index > 0 && index <= dictionary.Count) {
							Console.WriteLine("Are you sure you want to DELETE this item? [y/N]: ");
							line = Console.ReadLine();
							if (line.Equals("y", StringComparison.InvariantCultureIgnoreCase)) {
								Directory.Delete(dictionary[index], true);
							}
						}
					}
				} else if (int.TryParse(line, out index)) {
					if (index > 0 && index <= dictionary.Count) {
						break;
					}
				}
			}

			string appData = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft");

			if (Directory.Exists(appData)) {
				if (JunctionPoint.Exists(appData)) {
					JunctionPoint.Delete(appData);
				} else {
					Console.WriteLine();
					Console.WriteLine(@"You must rename or move your existing .minecraft folder before continuing.
It is located here: """ + appData + "\"");
					Console.WriteLine("Press any key to quit.");
					Console.ReadKey(true);
					return;
				}
			}

			JunctionPoint.Create(appData, dictionary[index], true);

			string mc = Path.Combine(appData, "Minecraft.exe");
			//Console.WriteLine(dictionary[index]);
			//Console.WriteLine(mc);
			//Console.ReadKey(true);

			ProcessStartInfo startInfo = new ProcessStartInfo(mc);
			startInfo.WorkingDirectory = dictionary[index];
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Process.Start(startInfo);

			Console.WriteLine();
			Console.WriteLine("Launching...");
			Thread.Sleep(5000);
		}
	}
}
