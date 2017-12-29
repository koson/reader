using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace BBeBinderPlugins
{
    /// <summary>
    /// Displays a prompt in a dialog box, waits for the user to input text or click a button, and then returns a string containing the contents of the text box.
    /// </summary>
    public class InputBox : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnLoadAsHtml;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtResult;
        private string strReturnValue;
        private Point pntStartLocation;

        public enum Buttons { CANCEL, LOAD_PAGE, LOAD_AS_IMAGES };
        Buttons buttonClicked = Buttons.CANCEL;
        private Button btnBrowse;

        public Buttons ButtonClicked
        {
            get { return buttonClicked; }
        }

        public string getReturnValue
        {
            get { return strReturnValue; }
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        /// <summary>
        /// Displays a prompt in a dialog box, waits for the user to input text or click a button, and then returns a string containing the contents of the text box.
        /// </summary>
        /// <param name="Prompt">String expression displayed as the message in the dialog box.</param>
        /// <param name="Title">String expression displayed in the title bar of the dialog box.</param>
        /// <returns>The value in the textbox is returned if the user clicks OK or presses the ENTER key. If the user clicks Cancel, a zero-length string is returned.</returns>
        public static InputBox Show(string Prompt, string Title)
        {
            return Show(Prompt, Title, "", -1, -1);
        }

        /// <summary>
        /// Displays a prompt in a dialog box, waits for the user to input text or click a button, and then returns a string containing the contents of the text box.
        /// </summary>
        /// <param name="Prompt">String expression displayed as the message in the dialog box.</param>
        /// <param name="Title">String expression displayed in the title bar of the dialog box.</param>
        /// <param name="DefaultResponse">String expression displayed in the text box as the default response if no other input is provided. If you omit DefaultResponse, the displayed text box is empty.</param>
        /// <returns>The value in the textbox is returned if the user clicks OK or presses the ENTER key. If the user clicks Cancel, a zero-length string is returned.</returns>
        public static InputBox Show(string Prompt, string Title, string DefaultResponse)
        {
            return Show(Prompt, Title, DefaultResponse, -1, -1);
        }

        /// <summary>
        /// Displays a prompt in a dialog box, waits for the user to input text or click a button, and then returns a string containing the contents of the text box.
        /// </summary>
        /// <param name="Prompt">String expression displayed as the message in the dialog box.</param>
        /// <param name="Title">String expression displayed in the title bar of the dialog box.</param>
        /// <param name="DefaultResponse">String expression displayed in the text box as the default response if no other input is provided. If you omit DefaultResponse, the displayed text box is empty.</param>
        /// <param name="XPos">Integer expression that specifies, in pixels, the distance of the left edge of the dialog box from the left edge of the screen.</param>
        /// <param name="YPos">Integer expression that specifies, in pixels, the distance of the upper edge of the dialog box from the top of the screen.</param>
        /// <returns>The value in the textbox is returned if the user clicks OK or presses the ENTER key. If the user clicks Cancel, a zero-length string is returned.</returns>
        public static InputBox Show(string Prompt, string Title, string DefaultResponse, int XPos, int YPos)
        {
            // Create a new input box dialog
            InputBox frmInputBox = new InputBox();
            frmInputBox.Title = Title;
            frmInputBox.Prompt = Prompt;
            frmInputBox.DefaultResponse = DefaultResponse;
            if (XPos >= 0 && YPos >= 0) frmInputBox.StartLocation = new Point(XPos, YPos);
            frmInputBox.ShowDialog();
            return frmInputBox;
        }


		public InputBox()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			this.strReturnValue = "";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.btnLoadAsHtml = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadAsHtml
            // 
            this.btnLoadAsHtml.Location = new System.Drawing.Point(358, 8);
            this.btnLoadAsHtml.Name = "btnLoadAsHtml";
            this.btnLoadAsHtml.Size = new System.Drawing.Size(72, 24);
            this.btnLoadAsHtml.TabIndex = 0;
            this.btnLoadAsHtml.Text = "&Load Page";
            this.btnLoadAsHtml.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(12, 75);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(357, 20);
            this.txtResult.TabIndex = 0;
            // 
            // lblText
            // 
            this.lblText.Location = new System.Drawing.Point(16, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(336, 64);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "InputBox";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(358, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(375, 72);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(55, 23);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // InputBox
            // 
            this.AcceptButton = this.btnLoadAsHtml;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(438, 105);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnLoadAsHtml);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputBox";
            this.Load += new System.EventHandler(this.InputBoxForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void InputBoxForm_Load(object sender, System.EventArgs e)
		{
			if (!this.pntStartLocation.IsEmpty) 
			{
				this.Top = this.pntStartLocation.X;
				this.Left = this.pntStartLocation.Y;
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.strReturnValue = this.txtResult.Text;
            this.buttonClicked = Buttons.LOAD_PAGE;
			this.Close();
		}

        private void btnLoadAsImages_Click(object sender, EventArgs e)
        {
            this.strReturnValue = this.txtResult.Text;
            this.buttonClicked = Buttons.LOAD_AS_IMAGES;
            this.Close();
        }


		private void btnCancel_Click(object sender, System.EventArgs e)
		{
            this.buttonClicked = Buttons.CANCEL;
            this.Close();
		}

		public string Title
		{
			set
			{
				this.Text = value;
			}
		}

		public string Prompt
		{
			set
			{
				this.lblText.Text = value;
			}
		}

		public string ReturnValue
		{
			get
			{
				return strReturnValue;
			}
		}

		public string DefaultResponse
		{
			set
			{
				this.txtResult.Text = value;
				this.txtResult.SelectAll();
			}
		}

		public Point StartLocation
		{
			set
			{
				this.pntStartLocation = value;
			}
		}

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "HTML Files (*.htm, *.htm)|*.HTM;*.HTML";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtResult.Text = dialog.FileName;
            }
        }

    }
}
