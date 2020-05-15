using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSEQEditor
{
    public partial class EventListForm : Form
    {
        private BindingList<MMLCommand> myList;

        public EventListForm()
        {
            InitializeComponent();

            myList = new BindingList<MMLCommand>();

            Type.DataSource = Enum.GetValues(typeof(Mml));
        }

        public void Reset()
        {
            myList.Clear();

            if (MainForm.Instance.Rseq != null)
            {
                foreach (var cmd in MainForm.Instance.Rseq.Song.Tracks[0])
                {
                    myList.Add(cmd);
                }
            }

            dataGridView1.DataSource = myList;
        }
    }
}
