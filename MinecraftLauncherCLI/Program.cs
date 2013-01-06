using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MinecraftLauncher
{
	public class Program
	{
		public static void Main( string[] args )
		{
			int index;
			int width = 62;
			int indent;

			while (true) {
				Manager.LoadClients();

				indent = Manager.Clients.Count.ToString().Length;

				Console.Clear();
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("       ┌─{0,-" + width + "}─┐", new string('─', width));
				Console.WriteLine("       │ {0,-" + width + "} │", "                      Minecraft Launcher");
				Console.WriteLine("       ├─{0,-" + width + "}─┤", new string('─', width));

				foreach (KeyValuePair<int, DirectoryInfo> pair in Manager.Clients) {
					Console.WriteLine("       │ {0,-" + width + "} │", string.Format("[{0," + indent + "}] {1}", pair.Key, pair.Value.Name));
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
						if (index > 0 && index <= Manager.Clients.Count) {
							Console.WriteLine("Type the new name for item: ");
							line = Console.ReadLine();
							Manager.Rename(index, line);
						}
					}
				} else if (line.Trim().Equals("copy", StringComparison.InvariantCultureIgnoreCase)) {
					Console.Write("Type number of item to copy: ");
					line = Console.ReadLine();
					if (int.TryParse(line, out index)) {
						if (index > 0 && index <= Manager.Clients.Count) {
							Console.WriteLine("Type the new name for item: ");
							line = Console.ReadLine();
							Manager.Copy(index, line);
						}
					}
				} else if (line.Trim().Equals("delete", StringComparison.InvariantCultureIgnoreCase)) {
					Console.Write("Type number of item to delete: ");
					line = Console.ReadLine();
					if (int.TryParse(line, out index)) {
						if (index > 0 && index <= Manager.Clients.Count) {
							Console.WriteLine("Are you sure you want to DELETE this item? [y/N]: ");
							line = Console.ReadLine();
							if (line.Equals("y", StringComparison.InvariantCultureIgnoreCase)) {
								Manager.Delete(index);
							}
						}
					}
				} else if (line.Trim().Equals("hide", StringComparison.InvariantCultureIgnoreCase)) {
					Console.Write("Type number of item to hide: ");
					line = Console.ReadLine();
					if (int.TryParse(line, out index)) {
						if (index > 0 && index <= Manager.Clients.Count) {
							Console.WriteLine("Are you sure you want to hide this item? [Y/n]: ");
							line = Console.ReadLine();
							if (line.Length == 0 || line.Equals("y", StringComparison.InvariantCultureIgnoreCase)) {
								Manager.HideFromLauncher(index);
							}
						}
					}
				} else if (int.TryParse(line, out index)) {
					if (index > 0 && index <= Manager.Clients.Count) {
						break;
					}
				}
			}

			string error;
			if ((error = Manager.Launch(index)).Length > 0) {
				Console.WriteLine();
				Console.WriteLine(error);
				Console.WriteLine("Press any key to quit.");
				Console.ReadKey(true);
				return;
			}

			Console.WriteLine();
			Console.WriteLine("Launching...");
			Thread.Sleep(3500);
		}
	}
}
