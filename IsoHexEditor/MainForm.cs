#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace IsoHexEditor
{
    // System.Drawing and the XNA Framework both define Color types.
    // To avoid conflicts, we define shortcut names for them both.
    using GdiColor = System.Drawing.Color;
    using XnaColor = Microsoft.Xna.Framework.Color;

    
    /// <summary>
    /// Custom form provides the main user interface for the program.
    /// In this sample we used the designer to add a splitter pane to the form,
    /// which contains a SpriteFontControl and a SpinningTriangleControl.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            vertexColor1.SelectedIndex = 1;
            vertexColor2.SelectedIndex = 2;
            vertexColor3.SelectedIndex = 4;

            //isoHexControl.WindowLocation = new Microsoft.Xna.Framework.Point(this.Location.Y, this.Location.Y);
            isoHexControl.WindowLocation.X = this.Location.X;
            isoHexControl.WindowLocation.Y = this.Location.Y;

        
        }


        /// <summary>
        /// Event handler updates the spinning triangle control when
        /// one of the three vertex color combo boxes is altered.
        /// </summary>
        void vertexColor_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Which vertex was changed?
            int vertexIndex;

            if (sender == vertexColor1)
                vertexIndex = 0;
            else if (sender == vertexColor2)
                vertexIndex = 1;
            else if (sender == vertexColor3)
                vertexIndex = 2;
            else
                return;

            // Which color was selected?
            ComboBox combo = (ComboBox)sender;

            string colorName = combo.SelectedItem.ToString();

            GdiColor gdiColor = GdiColor.FromName(colorName);

            XnaColor xnaColor = new XnaColor(gdiColor.R, gdiColor.G, gdiColor.B);

            // Update the spinning triangle control with the new color.
            isoHexControl.Vertices[vertexIndex].Color = xnaColor;            
        }

        void EditorWindowLocation_Changed(object sender, System.EventArgs e)
        {
            isoHexControl.WindowLocation.X = this.Location.X;
            isoHexControl.WindowLocation.Y = this.Location.Y;

            Console.WriteLine(isoHexControl.Location.X + " " + isoHexControl.Location.Y);

        }

        /// <summary>
        /// Event that handles when the draw wireframe checkbox is toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chkDrawWireFrame_Pressed(object sender, System.EventArgs e)
        {
            isoHexControl.IsDrawingWireFrame = chkDrawWireframe.Checked;
        }


        /// <summary>
        /// Event that handles when the draw model checkbox is toggled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chkDrawModel_Pressed(object sender, System.EventArgs e)
        {
            isoHexControl.IsDrawingModels = chkDrawModel.Checked;
        }
    }
}
