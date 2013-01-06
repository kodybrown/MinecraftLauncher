using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace MinecraftLauncher
{
	public partial class MainForm : Form
	{
		private Button SelectedButton = null;

		public MainForm()
		{
			InitializeComponent();

			FormBorderStyle = FormBorderStyle.Sizable;
			MaximizeBox = true;
			MinimizeBox = true;
			MinimumSize = new Size(292, 109);
			ShowInTaskbar = true;
			StartPosition = FormStartPosition.CenterScreen;
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			DisplayClients();
		}

		private void MainForm_KeyDown( object sender, KeyEventArgs e )
		{
			if (e.KeyCode == Keys.F5) {
				DisplayClients();
			}
		}

		private void DisplayClients()
		{
			Cursor = Cursors.WaitCursor;
			flowLayoutPanel.Controls.Clear();
			foreach (KeyValuePair<int, DirectoryInfo> pair in Manager.Clients) {
				Button btn = CopyButton(clientButton);
				btn.Text = pair.Value.Name;
				btn.MouseDown += new MouseEventHandler(client_MouseDown);
				btn.Tag = pair.Key;
				flowLayoutPanel.Controls.Add(btn);
			}
			Cursor = Cursors.Default;
		}

		void client_MouseDown( object sender, MouseEventArgs e )
		{
			SelectedButton = sender as Button;
			if (SelectedButton == null) {
				return;
			}

			if (e.Button == System.Windows.Forms.MouseButtons.Left) {
				LaunchMinecraft((int)SelectedButton.Tag);
			} else if (e.Button == System.Windows.Forms.MouseButtons.Right) {
				contextMenuStrip.Show(MousePosition);
			}
		}

		private Button CopyButton( Button btn )
		{
			Button newBtn;

			newBtn = new Button();
			newBtn.AccessibleDefaultActionDescription = btn.AccessibleDefaultActionDescription;
			newBtn.AccessibleDescription = btn.AccessibleDescription;
			newBtn.AccessibleName = btn.AccessibleName;
			newBtn.AccessibleRole = btn.AccessibleRole;
			newBtn.AllowDrop = btn.AllowDrop;
			newBtn.Anchor = btn.Anchor;
			newBtn.AutoEllipsis = btn.AutoEllipsis;
			newBtn.AutoScrollOffset = btn.AutoScrollOffset;
			newBtn.AutoSize = btn.AutoSize;
			newBtn.AutoSizeMode = btn.AutoSizeMode;
			newBtn.BackColor = btn.BackColor;
			newBtn.BackgroundImage = btn.BackgroundImage;
			newBtn.BackgroundImageLayout = btn.BackgroundImageLayout;
			newBtn.Bounds = btn.Bounds;
			newBtn.Capture = btn.Capture;
			newBtn.CausesValidation = btn.CausesValidation;
			newBtn.ClientSize = btn.ClientSize;
			newBtn.ContextMenu = btn.ContextMenu;
			newBtn.ContextMenuStrip = btn.ContextMenuStrip;
			newBtn.Cursor = btn.Cursor;
			newBtn.DialogResult = btn.DialogResult;
			newBtn.Dock = btn.Dock;
			newBtn.Enabled = btn.Enabled;
			newBtn.FlatStyle = btn.FlatStyle;
			newBtn.Font = btn.Font;
			newBtn.ForeColor = btn.ForeColor;
			newBtn.Height = btn.Height;
			newBtn.Image = btn.Image;
			newBtn.ImageAlign = btn.ImageAlign;
			newBtn.ImageIndex = btn.ImageIndex;
			newBtn.ImageKey = btn.ImageKey;
			newBtn.ImageList = btn.ImageList;
			newBtn.IsAccessible = btn.IsAccessible;
			newBtn.Left = btn.Left;
			newBtn.Location = btn.Location;
			newBtn.Margin = btn.Margin;
			newBtn.MaximumSize = btn.MaximumSize;
			newBtn.MinimumSize = btn.MinimumSize;
			newBtn.Padding = btn.Padding;
			//newBtn.Parent = btn.Parent;
			//newBtn.Region = btn.Region;
			newBtn.RightToLeft = btn.RightToLeft;
			//newBtn.Site = btn.Site;
			newBtn.Size = btn.Size;
			newBtn.TabIndex = btn.TabIndex;
			newBtn.TabStop = btn.TabStop;
			//newBtn.ToolTipText = btn.ToolTipText;
			newBtn.Text = btn.Text;
			newBtn.TextAlign = btn.TextAlign;
			newBtn.TextImageRelation = btn.TextImageRelation;
			newBtn.Top = btn.Top;
			newBtn.UseCompatibleTextRendering = btn.UseCompatibleTextRendering;
			newBtn.UseMnemonic = btn.UseMnemonic;
			newBtn.UseVisualStyleBackColor = btn.UseVisualStyleBackColor;
			newBtn.UseWaitCursor = btn.UseWaitCursor;
			newBtn.Visible = true;// btn.Visible;
			newBtn.Width = btn.Width;

			return newBtn;
		}

		private void LaunchMinecraft( int Index )
		{
			string error;
			try {
				Cursor = Cursors.AppStarting;
				if ((error = Manager.Launch(Index)).Length == 0) {
					Thread.Sleep(5000);
					//Application.Exit();
				} else {
					MessageBox.Show(error, "Minecraft Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void launchMenu_Click( object sender, EventArgs e )
		{
			if (SelectedButton != null) {
				LaunchMinecraft((int)SelectedButton.Tag);
			}
		}

		private void renameMenu_Click( object sender, EventArgs e )
		{
			if (SelectedButton != null) {
				InputDialog f = new InputDialog("Rename Minecraft Client", "Enter the new name for the client:", SelectedButton.Text);
				if (f.ShowDialog() == DialogResult.OK) {
					try {
						Cursor = Cursors.WaitCursor;
						Thread.Sleep(50);
						Manager.Rename((int)SelectedButton.Tag, f.Input);
						DisplayClients();
					} catch (Exception ex) {
						MessageBox.Show(ex.Message, "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			Cursor = Cursors.Default;
		}

		private void copyMenu_Click( object sender, EventArgs e )
		{
			if (SelectedButton != null) {
				InputDialog f = new InputDialog("Copy Minecraft Client", "Enter the new name for the client:", SelectedButton.Text);
				if (f.ShowDialog() == DialogResult.OK) {
					try {
						Cursor = Cursors.WaitCursor;
						Thread.Sleep(50);
						Manager.Copy((int)SelectedButton.Tag, f.Input);
					} catch (Exception ex) {
						MessageBox.Show(ex.Message, "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			Cursor = Cursors.Default;
		}

		private void removeMenu_Click( object sender, EventArgs e )
		{
			if (SelectedButton != null) {
				DialogResult r = MessageBox.Show(this, @"Are you sure you want to remove this client?

Press Y to PERMANENTLY delete this client from the disk (this cannot be undone).
Press N to hide this client from Minecraft Launcher (but leave the client directory on disk).
Press C to cancel.
", "Remove Minecraft Client", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				try {
					Cursor = Cursors.WaitCursor;
					Thread.Sleep(50);
					if (r == DialogResult.Yes) {
						Manager.Delete((int)SelectedButton.Tag);
					} else if (r == DialogResult.No) {
						Manager.HideFromLauncher((int)SelectedButton.Tag);
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.Message, "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			Cursor = Cursors.Default;
		}

	}
}
