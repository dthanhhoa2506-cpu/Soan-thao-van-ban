using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bttuan5
{
    public partial class Form1 : Form
    {

        string currentFile = "";
        bool isNewFile = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Load Fonts hệ thống
            foreach (FontFamily font in new InstalledFontCollection().Families)
            {
                cmbsize.Items.Add(font.Name);
            }

            // Load Size
            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int s in sizes)
                cmbFonts.Items.Add(s);

            // Giá trị mặc định
            cmbsize.Text = "Tahoma";
            cmbFonts.Text = "14";
            richText.Font = new Font("Tahoma", 14);
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {

            richText.Clear();
            cmbFonts.Text = "Tahoma";
            cmbsize.Text = "14";
            richText.Font = new Font("Tahoma", 14);
            currentFile = "";
            isNewFile = true;
        }


        // ================= OPEN DOCUMENT =================
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text file (*.txt)|*.txt|Rich Text (*.rtf)|*.rtf";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                currentFile = ofd.FileName;

                if (currentFile.EndsWith(".rtf"))
                    richText.LoadFile(currentFile);
                else
                    richText.LoadFile(currentFile, RichTextBoxStreamType.PlainText);

                isNewFile = false;
            }
        }

        // ================= SAVE DOCUMENT =================
        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveDocument();
        }

        private void SaveDocument()
        {
            if (isNewFile)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Rich Text (*.rtf)|*.rtf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    currentFile = sfd.FileName;
                    richText.SaveFile(currentFile);
                    isNewFile = false;
                }
            }
            else
            {
                richText.SaveFile(currentFile);
                MessageBox.Show("Lưu văn bản thành công!");
            }
        }

        // ================= EXIT =================
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ================= FONT DIALOG =================
        private void mnuFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;

            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richText.ForeColor = fontDlg.Color;
                richText.Font = fontDlg.Font;
            }
        }

        // ================= TOOLSTRIP FONT =================
        private void cmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont == null) return;

            richText.SelectionFont = new Font(
                cmbFonts.Text,
                richText.SelectionFont.Size,
                richText.SelectionFont.Style);
        }

        private void cmbSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (richText.SelectionFont == null) return;

            richText.SelectionFont = new Font(
                richText.SelectionFont.FontFamily,
                float.Parse(cmbsize.Text),
                richText.SelectionFont.Style);
        }

        // ================= BOLD / ITALIC / UNDERLINE =================
        private void ToggleStyle(FontStyle style)
        {
            if (richText.SelectionFont == null) return;

            FontStyle newStyle;

            if (richText.SelectionFont.Style.HasFlag(style))
                newStyle = richText.SelectionFont.Style & ~style;
            else
                newStyle = richText.SelectionFont.Style | style;

            richText.SelectionFont = new Font(
                richText.SelectionFont.FontFamily,
                richText.SelectionFont.Size,
                newStyle);
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Bold);
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Italic);
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            ToggleStyle(FontStyle.Underline);
        }

        // ================= WORD COUNT =================
        private void richText_TextChanged(object sender, EventArgs e)
        {
            string text = richText.Text.Trim();
            int count = string.IsNullOrEmpty(text)
                ? 0
                : text.Split(new char[] { ' ', '\n', '\t' },
                  StringSplitOptions.RemoveEmptyEntries).Length;

            lblWordCount.Text = "Tổng số từ: " + count;
        }
    }
}
