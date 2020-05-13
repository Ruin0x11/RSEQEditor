using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSEQEditor
{
    public static class Params
    {
        public const int MIN_KEY_WIDTH = 68;
        public const int MAX_KEY_WIDTH = MIN_KEY_WIDTH * 5;

        public const int FONT_SIZE8 = 8;

        public static int keyWidth = MIN_KEY_WIDTH * 2;

        public static Font baseFont8 = new Font("Dialog", 8, FontStyle.Regular);
        public static Font baseFont10Bold = new Font("Dialog", 10, FontStyle.Bold);

        public static int baseFont8OffsetHeight = 0;

        public static float ScaleX = 1.0f;
        public static float ScaleY = 0.2f;

        // editor

        public static Config config = new Config();
    }
}
