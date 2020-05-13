using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSEQEditor
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance { get { return _instance == null ? _instance = new MainForm() : _instance; } }

        private RSEQNode _rseq;
        public RSEQNode Rseq { get { return _rseq;  } }

        private int _key_width = 40;

        public int std_x = 0;
        public int std_y = 0;

        private BindingList<MMLCommand> myList;

        public MainForm()
        {
            InitializeComponent();

            myList = new BindingList<MMLCommand>();

            panel1.SuspendLayout();
            pianoRoll1.SuspendLayout();
            vScrollBar1.SuspendLayout();

            positionIndicator1.Left = 0;
            positionIndicator1.Top = 0;
            positionIndicator1.Width = panel1.Width;

            pianoRoll1.Bounds = new System.Drawing.Rectangle(0, positionIndicator1.Height, panel1.Width - vScrollBar1.Width, panel1.Height - positionIndicator1.Height - hScrollBar1.Height);

            vScrollBar1.Left = pianoRoll1.Left;
            vScrollBar1.Top = pianoRoll1.Top;
            vScrollBar1.Height = pianoRoll1.Height; 
            
            hScrollBar1.Left = pianoRoll1.Left;
            hScrollBar1.Top = pianoRoll1.Top + pianoRoll1.Height + 4;
            hScrollBar1.Width = pianoRoll1.Width;

            panel1.ResumeLayout();
            pianoRoll1.ResumeLayout();
            vScrollBar1.ResumeLayout();

            hScrollBar1.Maximum = 240;
            hScrollBar1.LargeChange = 240 * 4;

            vScrollBar1.Maximum = (int)(Params.ScaleY * 100 * 128);
            vScrollBar1.LargeChange = 24 * 4;
            hScrollBar1.SmallChange = 240;
            vScrollBar1.SmallChange = 24;

            updateScrollRangeHorizontal();
            updateScrollRangeVertical();

            int draft_start_to_draw_y = 68 * (int)(100 * Params.ScaleY) - pianoRoll1.Height / 2;
            int draft_vscroll_value = (int)((draft_start_to_draw_y * (double)vScrollBar1.Maximum) / (128 * (int)(100 * Params.ScaleY) - vScrollBar1.Height));
            vScrollBar1.Value = draft_vscroll_value;
            std_y = calculateStartToDrawY(vScrollBar1.Value);
            
            this.Bounds = Params.config.WindowRect;
            Rectangle rc2 = Screen.GetWorkingArea(this);
            if (this.Bounds.X < rc2.X || rc2.X + rc2.Width  < this.Bounds.X + this.Bounds.Width ||
                this.Bounds.Y < rc2.Y || rc2.Y + rc2.Height < this.Bounds.Y + this.Bounds.Height)
            {
                this.Bounds = new Rectangle(rc2.X, rc2.Y, this.Bounds.Width, this.Bounds.Height);
                Params.config.WindowRect = this.Bounds;
            }

            _instance = this;

            //Reset();
            Program.Open("D:\\big\\wiimusic\\raw\\files\\Sound\\MusicStatic\\rp_Music_sound.brsar");
        }

        public void MainForm_LocationChanged(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Params.config.WindowRect = this.Bounds;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private HashSet<string> cols = new HashSet<string>
            {
                "Value1",
                "Value2",
                "Value3",
                "Value4",
                "Value5",
            };

        private static string GetDescription<T>(T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            var col = dgv.Columns[e.ColumnIndex].Name;
            if (cols.Contains(col))
            {
                var cell = dgv[col, e.RowIndex];

                var val = (uint?)cell.Value;
                var enabled = val.HasValue;
                cell.ReadOnly = !enabled;
                if (enabled)
                {
                    cell.Style.BackColor = cell.OwningColumn.DefaultCellStyle.BackColor;
                    cell.Style.ForeColor = cell.OwningColumn.DefaultCellStyle.ForeColor;
                }
                else
                {
                    cell.Style.BackColor = Color.LightGray;
                    cell.Style.ForeColor = Color.DarkGray;
                }
            }

            if (dgv.Columns[e.ColumnIndex].Name == "Type" && e.RowIndex >= 0)
            {
                var cmd = (Mml)dgv[e.ColumnIndex, e.RowIndex].Value;
                var desc = GetDescription<Mml>(cmd);
                e.Value = $"0x{(uint)cmd:x2} - {desc}";
                e.FormattingApplied = true;
            }
            //e.Value = "dood";
            //e.FormattingApplied = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "BRSAR files (*.brsar)|*.brsar",
                Multiselect = false
            };
            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileName.Trim().Length > 0)
            {
                Program.Open(ofd.FileName);
            }
        }

        private int calculateStartToDrawX()
        {
            return (int)(hScrollBar1.Value * Params.ScaleX);
        }

        private int calculateStartToDrawY(int vscroll_value)
        {
            int min = vScrollBar1.Minimum;
            int max = vScrollBar1.Maximum - vScrollBar1.LargeChange;
            int value = vscroll_value;
            if (value < min)
            {
                value = min;
            }
            else if (max < value)
            {
                value = max;
            }
            return (int)(value * Params.ScaleY);
        }

        public void updateScrollRangeHorizontal()
        {
            int pwidth = pianoRoll1.Width;
            int hwidth = hScrollBar1.Width;
            if (pwidth <= 0 || hwidth <= 0)
            {
                return;
            }

            if (_rseq == null) return;
            int l = (int)_rseq.CalculateLength();
            float scalex = Params.ScaleX;
            int key_width = _key_width;
            int pict_piano_roll_width = pwidth - key_width;
            int large_change = (int)(pict_piano_roll_width / scalex);
            int maximum = (int)(l + large_change);

            int thumb_width = System.Windows.Forms.SystemInformation.HorizontalScrollBarThumbWidth;
            int box_width = (int)(large_change / (float)maximum * (hwidth - 2 * thumb_width));
            if (box_width < 20)
            {
                box_width = 20;
                if (hwidth - 2 * thumb_width > box_width)
                {
                    maximum = l * (hwidth - 2 * thumb_width) / (hwidth - 2 * thumb_width - box_width);
                    large_change = l * box_width / (hwidth - 2 * thumb_width - box_width);
                }
            }

            if (large_change <= 0) large_change = 1;
            if (maximum <= 0) maximum = 1;
            hScrollBar1.LargeChange = large_change;
            hScrollBar1.Maximum = maximum;

            int old_value = hScrollBar1.Value;
            if (old_value > maximum - large_change)
            {
                hScrollBar1.Value = maximum - large_change;
            }
        }

        public void updateScrollRangeVertical()
        {
            // コンポーネントの高さが0の場合，スクロールの設定が出来ないので．
            int pheight = pianoRoll1.Height;
            int vheight = vScrollBar1.Height;
            if (pheight <= 0 || vheight <= 0)
            {
                return;
            }

            float scaley = Params.ScaleY;

            int maximum = (int)(128 * (int)(100 * scaley) / scaley);
            int large_change = (int)(pheight / scaley);

            int thumb_height = System.Windows.Forms.SystemInformation.VerticalScrollBarThumbHeight;
            int box_height = (int)(large_change / (float)maximum * (vheight - 2 * thumb_height));
            if (box_height < 20)
            {
                box_height = 20;
                maximum = (int)(((128.0 * (int)(100 * scaley) - pheight) / scaley) * (vheight - 2 * thumb_height) / (vheight - 2 * thumb_height - box_height));
                large_change = (int)(((128.0 * (int)(100 * scaley) - pheight) / scaley) * box_height / (vheight - 2 * thumb_height - box_height));
            }

            if (large_change <= 0) large_change = 1;
            if (maximum <= 0) maximum = 1;
            vScrollBar1.LargeChange = large_change;
            vScrollBar1.Maximum = maximum;
            vScrollBar1.SmallChange = 100;

            int new_value = maximum - large_change;
            if (new_value < vScrollBar1.Minimum)
            {
                new_value = vScrollBar1.Minimum;
            }
            if (vScrollBar1.Value > new_value)
            {
                vScrollBar1.Value = new_value;
            }
        }

        public void vScrollBar1_Enter(Object sender, EventArgs e)
        {
            pianoRoll1.Focus();
        }

        public void vScrollBar1_ValueChanged(Object sender, EventArgs e)
        {
            std_y = calculateStartToDrawY(vScrollBar1.Value);
            Console.WriteLine(std_y);
            pianoRoll1.Refresh();
        }

        public void vScrollBar1_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                updateScrollRangeVertical();
                std_y = calculateStartToDrawY(vScrollBar1.Value);
            }
        }

        public void hScrollBar1_Enter(Object sender, EventArgs e)
        {
            pianoRoll1.Focus();
        }

        public void hScrollBar1_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                updateScrollRangeHorizontal();
            }
        }

        public void RefreshScreen()
        {
            pianoRoll1.Refresh();
            positionIndicator1.Refresh();
        }

        public void hScrollBar1_ValueChanged(Object sender, EventArgs e)
        {
            std_x = calculateStartToDrawX();
            RefreshScreen();
        }

        public void Reset()
        {
            myList.Clear();

            if (Program.RootNode != null)
            {
                RSARSoundNode node = (RSARSoundNode)Program.RootNode.FindChild("RP/SSN/HAPPY/BIRTHDAY/SCORE", true);
                RSEQNode rseq = (RSEQNode)node._soundFileNode;
                _rseq = rseq;
                pianoRoll1.SetNode(rseq);

                foreach (var cmd in rseq.Commands)
                {
                    myList.Add(cmd);
                }
            }
            else
            {
                pianoRoll1.SetNode(null);
            }
        }

        private void pianoRoll1_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshScreen();
        }
    }
}
