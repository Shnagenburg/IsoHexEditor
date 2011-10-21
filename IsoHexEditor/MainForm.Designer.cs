namespace IsoHexEditor
{
    partial class MainForm
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
            this.vertexColor3 = new System.Windows.Forms.ComboBox();
            this.vertexColor2 = new System.Windows.Forms.ComboBox();
            this.vertexColor1 = new System.Windows.Forms.ComboBox();
            this.chkDrawWireframe = new System.Windows.Forms.CheckBox();
            this.chkDrawModel = new System.Windows.Forms.CheckBox();
            this.tbDepth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.isoHexControl = new IsoHexEditor.IsoHexControl();
            this.SuspendLayout();
            // 
            // vertexColor3
            // 
            this.vertexColor3.DropDownHeight = 500;
            this.vertexColor3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vertexColor3.FormattingEnabled = true;
            this.vertexColor3.IntegralHeight = false;
            this.vertexColor3.Items.AddRange(new object[] {
            "BurlyWood",
            "Chartreuse",
            "Coral",
            "CornflowerBlue",
            "Cornsilk",
            "Firebrick",
            "Fuchsia",
            "Goldenrod",
            "Indigo",
            "Tan",
            "Teal",
            "Thistle",
            "Tomato"});
            this.vertexColor3.Location = new System.Drawing.Point(440, 63);
            this.vertexColor3.Name = "vertexColor3";
            this.vertexColor3.Size = new System.Drawing.Size(103, 21);
            this.vertexColor3.TabIndex = 7;
            this.vertexColor3.SelectedIndexChanged += new System.EventHandler(this.vertexColor_SelectedIndexChanged);
            // 
            // vertexColor2
            // 
            this.vertexColor2.DropDownHeight = 500;
            this.vertexColor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vertexColor2.FormattingEnabled = true;
            this.vertexColor2.IntegralHeight = false;
            this.vertexColor2.Items.AddRange(new object[] {
            "BurlyWood",
            "Chartreuse",
            "Coral",
            "CornflowerBlue",
            "Cornsilk",
            "Firebrick",
            "Fuchsia",
            "Goldenrod",
            "Indigo",
            "Tan",
            "Teal",
            "Thistle",
            "Tomato"});
            this.vertexColor2.Location = new System.Drawing.Point(331, 63);
            this.vertexColor2.Name = "vertexColor2";
            this.vertexColor2.Size = new System.Drawing.Size(103, 21);
            this.vertexColor2.TabIndex = 6;
            this.vertexColor2.SelectedIndexChanged += new System.EventHandler(this.vertexColor_SelectedIndexChanged);
            // 
            // vertexColor1
            // 
            this.vertexColor1.DropDownHeight = 500;
            this.vertexColor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vertexColor1.FormattingEnabled = true;
            this.vertexColor1.IntegralHeight = false;
            this.vertexColor1.Items.AddRange(new object[] {
            "BurlyWood",
            "Chartreuse",
            "Coral",
            "CornflowerBlue",
            "Cornsilk",
            "Firebrick",
            "Fuchsia",
            "Goldenrod",
            "Indigo",
            "Tan",
            "Teal",
            "Thistle",
            "Tomato"});
            this.vertexColor1.Location = new System.Drawing.Point(222, 63);
            this.vertexColor1.Name = "vertexColor1";
            this.vertexColor1.Size = new System.Drawing.Size(103, 21);
            this.vertexColor1.TabIndex = 5;
            this.vertexColor1.SelectedIndexChanged += new System.EventHandler(this.vertexColor_SelectedIndexChanged);
            // 
            // chkDrawWireframe
            // 
            this.chkDrawWireframe.AutoSize = true;
            this.chkDrawWireframe.Checked = true;
            this.chkDrawWireframe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawWireframe.Location = new System.Drawing.Point(632, 28);
            this.chkDrawWireframe.Name = "chkDrawWireframe";
            this.chkDrawWireframe.Size = new System.Drawing.Size(102, 17);
            this.chkDrawWireframe.TabIndex = 9;
            this.chkDrawWireframe.Text = "Draw Wireframe";
            this.chkDrawWireframe.UseVisualStyleBackColor = true;
            this.chkDrawWireframe.CheckedChanged += new System.EventHandler(this.chkDrawWireFrame_Pressed);
            // 
            // chkDrawModel
            // 
            this.chkDrawModel.AutoSize = true;
            this.chkDrawModel.Checked = true;
            this.chkDrawModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDrawModel.Location = new System.Drawing.Point(632, 51);
            this.chkDrawModel.Name = "chkDrawModel";
            this.chkDrawModel.Size = new System.Drawing.Size(83, 17);
            this.chkDrawModel.TabIndex = 10;
            this.chkDrawModel.Text = "Draw Model";
            this.chkDrawModel.UseVisualStyleBackColor = true;
            this.chkDrawModel.CheckedChanged += new System.EventHandler(this.chkDrawModel_Pressed);
            // 
            // tbDepth
            // 
            this.tbDepth.Location = new System.Drawing.Point(74, 63);
            this.tbDepth.Name = "tbDepth";
            this.tbDepth.Size = new System.Drawing.Size(100, 20);
            this.tbDepth.TabIndex = 11;
            this.tbDepth.TextChanged += new System.EventHandler(this.tbDepth_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Depth:";
            // 
            // isoHexControl
            // 
            this.isoHexControl.HexGrid = null;
            this.isoHexControl.Location = new System.Drawing.Point(12, 106);
            this.isoHexControl.Name = "isoHexControl";
            this.isoHexControl.SelectedHex = null;
            this.isoHexControl.Size = new System.Drawing.Size(768, 455);
            this.isoHexControl.TabIndex = 8;
            this.isoHexControl.Text = "isoHexControl";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbDepth);
            this.Controls.Add(this.chkDrawModel);
            this.Controls.Add(this.chkDrawWireframe);
            this.Controls.Add(this.isoHexControl);
            this.Controls.Add(this.vertexColor3);
            this.Controls.Add(this.vertexColor2);
            this.Controls.Add(this.vertexColor1);
            this.Name = "MainForm";
            this.Text = "IsoHexEditor";
            this.LocationChanged += new System.EventHandler(this.EditorWindowLocation_Changed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private IsoHexControl isoHexControl;
        private System.Windows.Forms.ComboBox vertexColor3;
        private System.Windows.Forms.ComboBox vertexColor2;
        private System.Windows.Forms.ComboBox vertexColor1;
        private System.Windows.Forms.CheckBox chkDrawWireframe;
        private System.Windows.Forms.CheckBox chkDrawModel;
        private System.Windows.Forms.TextBox tbDepth;
        private System.Windows.Forms.Label label1;

    }
}

