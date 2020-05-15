using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RSEQEditor
{
    class PianoRoll : PictureBox
    {
        internal class NoteDrawable
        {
            public int note;
            public int tick;
            public int length;
            public Color color;
            public string text;

            public void Draw(Graphics g, int x, int y, SolidBrush b, Pen p)
            {
                var gap = 1;
                var _x = tick * Params.ScaleX - x;
                var _y = (int)((127 - note) * 100 * Params.ScaleY) - y;
                var _w = (int)(length * Params.ScaleX);
                var _h = (int)(100 * Params.ScaleY);
                b.Color = ControlPaint.Dark(color, 0.25f);
                g.FillRectangle(b, _x, _y, _w, _h);
                b.Color = color;
                g.FillRectangle(b, _x + gap + 1, _y, _w - gap - 1, _h - gap);
                p.Color = Color.DarkGray;
                g.DrawRectangle(p, _x, _y, _w, _h);
                b.Color = Color.Black;
                g.DrawString(text, Params.baseFont8, b, _x, _y);
            }
        }

        internal class LabelDrawable
        {
            public int tick;
            public Color color;
            public string text;

            public void Draw(Graphics g, int x, int y, SolidBrush b, Pen p)
            {
                var _x = tick * Params.ScaleX - x;
                var _y = 0;
                var _h = g.VisibleClipBounds.Height;
                b.Color = color;
                g.FillRectangle(b, _x, _y, 50, 20);
                g.DrawString(text, Params.baseFont8, b, _x + 55, _y);
                p.Color = color;
                g.DrawLine(p, _x, _y, _x, _h);
            }
        }

        private RSEQNode _node;
        private int _track;
        private List<NoteDrawable> _drawObjects;
        private List<LabelDrawable> _labelObjects;
        private Dictionary<int, int> _offsetToTick;
        public SolidBrush _brush = new SolidBrush(Color.DarkGray);
        public SolidBrush _noteBrush = new SolidBrush(Color.AliceBlue);
        public Pen _pen = new Pen(Color.Black);

        public PianoRoll()
        {
            _drawObjects = new List<NoteDrawable>();
            _labelObjects = new List<LabelDrawable>();
            _offsetToTick = new Dictionary<int, int>();
        }

        public void SetNode(RSEQNode node)
        {
            _node = node;
            _track = 0;

            UpdateDrawObjects();
        }

        public void SetTrack(int track)
        {
            if (_node == null || !_node.Song.Tracks.ContainsKey(track))
                return;

            _track = track;

            UpdateDrawObjects();
        }

        private void UpdateDrawObjects()
        {
            _offsetToTick.Clear();

            int tick = 0;
            foreach (MMLCommand cmd in _node.Song.Tracks[_track])
            {
                _offsetToTick.Add(cmd._offset, tick);

                int? len;
                if ((len = cmd.GetLength()).HasValue)
                {
                    tick += len.Value;
                }
            }

            _drawObjects.Clear();

            tick = 0;
            foreach (MMLCommand cmd in _node.Song.Tracks[_track])
            {
                int? len;
                if ((len = cmd.GetLength()).HasValue)
                {
                    var note = (int)cmd._value1.Value;
                    var color = Color.AliceBlue;
                    var text = MidiUtil.GetNoteString(note);
                    if (cmd._cmd == Mml.MML_WAIT)
                    {
                        note = 67;
                        color = Color.DarkGray;
                        text = "Rest";
                    }

                    NoteDrawable obj = new NoteDrawable
                    {
                        note = note,
                        tick = tick,
                        length = len.Value,
                        color = color,
                        text = text
                    };
                    _drawObjects.Add(obj);
                    tick += len.Value;
                }
            }

            _labelObjects.Clear();

            foreach (RSEQLabelNode label in _node.Children)
            {
                int labelTick;
                if (_offsetToTick.TryGetValue((int)label.Id, out labelTick))
                {
                    LabelDrawable obj = new LabelDrawable
                    {
                        tick = labelTick,
                        text = label.Name,
                        color = Color.Blue
                    };
                    _labelObjects.Add(obj);
                }
            }

            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (_node == null)
            {
                return;
            }

            int track_height = (int)(Params.ScaleY * 100);
            int half_track_height = track_height / 2;

            var mouse_position = PointToClient(System.Windows.Forms.Control.MousePosition);

            var stdx = MainForm.Instance.std_x;
            var stdy = MainForm.Instance.std_y;
            int key_width = Params.keyWidth;

            var brush = new SolidBrush(Color.White);
            var pen = new Pen(Color.White);

            int odd = -1;
            int y = 128 * track_height - stdy;
            int dy = -track_height;
            for (int i = 0; i <= 127; i++)
            {
                odd++;
                if (odd == 12)
                {
                    odd = 0;
                }
                int order = (i - odd) / 12 - 2;
                y += dy;
                if (y > Height)
                {
                    continue;
                }
                else if (0 > y + track_height)
                {
                    break;
                }
                bool note_is_whitekey = MidiUtil.IsNoteWhiteKey(i);

                Color b = Color.Black;
                Color border;
                bool paint_required = true;
                if (order == -2 || order == -1 || (6 <= order && order <= 8))
                {
                    if (note_is_whitekey)
                    {
                        b = Color.FromArgb(180, 180, 180);
                    }
                    else
                    {
                        b = Color.FromArgb(106, 108, 108);
                    }
                    border = Color.FromArgb(106, 108, 108);
                }
                else if (order == 5 || order == 0)
                {
                    if (note_is_whitekey)
                    {
                        b = Color.FromArgb(212, 212, 212);
                    }
                    else
                    {
                        b = Color.FromArgb(180, 180, 180);
                    }
                    border = Color.FromArgb(150, 152, 150);
                }
                else
                {
                    if (note_is_whitekey)
                    {
                        b = Color.FromArgb(240, 240, 240);
                    }
                    else
                    {
                        b = Color.FromArgb(212, 212, 212);
                    }
                    border = Color.FromArgb(210, 205, 172);
                }
                if (paint_required)
                {
                    brush.Color = b;
                    pe.Graphics.FillRectangle(brush, key_width, y, Width - key_width, track_height + 1);
                }
                if (odd == 0 || odd == 5)
                {
                    pen.Color = border;
                    pe.Graphics.DrawLine(pen, key_width, y + track_height, Width, y + track_height);
                }
            }

            int hilighted_note = -1;
            pen.Color = Color.FromArgb(212, 212, 212);
            pe.Graphics.DrawLine(pen, key_width, 0, key_width, Height);
            int odd2 = -1;
            y = 128 * track_height - stdy;
            dy = -track_height;
            for (int i = 0; i <= 127; i++)
            {
                odd2++;
                if (odd2 == 12)
                {
                    odd2 = 0;
                }
                y += dy;
                if (y > Height)
                {
                    continue;
                }
                else if (y + track_height < 0)
                {
                    break;
                }

                pen.Color = Color.FromArgb(212, 212, 212);
                pe.Graphics.DrawLine(pen, 0, y, key_width, y);
                bool hilighted = false;
                //if (edit_mode == EditMode.ADD_ENTRY)
                //{
                //    if (AppManager.mAddingEvent.ID.Note == i)
                //    {
                //        hilighted = true;
                //        hilighted_note = i;
                //    }
                //}
                //else if (edit_mode == EditMode.EDIT_LEFT_EDGE || edit_mode == EditMode.EDIT_RIGHT_EDGE)
                //{
                //    if (AppManager.itemSelection.getLastEvent().original.ID.Note == i)
                //    { //TODO: ここでNullpointer exception
                //        hilighted = true;
                //        hilighted_note = i;
                //    }
                //}
                //else
                //{
                if (3 <= mouse_position.X && mouse_position.X <= Width - 17 &&
                    0 <= mouse_position.Y && mouse_position.Y <= Height - 1)
                {
                    if (y <= mouse_position.Y && mouse_position.Y < y + track_height)
                    {
                        hilighted = true;
                        hilighted_note = i;
                    }
                }
                //}
                if (hilighted)
                {
                    brush.Color = Color.Yellow;
                    pe.Graphics.FillRectangle(brush, 35, y, key_width - 35, track_height);
                }
                if (odd2 == 0 || hilighted)
                {
                    brush.Color = Color.FromArgb(72, 77, 98);
                    pe.Graphics.DrawString(MidiUtil.GetNoteString(i), Params.baseFont8, brush, 42, y + half_track_height - Params.baseFont8OffsetHeight + 1);
                }
                if (!MidiUtil.IsNoteWhiteKey(i))
                {
                    brush.Color = Color.FromArgb(125, 123, 124);
                    pe.Graphics.FillRectangle(brush, 0, y, 34, track_height);
                }
            }

            foreach (NoteDrawable obj in _drawObjects)
            {
                obj.Draw(pe.Graphics, stdx + key_width, stdy, _brush, _pen);
            }
            foreach (LabelDrawable obj in _labelObjects)
            {
                obj.Draw(pe.Graphics, stdx + key_width, stdy, _brush, _pen);
            }

            if (hilighted_note >= 0)
            {
                _brush.Color = Color.Black;
                pe.Graphics.DrawString(MidiUtil.GetNoteString(hilighted_note),
                                       Params.baseFont10Bold,
                                       _brush,
                                       new Rectangle(mouse_position.X - 110, mouse_position.Y - 10, 100, 100),
                                       new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near });
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
    }
}
