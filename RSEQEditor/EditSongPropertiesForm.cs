using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RSEQEditor.WiiMusic;

namespace RSEQEditor
{
    public partial class EditSongPropertiesForm : Form
    {
        public WiiMusicSong TargetSong;

        private ResourceNode _rootNode;
        private BMGNode _messagesNode;

        public EditSongPropertiesForm()
        {
            InitializeComponent();

            _rootNode = NodeFactory.FromFile(null, "D:\\big\\wiimusic\\raw\\files\\US\\Message\\message.carc");
            _messagesNode = _rootNode.Children[0] as BMGNode;
            _messagesNode.Populate();
        }

        public DialogResult ShowDialog(IWin32Window owner, WiiMusicSong song)
        {
            TargetSong = song;
            nameBox.Text = _messagesNode.GetMessageByMID(TargetSong.MsgIdName).Message;
            genreBox.Text = _messagesNode.GetMessageByMID(TargetSong.MsgIdGenre).Message;
            descriptionBox.Text = _messagesNode.GetMessageByMID(TargetSong.MsgIdDescription).Message;
            return base.ShowDialog();
        }

        private unsafe void btnOkay_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _messagesNode.GetMessageByMID(TargetSong.MsgIdName).Message = nameBox.Text;
            _messagesNode.GetMessageByMID(TargetSong.MsgIdGenre).Message = genreBox.Text;
            _messagesNode.GetMessageByMID(TargetSong.MsgIdDescription).Message = descriptionBox.Text;
            _rootNode.Export("D:\\big\\wiimusic\\raw\\files\\US\\Message\\message222.carc");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) 
        { 
            DialogResult = DialogResult.Cancel; 
            Close(); 
        }
    }
}
