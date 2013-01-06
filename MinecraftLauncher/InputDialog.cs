using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftLauncher
{
	public partial class InputDialog : Form
	{
		private delegate void Func();

		public InputDialog()
		{
			InitializeComponent();

			AcceptButton = okButton;
			CancelButton = cancelButton;
			FormBorderStyle = FormBorderStyle.Sizable;
			MaximizeBox = false;
			MinimizeBox = false;
			MaximumSize = new Size(413, 264);
			MinimumSize = new Size(413, 164);
			ShowInTaskbar = false;
			Size = MinimumSize;
			StartPosition = FormStartPosition.CenterParent;
		}

		public InputDialog( string Caption, string Prompt, string DefaultInput )
			: this()
		{
			SetCaption(Caption);
			SetPrompt(Prompt);
			SetInput(DefaultInput);
		}

		private void InputDialog_Load( object sender, EventArgs e )
		{
			inputBox.Focus();
			inputBox.SelectAll();
		}

		public string Caption { get { return GetCaption(); } set { SetCaption(value); } }
		public string GetCaption()
		{
			return this.Text;
		}
		public void SetCaption( string CaptionText )
		{
			if (InvokeRequired) {
				Func del = delegate { SetCaption(CaptionText); };
				BeginInvoke(del);
			} else {
				this.Text = CaptionText ?? string.Empty;
			}
		}

		public string Prompt { get { return GetPrompt(); } set { SetPrompt(value); } }
		public string GetPrompt()
		{
			return promptLabel.Text;
		}
		public void SetPrompt( string PromptText )
		{
			if (InvokeRequired) {
				Func del = delegate { SetPrompt(PromptText); };
				BeginInvoke(del);
			} else {
				promptLabel.Text = PromptText ?? string.Empty;
			}
		}

		public string Input { get { return GetInput(); } set { SetInput(value); } }
		public string GetInput()
		{
			return inputBox.Text;
		}
		public void SetInput( string InputText )
		{
			if (InvokeRequired) {
				Func del = delegate { SetInput(InputText); };
				BeginInvoke(del);
			} else {
				inputBox.Text = InputText ?? string.Empty;
			}
		}
	}
}
