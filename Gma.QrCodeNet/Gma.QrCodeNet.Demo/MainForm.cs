﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Gma.QrCodeNet.Encoding;

namespace Gma.QrCodeNet.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            qrCodeControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            qrCodeControl1.Text = textBoxInput.Text;
            qrCodeImgControl1.Text = textBoxInput.Text;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
        	
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|Encapsuled PostScript (*.eps)|*.eps";
            saveFileDialog.FileName = Path.GetFileName(GetFileNameProposal());
            saveFileDialog.DefaultExt = "png";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

			if (saveFileDialog.FileName.EndsWith("eps"))
			{
				// Generate the matrix from scratch as it is not reachable from the qrCodeControl1
				var encoder = new QrEncoder(qrCodeControl1.ErrorCorrectionLevel);
				var qrCode = encoder.Encode(textBoxInput.Text);

				// Initialize the EPS renderer
				var renderer = new Gma.QrCodeNet.Encoding.Windows.Render.EncapsulatedPostScriptRenderer(
					qrCodeControl1.DarkBrush,
					qrCodeControl1.LightBrush,
					Gma.QrCodeNet.Encoding.Windows.Render.QuietZoneModules.Two); // No easy way to convert Control.QuietZoneModule to Render.QuietZoneModule

				using (var file = File.CreateText(saveFileDialog.FileName))
				{
					renderer.WriteToStream(qrCode.Matrix, 6, file); // 72/6 = 12 modules per inch
				}
			}
			else
			{
				using (Bitmap bitmap = new Bitmap(qrCodeControl1.Size.Width, qrCodeControl1.Size.Height))
				{
					qrCodeControl1.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), bitmap.Size));
					bitmap.Save(
						saveFileDialog.FileName,
						saveFileDialog.FileName.EndsWith("png")
							? ImageFormat.Png
							: ImageFormat.Bmp);
				}
			}

        }

        private string GetFileNameProposal()
        {
            return textBoxInput.Text.Length > 10 ? textBoxInput.Text.Substring(0, 10) : textBoxInput.Text;
        }

        private void checkBoxArtistic_CheckedChanged(object sender, EventArgs e)
        {
            qrCodeControl1.Artistic = checkBoxArtistic.Checked;
        }
    }
}
