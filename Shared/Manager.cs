using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MinecraftLauncher
{
	public static class Manager
	{
		public static Dictionary<int, DirectoryInfo> Clients
		{
			get
			{
				if (_clients.Count == 0) {
					LoadClients();
				}
				return _clients;
			}
		}
		private static Dictionary<int, DirectoryInfo> _clients = new Dictionary<int, DirectoryInfo>();

		public static void LoadClients()
		{
			int index = 1;
			_clients.Clear();
			foreach (string dir in Directory.GetDirectories(Environment.CurrentDirectory, "*.*", SearchOption.TopDirectoryOnly)) {
				if (!Path.GetFileName(dir).StartsWith("~")) {
					_clients.Add(index++, new DirectoryInfo(dir));
				}
			}
		}

		public static string Launch( int Index )
		{
			string appData = Environment.ExpandEnvironmentVariables(@"%AppData%\.minecraft");

			if (Directory.Exists(appData)) {
				if (JunctionPoint.Exists(appData)) {
					JunctionPoint.Delete(appData);
				} else {
					return "You must rename or move your existing .minecraft folder before continuing.\nIt is located here: \"" + appData + "\"";
				}
			}

			JunctionPoint.Create(appData, Manager.Clients[Index].FullName, true);

			ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(appData, "Minecraft.exe"));
			startInfo.WorkingDirectory = Manager.Clients[Index].FullName;
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Process.Start(startInfo);

			return string.Empty;
		}

		public static void Rename( int Index, string NewName )
		{
			string dest = Path.Combine(Manager.Clients[Index].Parent.FullName, NewName);
			if (!dest.Equals(Manager.Clients[Index].Name, StringComparison.InvariantCultureIgnoreCase)) {
				Manager.Clients[Index].MoveTo(dest);
			}
		}

		public static void Copy( int Index, string NewName )
		{
			string dest = Path.Combine(Manager.Clients[Index].Parent.FullName, NewName);
			if (!dest.Equals(Manager.Clients[Index].Name, StringComparison.InvariantCultureIgnoreCase)) {
				if (!Directory.Exists(dest)) {
					DirectoryExtensions.Copy(Manager.Clients[Index].FullName, dest, true);
				}
			}
		}

		public static void Delete( int Index )
		{
			//Directory.Delete(Manager.Clients[Index].FullName, true);
			string NewName = "~DELETED~" + Manager.Clients[Index].Name;
			string dest = Path.Combine(Manager.Clients[Index].Parent.FullName, NewName);
			if (!Directory.Exists(dest)) {
				Manager.Clients[Index].MoveTo(dest);
			}
		}

		public static void HideFromLauncher( int Index )
		{
			string NewName = "~" + Manager.Clients[Index].Name;
			string dest = Path.Combine(Manager.Clients[Index].Parent.FullName, NewName);
			Manager.Clients[Index].MoveTo(dest);
		}

	}
}
