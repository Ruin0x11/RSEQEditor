using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSEQEditor
{
    public static class WiiMusic
    {
        public class WiiMusicSong
        {
            public string DisplayName;
            public string BrsarRootPath;

            public int MsgIdName;
            public int MsgIdGenre;
            public int MsgIdDescription;

            public WiiMusicSong(string displayName, string brsarRootPath, int msgIdName, int msgIdGenre, int msgIdDescription)
            {
                DisplayName = displayName;
                BrsarRootPath = brsarRootPath;
                MsgIdName = msgIdName;
                MsgIdGenre = msgIdGenre;
                MsgIdDescription = msgIdDescription;
            }
        }

        // XXX: Most of the message IDs are just guesses.
        public static readonly List<WiiMusicSong> SONGS = new List<WiiMusicSong>
        {
            new WiiMusicSong("Ode To Joy", "RP/SSN/YOROKOBI/NO/UTA", 51200, 76800, 102400),
            new WiiMusicSong("Bridal Chorus", "RP/SSN/BRIDAL/CHORUS", 51456, 77056, 102656),
            new WiiMusicSong("Swan Lake", "RP/SSN/SWAN/LAKE", 51712, 77312, 102912),
            new WiiMusicSong("Carmen", "RP/SSN/CARMEN", 51968, 77568, 103168),
            new WiiMusicSong("Wii Music", "RP/SSN/WII/MUSIC", 52224, 77824, 103424),
            new WiiMusicSong("Wii Sports", "RP/SSN/WII/SPORTS", 63232, 83456, 103680),
            new WiiMusicSong("The Blue Danube", "RP/SSN/DONAU", 52480, 78080, 103680),
            new WiiMusicSong("A Little Night Music", "RP/SSN/EINE/KLINE", 52736, 78336, 103936),
            new WiiMusicSong("Minuet In G Major", "RP/SSN/MINUETT", 52992, 78592, 104192),
            new WiiMusicSong("Happy Birthday To You", "RP/SSN/HAPPY/BIRTHDAY", 53248, 78848, 104448),
            new WiiMusicSong("Do-Re-Mi", "RP/SSN/DO/RE/MI", 53504, 79104, 104704),
            new WiiMusicSong("The Entertainer", "RP/SSN/ENTERTAINER", 53760, 79360, 104960),
            new WiiMusicSong("American Patrol", "RP/SSN/AMERICAN/PATROL", 54016, 79616, 105216),
            new WiiMusicSong("Turkey in the Straw", "RP/SSN/OKLAHOMA/MIXER", 54272, 79872, 105472),
            new WiiMusicSong("Yankee Doodle", "RP/SSN/ALPUS/ICHIMAN/JAKU", 54528, 80128, 105728),
            new WiiMusicSong("Oh, My Darling Clementine", "RP/SSN/CLEMENTINE", 54784, 80384, 105984),
            new WiiMusicSong("My Grandfather's Clock", "RP/SSN/GRANDFATHERS/CLOCK", 55040, 80640, 106240),
            new WiiMusicSong("From the New World", "RP/SSN/NEW/WORLD", 55296, 80896, 106496),
            new WiiMusicSong("La Bamba", "RP/SSN/LA/BAMBA", 55552, 81152, 106752),
            new WiiMusicSong("Scarborough Fair", "RP/SSN/SCABOROU/FAIR", 55808, 81408, 107008),
            new WiiMusicSong("Long, Long Ago", "RP/SSN/LONG/LONG/AGO", 56064, 0, 107264),
            new WiiMusicSong("Twinkle Twinkle Little Star", "RP/SSN/LITTLE/STAR", 56320, 81920, 107520),
            new WiiMusicSong("Sur le pont d'Avignon", "RP/SSN/AVIGNON", 56576, 82176, 107776),
            new WiiMusicSong("Frere Jacques", "RP/SSN/ARE/YOU/SLEEPING", 56832, 82432, 108032),
            new WiiMusicSong("The Flea Waltz", "RP/SSN/NEKO/FUN/JATTA", 57088, 82688, 108288),
            new WiiMusicSong("O Christmas Tree", "RP/SSN/MOMINOKI", 57344, 82944, 108544),
            new WiiMusicSong("Little Hans", "RP/SSN/CHOOUCHO", 57600, 83200, 108800),
            new WiiMusicSong("Animal Crossing -- K.K Blues", "RP/SSN/KEKE/BLUES", 57856, 83456, 109056),
            new WiiMusicSong("From Santurtzi to Bilbao", "RP/SSN/DESDE/SANTURCE", 58112, 83712, 109312),
            new WiiMusicSong("Troika", "RP/SSN/TROIKA", 58368, 83968, 109568),
            new WiiMusicSong("La Cucaracha", "RP/SSN/LA/CUCARACHA", 58624, 84224, 109824),
            new WiiMusicSong("Over the Waves", "RP/SSN/OVER/THE/WAVES", 58880, 84480, 110080),
            new WiiMusicSong("Sakura Sakura", "RP/SSN/SAKURA/SAKURA", 59136, 84736, 110336),
            new WiiMusicSong("Sukiyaki", "RP/SSN/SUKIYAKI", 59392, 84992, 110592),
            new WiiMusicSong("Daydream Believer", "RP/SSN/DAYDREAM/BELIEVER", 59648, 85248, 110848),
            new WiiMusicSong("Every Breath You Take", "RP/SSN/EVERY/BREATH", 59904, 85504, 111104),
            new WiiMusicSong("Chariots Of Fire", "RP/SSN/CHARIOTS/OF/FIRE", 60160, 85760, 111360),
            new WiiMusicSong("September", "RP/SSN/SEPTEMBER", 60416, 86016, 111616),
            new WiiMusicSong("Please Mr. Postman", "RP/SSN/PLEASE/MR/POSTMAN", 60672, 86272, 111872),
            new WiiMusicSong("Material Girl", "RP/SSN/MATERIAL/GIRL", 60928, 86528, 112128),
            new WiiMusicSong("The Loco-Motion", "RP/SSN/LOCO/MOTION", 61184, 86784, 112384),
            new WiiMusicSong("I'll Be There", "RP/SSN/ILL/BE/THERE", 61440, 87040, 112640),
            new WiiMusicSong("Jingle Bell Rock", "RP/SSN/JINGLE/BELL/ROCK", 61696, 87296, 112896),
            new WiiMusicSong("Wake Me Up Before You Go-Go", "RP/SSN/WAKE/ME/UP", 61952, 87552, 113152),
            new WiiMusicSong("Woman", "RP/SSN/WOMAN", 62208, 87808, 113408),
            new WiiMusicSong("I've Never Been to Me", "RP/SSN/NEVER/BEEN/TO/ME", 62464, 88064, 113664),
            new WiiMusicSong("Super Mario Bros.", "RP/SSN/SUPER/MARIO", 62720, 88320, 113920),
            new WiiMusicSong("The Legend of Zelda", "RP/SSN/ZELDA", 62976, 88576, 114176),
            new WiiMusicSong("Animal Crossing", "RP/SSN/ANIMAL/CROSSING", 63488, 88832, 114688),
            new WiiMusicSong("F-Zero -- Mute City Theme", "RP/SSN/F/ZERO", 63744, 89088, 114944),
        };
    }
}

