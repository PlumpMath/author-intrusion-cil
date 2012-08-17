namespace AuthorIntrustionSwf
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.webControl = new Awesomium.Windows.Forms.WebControl();
			this.SuspendLayout();
			// 
			// webControl
			// 
			this.webControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webControl.Location = new System.Drawing.Point(0, 0);
			this.webControl.Name = "webControl";
			this.webControl.Size = new System.Drawing.Size(568, 493);
			this.webControl.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 493);
			this.Controls.Add(this.webControl);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}

		#endregion

		private Awesomium.Windows.Forms.WebControl webControl;
	}
}

