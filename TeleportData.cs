using UnityEngine;

namespace QoLTeleportKit
{
    public class TeleportData
    {
        public readonly (int id, string name, Vector3 position, string scene)[] Bosses =
        {
            // Page 1: Hall of Gods [1 Floor] (1-18)
            (1, "Gruz Mother", new Vector3(28.03279f, 6.40824f, 0f), "GG_Workshop"),
            (2, "Vengefly King", new Vector3(36.43205f, 6.408124f, 0f), "GG_Workshop"),
            (3, "Brooding Mawlek", new Vector3(43.84856f, 6.408124f, 0f), "GG_Workshop"),
            (4, "False Knight/Failed Champion", new Vector3(52.05054f, 6.408124f, 0f), "GG_Workshop"),
            (5, "Hornet Protector/Hornet Sentinel", new Vector3(60.69859f, 6.408124f, 0f), "GG_Workshop"),
            (6, "Massive Moss Charger", new Vector3(71.1746f, 6.408124f, 0f), "GG_Workshop"),
            (7, "Flukemarm", new Vector3(81.26862f, 6.408124f, 0f), "GG_Workshop"),
            (8, "Sisters of Battle", new Vector3(90.92618f, 6.408124f, 0f), "GG_Workshop"),
            (9, "Oblobbles", new Vector3(100.5547f, 6.408124f, 0f), "GG_Workshop"),
            (10, "Hive Knight", new Vector3(111.3527f, 6.408124f, 0f), "GG_Workshop"),
            (11, "Broken Vessel/Lost Kin", new Vector3(119.6147f, 6.408124f, 0f), "GG_Workshop"),
            (12, "Nosk/Winged Nosk", new Vector3(129.0463f, 6.408124f, 0f), "GG_Workshop"),
            (13, "The Collector", new Vector3(139.9184f, 6.408124f, 0f), "GG_Workshop"),
            (14, "God Tamer", new Vector3(152.4324f, 6.408124f, 0f), "GG_Workshop"),
            (15, "Crystal Guardian/Enraged Guardian", new Vector3(163.0346f, 6.408124f, 0f), "GG_Workshop"),
            (16, "Uumuu", new Vector3(175.5506f, 6.408124f, 0f), "GG_Workshop"),
            (17, "Traitor Lord", new Vector3(186.6932f, 6.408124f, 0f), "GG_Workshop"),
            (18, "Grey Prince Zote", new Vector3(197.9937f, 6.408124f, 0f), "GG_Workshop"),

            // Page 2: Hall of Gods [2 Floor] (19-36)
            (19, "Soul Warrior", new Vector3(33.99411f, 36.40812f, 0f), "GG_Workshop"),
            (20, "Soul Master/Soul Tyrant", new Vector3(44.98816f, 36.40812f, 0f), "GG_Workshop"),
            (21, "Dung Defender/White Defender", new Vector3(57.45609f, 36.40812f, 0f), "GG_Workshop"),
            (22, "Watcher Knights", new Vector3(69.87215f, 36.40812f, 0f), "GG_Workshop"),
            (23, "No Eyes", new Vector3(82.36407f, 36.40812f, 0f), "GG_Workshop"),
            (24, "Marmu", new Vector3(91.36407f, 36.40812f, 0f), "GG_Workshop"),
            (25, "Xero", new Vector3(100.3461f, 36.40812f, 0f), "GG_Workshop"),
            (26, "Markoth", new Vector3(108.8942f, 36.40812f, 0f), "GG_Workshop"),
            (27, "Galien", new Vector3(117.8981f, 36.40812f, 0f), "GG_Workshop"),
            (28, "Gorb", new Vector3(126.3222f, 36.40812f, 0f), "GG_Workshop"),
            (29, "Elder Hu", new Vector3(134.6922f, 36.40812f, 0f), "GG_Workshop"),
            (30, "Brothers Oro & Mato", new Vector3(146.4543f, 36.40812f, 0f), "GG_Workshop"),
            (31, "Paintmaster Sheo", new Vector3(161.4882f, 36.40812f, 0f), "GG_Workshop"),
            (32, "Nailmaster Sly", new Vector3(172.1839f, 36.40812f, 0f), "GG_Workshop"),
            (33, "Pure Vessel", new Vector3(183.3f, 36.4f, 0f), "GG_Workshop"),
            (34, "Grimm/Nightmare King", new Vector3(193.3637f, 36.40812f, 0f), "GG_Workshop"),
            (35, "Absolute Radiance", new Vector3(248.4834f, 36.40812f, 0f), "GG_Workshop"),
            (36, "The Eternal Ordeal", new Vector3(197.8246f, 63.40812f, 0f), "GG_Workshop"),

            // Page 3: Pantheon's (37-43)
            (37, "Pantheon I", new Vector3(97.15343f, 35.40812f, 0f), "GG_Atrium"),
            (38, "Pantheon II", new Vector3(108.4116f, 35.40812f, 0f), "GG_Atrium"),
            (39, "Pantheon III", new Vector3(120.2336f, 35.40812f, 0f), "GG_Atrium"),
            (40, "Pantheon IV", new Vector3(147.3174f, 35.40812f, 0f), "GG_Atrium"),
            (41, "Pantheon V", new Vector3(132f, 94.6236f, 0f), "GG_Atrium"),
            (42, "Segmented Pantheon V", new Vector3(53.81337f, 19.40812f, 0f), "GG_Atrium_Roof"),
            (43, "Pantheon Bench", new Vector3(120.97f, 42.40812f, 0f), "GG_Atrium_Roof"),

            // Page 4: PoP Segments (44-56)
            (44, "PoP Start", new Vector3(2.156f, 7.408124f, 0f), "White_Palace_06"),
            (45, "PoP Segment I [ROOM I]", new Vector3(266.1593f, 12.40812f, 0f), "White_Palace_18"),
            (46, "PoP Segment II [ROOM I]", new Vector3(162.1888f, 11.40812f, 0f), "White_Palace_18"),
            (47, "PoP Segment III [ROOM I]", new Vector3(78.96233f, 35.40812f, 0f), "White_Palace_18"),
            (48, "PoP Segment IV [ROOM II]", new Vector3(60.47843f, 33.40812f, 0f), "White_Palace_17"),
            (49, "PoP Segment V [ROOM II]", new Vector3(32.01745f, 33.40812f, 0f), "White_Palace_17"),
            (50, "PoP Segment VI [ROOM II]", new Vector3(12.39705f, 57.40812f, 0f), "White_Palace_17"),
            (51, "PoP Segment VII [ROOM II]", new Vector3(51.30534f, 63.40812f, 0f), "White_Palace_17"),
            (52, "PoP Segment VIII [ROOM III]", new Vector3(88.826f, 33.40812f, 0f), "White_Palace_19"),
            (53, "PoP Segment IX [ROOM III]", new Vector3(153.3704f, 119.4081f, 0f), "White_Palace_19"),
            (54, "PoP Segment X [ROOM III]", new Vector3(15.4637f, 157.4081f, 0f), "White_Palace_19"),
            (55, "PoP Segment XI [ROOM IV]", new Vector3(19.5625f, 169.4081f, 0f), "White_Palace_20"),
            (56, "PoP Segment XII [ROOM IV]", new Vector3(228.5202f, 168.5394f, 0f), "White_Palace_20")
        };

        public Vector3? CustomTeleportPosition { get; set; }
        public string CustomTeleportScene { get; set; }
    }
}