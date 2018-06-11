using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        String filename;
        PrintDialog pd;
        PrintDocument pdoc;
        string[] lines;
        int linesPrinted;
        public Form1()
        {
            InitializeComponent();
            FontSizeUpDown.Maximum = 100;
            FontSizeUpDown.Minimum = 5;
            FontSizeUpDown.Value = 12;
            mainTextBox.SelectionChanged += selectionChanged;
        }
        private void selectionChanged(object sender, EventArgs e)
        {
            var font = mainTextBox.SelectionFont;
            FontSizeUpDown.Value = (decimal)font.Size;
            if (font.Bold)
                boldBtn.Font = new Font(boldBtn.Font.FontFamily, boldBtn.Font.Size, font.Style);
            if (font.Italic)
                italicBtn.Font = new Font(italicBtn.Font.FontFamily, italicBtn.Font.Size, font.Style);
            if (font.Underline)
                underlineBtn.Font = new Font(underlineBtn.Font.FontFamily, underlineBtn.Font.Size, font.Style);

        }
        private void создатьНовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fod = new SaveFileDialog();
                fod.Title = "Создание нового файла";
                fod.DefaultExt = "*.rtf";
                if (fod.ShowDialog() != DialogResult.Cancel)
                {
                    filename = fod.FileName;
                    File.Create(filename);
                    mainTextBox.Clear();
                    mainTextBox.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var fod = new OpenFileDialog();
                //fod.DefaultExt = "*.rtf";
                if (fod.ShowDialog() != DialogResult.Cancel) {
                    filename = fod.FileName;
                    mainTextBox.LoadFile(filename);
                    mainTextBox.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (filename != null)
                    mainTextBox.SaveFile(filename);
                else
                {
                    var sfd = new SaveFileDialog();
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        filename = sfd.FileName;
                        mainTextBox.SaveFile(filename);
                    }
                }
            } 
             catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Font currentFont = mainTextBox.SelectionFont;
            FontStyle style;
            if (!currentFont.Bold)
                style = FontStyle.Bold;
            else
                style = FontStyle.Regular;
            mainTextBox.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, style);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Font currentFont = mainTextBox.SelectionFont;
            FontStyle style;
            if (!currentFont.Italic)
                style = FontStyle.Italic;
            else
                style = FontStyle.Regular;
            mainTextBox.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, style);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Font currentFont = mainTextBox.SelectionFont;
            FontStyle style;
            if (!currentFont.Underline)
                style = FontStyle.Underline;
            else
                style = FontStyle.Regular;
            mainTextBox.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, style);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog()!=DialogResult.Cancel)
            {
                mainTextBox.SelectionColor = cd.Color;
            }
            
        }

        private void FontSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            float size = (float)this.FontSizeUpDown.Value;
            var currentFont = mainTextBox.SelectionFont;
            mainTextBox.SelectionFont = new Font(currentFont.FontFamily, size, currentFont.Style);
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pd = new PrintDialog();
            pdoc = new PrintDocument();
            if (pd.ShowDialog() == DialogResult.OK)
            {
                pdoc.Print();
            }

        }

        // OnBeginPrint 
        private void OnBeginPrint(object sender,
                          System.Drawing.Printing.PrintEventArgs e)
        {
            char[] param = { '\n' };

            if (pd.PrinterSettings.PrintRange == PrintRange.Selection)
            {
                lines = mainTextBox.SelectedText.Split(param);
            }
            else
            {
                lines = mainTextBox.Text.Split(param);
            }

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                lines[i++] = s.TrimEnd(trimParam);
            }
        }
        // OnPrintPage
        private void OnPrintPage(object sender,
                                   System.Drawing.Printing.PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            Brush brush = new SolidBrush(mainTextBox.ForeColor);

            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++],
                     mainTextBox.Font, brush, x, y);
                y += 15;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
                else
                    e.HasMorePages = false;
            }
        }
    }
}
