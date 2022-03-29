﻿using Ardalis.SmartEnum;

namespace Moonpie.Protocol.Protocol;

public class ProtocolVersion : SmartEnum<ProtocolVersion>
{
    #region Enum
    #pragma warning disable 
    public static readonly ProtocolVersion v22w03a = new("22w03a",1073741889, true);
    public static readonly ProtocolVersion v1_18_1 = new("1.18.1",757, false);
    public static readonly ProtocolVersion v1_18_1_rc3 = new("1.18.1-rc3",1073741888, true);
    public static readonly ProtocolVersion v1_18_1_rc2 = new("1.18.1-rc2",1073741887, true);
    public static readonly ProtocolVersion v1_18_1_rc1 = new("1.18.1-rc1",1073741886, true);
    public static readonly ProtocolVersion v1_18_1_pre1 = new("1.18.1-pre1",1073741885, true);
    public static readonly ProtocolVersion v1_18_rc4 = new("1.18-rc4",1073741884, true);
    public static readonly ProtocolVersion v1_18_rc3 = new("1.18-rc3",1073741883, true);
    public static readonly ProtocolVersion v1_18_rc2 = new("1.18-rc2",1073741882, true);
    public static readonly ProtocolVersion v1_18_rc1 = new("1.18-rc1",1073741881, true);
    public static readonly ProtocolVersion v1_18_pre8 = new("1.18-pre8",1073741880, true);
    public static readonly ProtocolVersion v1_18_pre7 = new("1.18-pre7",1073741879, true);
    public static readonly ProtocolVersion v1_18_pre6 = new("1.18-pre6",1073741878, true);
    public static readonly ProtocolVersion v1_18_pre5 = new("1.18-pre5",1073741877, true);
    public static readonly ProtocolVersion v1_18_pre4 = new("1.18-pre4",1073741876, true);
    public static readonly ProtocolVersion v1_18_pre3 = new("1.18-pre3",1073741875, true);
    public static readonly ProtocolVersion v1_18_pre2 = new("1.18-pre2",1073741874, true);
    public static readonly ProtocolVersion v1_18_pre1 = new("1.18-pre1",1073741873, true);
    public static readonly ProtocolVersion v21w44a = new("21w44a",1073741872, true);
    public static readonly ProtocolVersion v21w43a = new("21w43a",1073741871, true);
    public static readonly ProtocolVersion v21w42a = new("21w42a",1073741870, true);
    public static readonly ProtocolVersion v21w41a = new("21w41a",1073741869, true);
    public static readonly ProtocolVersion v21w40a = new("21w40a",1073741868, true);
    public static readonly ProtocolVersion v21w39a = new("21w39a",1073741867, true);
    public static readonly ProtocolVersion v21w38a = new("21w38a",1073741866, true);
    public static readonly ProtocolVersion v21w37a = new("21w37a",1073741865, true);
    public static readonly ProtocolVersion v1_17_1 = new("1.17.1",756, false);
    public static readonly ProtocolVersion v1_17_1_rc2 = new("1.17.1-rc2",1073741864, true);
    public static readonly ProtocolVersion v1_17_1_rc1 = new("1.17.1-rc1",1073741863, true);
    public static readonly ProtocolVersion v1_17_1_pre3 = new("1.17.1-pre3",1073741862, true);
    public static readonly ProtocolVersion v1_17_1_pre2 = new("1.17.1-pre2",1073741861, true);
    public static readonly ProtocolVersion v1_17_1_pre1 = new("1.17.1-pre1",1073741860, true);
    public static readonly ProtocolVersion v1_17 = new("1.17",755, false);
    public static readonly ProtocolVersion v1_17_rc2 = new("1.17-rc2",1073741859, true);
    public static readonly ProtocolVersion v1_17_rc1 = new("1.17-rc1",1073741858, true);
    public static readonly ProtocolVersion v1_17_pre5 = new("1.17-pre5",1073741857, true);
    public static readonly ProtocolVersion v1_17_pre4 = new("1.17-pre4",1073741856, true);
    public static readonly ProtocolVersion v1_17_pre3 = new("1.17-pre3",1073741855, true);
    public static readonly ProtocolVersion v1_17_pre2 = new("1.17-pre2",1073741854, true);
    public static readonly ProtocolVersion v1_17_pre1 = new("1.17-pre1",1073741853, true);
    public static readonly ProtocolVersion v21w20a = new("21w20a",1073741852, true);
    public static readonly ProtocolVersion v21w19a = new("21w19a",1073741851, true);
    public static readonly ProtocolVersion v21w18a = new("21w18a",1073741850, true);
    public static readonly ProtocolVersion v21w17a = new("21w17a",1073741849, true);
    public static readonly ProtocolVersion v21w16a = new("21w16a",1073741847, true);
    public static readonly ProtocolVersion v21w15a = new("21w15a",1073741846, true);
    public static readonly ProtocolVersion v21w14a = new("21w14a",1073741845, true);
    public static readonly ProtocolVersion v21w13a = new("21w13a",1073741844, true);
    public static readonly ProtocolVersion v21w11a = new("21w11a",1073741843, true);
    public static readonly ProtocolVersion v21w10a = new("21w10a",1073741842, true);
    public static readonly ProtocolVersion v21w08b = new("21w08b",1073741841, true);
    public static readonly ProtocolVersion v21w08a = new("21w08a",1073741840, true);
    public static readonly ProtocolVersion v21w07a = new("21w07a",1073741839, true);
    public static readonly ProtocolVersion v21w06a = new("21w06a",1073741838, true);
    public static readonly ProtocolVersion v21w05b = new("21w05b",1073741837, true);
    public static readonly ProtocolVersion v21w05a = new("21w05a",1073741836, true);
    public static readonly ProtocolVersion v21w03a = new("21w03a",1073741835, true);
    public static readonly ProtocolVersion v20w51a = new("20w51a",1073741833, true);
    public static readonly ProtocolVersion v20w49a = new("20w49a",1073741832, true);
    public static readonly ProtocolVersion v20w48a = new("20w48a",1073741831, true);
    public static readonly ProtocolVersion v20w46a = new("20w46a",1073741830, true);
    public static readonly ProtocolVersion v20w45a = new("20w45a",1073741829, true);
    public static readonly ProtocolVersion v1_16_5 = new("1.16.5",754, false);
    public static readonly ProtocolVersion v1_16_5_rc1 = new("1.16.5-rc1",1073741834, true);
    public static readonly ProtocolVersion v1_16_4_rc1 = new("1.16.4-rc1",1073741827, true);
    public static readonly ProtocolVersion v1_16_4_pre2 = new("1.16.4-pre2",1073741826, true);
    public static readonly ProtocolVersion v1_16_4_pre1 = new("1.16.4-pre1",1073741825, true);
    public static readonly ProtocolVersion v1_16_3 = new("1.16.3",753, false);
    public static readonly ProtocolVersion v1_16_3_rc1 = new("1.16.3-rc1",752, true);
    public static readonly ProtocolVersion v1_16_2 = new("1.16.2",751, false);
    public static readonly ProtocolVersion v1_16_2_rc2 = new("1.16.2-rc2",750, true);
    public static readonly ProtocolVersion v1_16_2_rc1 = new("1.16.2-rc1",749, true);
    public static readonly ProtocolVersion v1_16_2_pre3 = new("1.16.2-pre3",748, true);
    public static readonly ProtocolVersion v1_16_2_pre2 = new("1.16.2-pre2",746, true);
    public static readonly ProtocolVersion v1_16_2_pre1 = new("1.16.2-pre1",744, true);
    public static readonly ProtocolVersion v20w30a = new("20w30a",743, true);
    public static readonly ProtocolVersion v20w29a = new("20w29a",741, true);
    public static readonly ProtocolVersion v20w28a = new("20w28a",740, true);
    public static readonly ProtocolVersion v20w27a = new("20w27a",738, true);
    public static readonly ProtocolVersion v1_16_1 = new("1.16.1",736, false);
    public static readonly ProtocolVersion v1_16 = new("1.16",735, false);
    public static readonly ProtocolVersion v1_16_rc1 = new("1.16-rc1",734, true);
    public static readonly ProtocolVersion v1_16_pre8 = new("1.16-pre8",733, true);
    public static readonly ProtocolVersion v1_16_pre7 = new("1.16-pre7",732, true);
    public static readonly ProtocolVersion v1_16_pre6 = new("1.16-pre6",730, true);
    public static readonly ProtocolVersion v1_16_pre5 = new("1.16-pre5",729, true);
    public static readonly ProtocolVersion v1_16_pre4 = new("1.16-pre4",727, true);
    public static readonly ProtocolVersion v1_16_pre3 = new("1.16-pre3",725, true);
    public static readonly ProtocolVersion v1_16_pre2 = new("1.16-pre2",722, true);
    public static readonly ProtocolVersion v1_16_pre1 = new("1.16-pre1",721, true);
    public static readonly ProtocolVersion v20w22a = new("20w22a",719, true);
    public static readonly ProtocolVersion v20w21a = new("20w21a",718, true);
    public static readonly ProtocolVersion v20w20b = new("20w20b",717, true);
    public static readonly ProtocolVersion v20w20a = new("20w20a",716, true);
    public static readonly ProtocolVersion v20w19a = new("20w19a",715, true);
    public static readonly ProtocolVersion v20w18a = new("20w18a",714, true);
    public static readonly ProtocolVersion v20w17a = new("20w17a",713, true);
    public static readonly ProtocolVersion v20w16a = new("20w16a",712, true);
    public static readonly ProtocolVersion v20w15a = new("20w15a",711, true);
    public static readonly ProtocolVersion v20w14a = new("20w14a",710, true);
    public static readonly ProtocolVersion v20w13b = new("20w13b",709, true);
    public static readonly ProtocolVersion v20w13a = new("20w13a",708, true);
    public static readonly ProtocolVersion v20w12a = new("20w12a",707, true);
    public static readonly ProtocolVersion v20w11a = new("20w11a",706, true);
    public static readonly ProtocolVersion v20w10a = new("20w10a",705, true);
    public static readonly ProtocolVersion v20w09a = new("20w09a",704, true);
    public static readonly ProtocolVersion v20w08a = new("20w08a",703, true);
    public static readonly ProtocolVersion v20w07a = new("20w07a",702, true);
    public static readonly ProtocolVersion v20w06a = new("20w06a",701, true);
    public static readonly ProtocolVersion v1_15_2 = new("1.15.2",578, false);
    public static readonly ProtocolVersion v1_15_2_pre2 = new("1.15.2-pre2",577, true);
    public static readonly ProtocolVersion v1_15_2_pre1 = new("1.15.2-pre1",576, true);
    public static readonly ProtocolVersion v1_15_1 = new("1.15.1",575, false);
    public static readonly ProtocolVersion v1_15_1_pre1 = new("1.15.1-pre1",574, true);
    public static readonly ProtocolVersion v1_15 = new("1.15",573, false);
    public static readonly ProtocolVersion v1_15_pre7 = new("1.15-pre7",572, true);
    public static readonly ProtocolVersion v1_15_pre6 = new("1.15-pre6",571, true);
    public static readonly ProtocolVersion v1_15_pre5 = new("1.15-pre5",570, true);
    public static readonly ProtocolVersion v1_15_pre4 = new("1.15-pre4",569, true);
    public static readonly ProtocolVersion v1_15_pre3 = new("1.15-pre3",567, true);
    public static readonly ProtocolVersion v1_15_pre2 = new("1.15-pre2",566, true);
    public static readonly ProtocolVersion v1_15_pre1 = new("1.15-pre1",565, true);
    public static readonly ProtocolVersion v19w46b = new("19w46b",564, true);
    public static readonly ProtocolVersion v19w46a = new("19w46a",563, true);
    public static readonly ProtocolVersion v19w45b = new("19w45b",562, true);
    public static readonly ProtocolVersion v19w45a = new("19w45a",561, true);
    public static readonly ProtocolVersion v19w44a = new("19w44a",560, true);
    public static readonly ProtocolVersion v19w42a = new("19w42a",559, true);
    public static readonly ProtocolVersion v19w41a = new("19w41a",558, true);
    public static readonly ProtocolVersion v19w40a = new("19w40a",557, true);
    public static readonly ProtocolVersion v19w39a = new("19w39a",556, true);
    public static readonly ProtocolVersion v19w38b = new("19w38b",555, true);
    public static readonly ProtocolVersion v19w38a = new("19w38a",554, true);
    public static readonly ProtocolVersion v19w37a = new("19w37a",553, true);
    public static readonly ProtocolVersion v19w36a = new("19w36a",552, true);
    public static readonly ProtocolVersion v19w35a = new("19w35a",551, true);
    public static readonly ProtocolVersion v19w34a = new("19w34a",550, true);
    public static readonly ProtocolVersion v1_14_4 = new("1.14.4",498, false);
    public static readonly ProtocolVersion v1_14_4_pre7 = new("1.14.4-pre7",497, true);
    public static readonly ProtocolVersion v1_14_4_pre6 = new("1.14.4-pre6",496, true);
    public static readonly ProtocolVersion v1_14_4_pre5 = new("1.14.4-pre5",495, true);
    public static readonly ProtocolVersion v1_14_4_pre4 = new("1.14.4-pre4",494, true);
    public static readonly ProtocolVersion v1_14_4_pre3 = new("1.14.4-pre3",493, true);
    public static readonly ProtocolVersion v1_14_4_pre2 = new("1.14.4-pre2",492, true);
    public static readonly ProtocolVersion v1_14_4_pre1 = new("1.14.4-pre1",491, true);
    public static readonly ProtocolVersion v1_14_3 = new("1.14.3",490, false);
    public static readonly ProtocolVersion v1_14_3_pre4 = new("1.14.3-pre4",489, true);
    public static readonly ProtocolVersion v1_14_3_pre3 = new("1.14.3-pre3",488, true);
    public static readonly ProtocolVersion v1_14_3_pre2 = new("1.14.3-pre2",487, true);
    public static readonly ProtocolVersion v1_14_3_pre1 = new("1.14.3-pre1",486, true);
    public static readonly ProtocolVersion v1_14_2 = new("1.14.2",485, false);
    public static readonly ProtocolVersion v1_14_2_pre4 = new("1.14.2-pre4",484, true);
    public static readonly ProtocolVersion v1_14_2_pre3 = new("1.14.2-pre3",483, true);
    public static readonly ProtocolVersion v1_14_2_pre2 = new("1.14.2-pre2",482, true);
    public static readonly ProtocolVersion v1_14_2_pre1 = new("1.14.2-pre1",481, true);
    public static readonly ProtocolVersion v1_14_1 = new("1.14.1",480, false);
    public static readonly ProtocolVersion v1_14_1_pre2 = new("1.14.1-pre2",479, true);
    public static readonly ProtocolVersion v1_14_1_pre1 = new("1.14.1-pre1",478, true);
    public static readonly ProtocolVersion v1_14 = new("1.14",477, false);
    public static readonly ProtocolVersion v1_14_pre5 = new("1.14-pre5",476, true);
    public static readonly ProtocolVersion v1_14_pre4 = new("1.14-pre4",475, true);
    public static readonly ProtocolVersion v1_14_pre3 = new("1.14-pre3",474, true);
    public static readonly ProtocolVersion v1_14_pre2 = new("1.14-pre2",473, true);
    public static readonly ProtocolVersion v1_14_pre1 = new("1.14-pre1",472, true);
    public static readonly ProtocolVersion v19w14b = new("19w14b",471, true);
    public static readonly ProtocolVersion v19w14a = new("19w14a",470, true);
    public static readonly ProtocolVersion v19w13b = new("19w13b",469, true);
    public static readonly ProtocolVersion v19w13a = new("19w13a",468, true);
    public static readonly ProtocolVersion v19w12b = new("19w12b",467, true);
    public static readonly ProtocolVersion v19w12a = new("19w12a",466, true);
    public static readonly ProtocolVersion v19w11b = new("19w11b",465, true);
    public static readonly ProtocolVersion v19w11a = new("19w11a",464, true);
    public static readonly ProtocolVersion v19w09a = new("19w09a",463, true);
    public static readonly ProtocolVersion v19w08b = new("19w08b",462, true);
    public static readonly ProtocolVersion v19w08a = new("19w08a",461, true);
    public static readonly ProtocolVersion v19w07a = new("19w07a",460, true);
    public static readonly ProtocolVersion v19w06a = new("19w06a",459, true);
    public static readonly ProtocolVersion v19w05a = new("19w05a",458, true);
    public static readonly ProtocolVersion v19w04b = new("19w04b",457, true);
    public static readonly ProtocolVersion v19w04a = new("19w04a",456, true);
    public static readonly ProtocolVersion v19w03c = new("19w03c",455, true);
    public static readonly ProtocolVersion v19w03b = new("19w03b",454, true);
    public static readonly ProtocolVersion v19w03a = new("19w03a",453, true);
    public static readonly ProtocolVersion v19w02a = new("19w02a",452, true);
    public static readonly ProtocolVersion v18w50a = new("18w50a",451, true);
    public static readonly ProtocolVersion v18w49a = new("18w49a",450, true);
    public static readonly ProtocolVersion v18w48b = new("18w48b",449, true);
    public static readonly ProtocolVersion v18w48a = new("18w48a",448, true);
    public static readonly ProtocolVersion v18w47b = new("18w47b",447, true);
    public static readonly ProtocolVersion v18w47a = new("18w47a",446, true);
    public static readonly ProtocolVersion v18w46a = new("18w46a",445, true);
    public static readonly ProtocolVersion v18w45a = new("18w45a",444, true);
    public static readonly ProtocolVersion v18w44a = new("18w44a",443, true);
    public static readonly ProtocolVersion v18w43c = new("18w43c",442, true);
    public static readonly ProtocolVersion v18w43b = new("18w43b",441, true);
    public static readonly ProtocolVersion v18w43a = new("18w43a",440, true);
    public static readonly ProtocolVersion v1_13_2 = new("1.13.2",404, false);
    public static readonly ProtocolVersion v1_13_2_pre2 = new("1.13.2-pre2",403, true);
    public static readonly ProtocolVersion v1_13_2_pre1 = new("1.13.2-pre1",402, true);
    public static readonly ProtocolVersion v1_13_1 = new("1.13.1",401, false);
    public static readonly ProtocolVersion v1_13_1_pre2 = new("1.13.1-pre2",400, true);
    public static readonly ProtocolVersion v1_13_1_pre1 = new("1.13.1-pre1",399, true);
    public static readonly ProtocolVersion v18w33a = new("18w33a",398, true);
    public static readonly ProtocolVersion v18w32a = new("18w32a",397, true);
    public static readonly ProtocolVersion v18w31a = new("18w31a",396, true);
    public static readonly ProtocolVersion v18w30b = new("18w30b",395, true);
    public static readonly ProtocolVersion v18w30a = new("18w30a",394, true);
    public static readonly ProtocolVersion v1_13 = new("1.13",393, false);
    public static readonly ProtocolVersion v1_13_pre10 = new("1.13-pre10",392, true);
    public static readonly ProtocolVersion v1_13_pre9 = new("1.13-pre9",391, true);
    public static readonly ProtocolVersion v1_13_pre8 = new("1.13-pre8",390, true);
    public static readonly ProtocolVersion v1_13_pre7 = new("1.13-pre7",389, true);
    public static readonly ProtocolVersion v1_13_pre6 = new("1.13-pre6",388, true);
    public static readonly ProtocolVersion v1_13_pre5 = new("1.13-pre5",387, true);
    public static readonly ProtocolVersion v1_13_pre4 = new("1.13-pre4",386, true);
    public static readonly ProtocolVersion v1_13_pre3 = new("1.13-pre3",385, true);
    public static readonly ProtocolVersion v1_13_pre2 = new("1.13-pre2",384, true);
    public static readonly ProtocolVersion v1_13_pre1 = new("1.13-pre1",383, true);
    public static readonly ProtocolVersion v18w22c = new("18w22c",382, true);
    public static readonly ProtocolVersion v18w22b = new("18w22b",381, true);
    public static readonly ProtocolVersion v18w22a = new("18w22a",380, true);
    public static readonly ProtocolVersion v18w21b = new("18w21b",379, true);
    public static readonly ProtocolVersion v18w21a = new("18w21a",378, true);
    public static readonly ProtocolVersion v18w20c = new("18w20c",377, true);
    public static readonly ProtocolVersion v18w20b = new("18w20b",376, true);
    public static readonly ProtocolVersion v18w20a = new("18w20a",375, true);
    public static readonly ProtocolVersion v18w19b = new("18w19b",374, true);
    public static readonly ProtocolVersion v18w19a = new("18w19a",373, true);
    public static readonly ProtocolVersion v18w16a = new("18w16a",372, true);
    public static readonly ProtocolVersion v18w15a = new("18w15a",371, true);
    public static readonly ProtocolVersion v18w14b = new("18w14b",370, true);
    public static readonly ProtocolVersion v18w14a = new("18w14a",369, true);
    public static readonly ProtocolVersion v18w11a = new("18w11a",368, true);
    public static readonly ProtocolVersion v18w10d = new("18w10d",367, true);
    public static readonly ProtocolVersion v18w10c = new("18w10c",366, true);
    public static readonly ProtocolVersion v18w10b = new("18w10b",365, true);
    public static readonly ProtocolVersion v18w10a = new("18w10a",364, true);
    public static readonly ProtocolVersion v18w09a = new("18w09a",363, true);
    public static readonly ProtocolVersion v18w08b = new("18w08b",362, true);
    public static readonly ProtocolVersion v18w08a = new("18w08a",361, true);
    public static readonly ProtocolVersion v18w07c = new("18w07c",360, true);
    public static readonly ProtocolVersion v18w07b = new("18w07b",359, true);
    public static readonly ProtocolVersion v18w07a = new("18w07a",358, true);
    public static readonly ProtocolVersion v18w06a = new("18w06a",357, true);
    public static readonly ProtocolVersion v18w05a = new("18w05a",356, true);
    public static readonly ProtocolVersion v18w03b = new("18w03b",355, true);
    public static readonly ProtocolVersion v18w03a = new("18w03a",354, true);
    public static readonly ProtocolVersion v18w02a = new("18w02a",353, true);
    public static readonly ProtocolVersion v18w01a = new("18w01a",352, true);
    public static readonly ProtocolVersion v17w50a = new("17w50a",351, true);
    public static readonly ProtocolVersion v17w49b = new("17w49b",350, true);
    public static readonly ProtocolVersion v17w49a = new("17w49a",349, true);
    public static readonly ProtocolVersion v17w48a = new("17w48a",348, true);
    public static readonly ProtocolVersion v17w47b = new("17w47b",347, true);
    public static readonly ProtocolVersion v17w47a = new("17w47a",346, true);
    public static readonly ProtocolVersion v17w46a = new("17w46a",345, true);
    public static readonly ProtocolVersion v17w45b = new("17w45b",344, true);
    public static readonly ProtocolVersion v17w45a = new("17w45a",343, true);
    public static readonly ProtocolVersion v17w43b = new("17w43b",342, true);
    public static readonly ProtocolVersion v17w43a = new("17w43a",341, true);
    public static readonly ProtocolVersion v1_12_2 = new("1.12.2",340, false);
    public static readonly ProtocolVersion v1_12_2_pre2 = new("1.12.2-pre2",339, true);
    public static readonly ProtocolVersion v1_12_1 = new("1.12.1",338, false);
    public static readonly ProtocolVersion v1_12_1_pre1 = new("1.12.1-pre1",337, true);
    public static readonly ProtocolVersion v17w31a = new("17w31a",336, true);
    public static readonly ProtocolVersion v1_12 = new("1.12",335, false);
    public static readonly ProtocolVersion v1_12_pre7 = new("1.12-pre7",334, true);
    public static readonly ProtocolVersion v1_12_pre6 = new("1.12-pre6",333, true);
    public static readonly ProtocolVersion v1_12_pre5 = new("1.12-pre5",332, true);
    public static readonly ProtocolVersion v1_12_pre4 = new("1.12-pre4",331, true);
    public static readonly ProtocolVersion v1_12_pre3 = new("1.12-pre3",330, true);
    public static readonly ProtocolVersion v1_12_pre2 = new("1.12-pre2",329, true);
    public static readonly ProtocolVersion v1_12_pre1 = new("1.12-pre1",328, true);
    public static readonly ProtocolVersion v17w18b = new("17w18b",327, true);
    public static readonly ProtocolVersion v17w18a = new("17w18a",326, true);
    public static readonly ProtocolVersion v17w17b = new("17w17b",325, true);
    public static readonly ProtocolVersion v17w17a = new("17w17a",324, true);
    public static readonly ProtocolVersion v17w16b = new("17w16b",323, true);
    public static readonly ProtocolVersion v17w16a = new("17w16a",322, true);
    public static readonly ProtocolVersion v17w15a = new("17w15a",321, true);
    public static readonly ProtocolVersion v17w14a = new("17w14a",320, true);
    public static readonly ProtocolVersion v17w13b = new("17w13b",319, true);
    public static readonly ProtocolVersion v17w13a = new("17w13a",318, true);
    public static readonly ProtocolVersion v17w06a = new("17w06a",317, true);
    public static readonly ProtocolVersion v1_11_2 = new("1.11.2",316, false);
    public static readonly ProtocolVersion v1_11 = new("1.11",315, false);
    public static readonly ProtocolVersion v1_11_pre1 = new("1.11-pre1",314, true);
    public static readonly ProtocolVersion v16w44a = new("16w44a",313, true);
    public static readonly ProtocolVersion v16w42a = new("16w42a",312, true);
    public static readonly ProtocolVersion v16w41a = new("16w41a",311, true);
    public static readonly ProtocolVersion v16w40a = new("16w40a",310, true);
    public static readonly ProtocolVersion v16w39c = new("16w39c",309, true);
    public static readonly ProtocolVersion v16w39b = new("16w39b",308, true);
    public static readonly ProtocolVersion v16w39a = new("16w39a",307, true);
    public static readonly ProtocolVersion v16w38a = new("16w38a",306, true);
    public static readonly ProtocolVersion v16w36a = new("16w36a",305, true);
    public static readonly ProtocolVersion v16w35a = new("16w35a",304, true);
    public static readonly ProtocolVersion v16w33a = new("16w33a",303, true);
    public static readonly ProtocolVersion v16w32b = new("16w32b",302, true);
    public static readonly ProtocolVersion v16w32a = new("16w32a",301, true);
    public static readonly ProtocolVersion v1_10_2 = new("1.10.2",210, false);
    public static readonly ProtocolVersion v1_10_pre2 = new("1.10-pre2",205, true);
    public static readonly ProtocolVersion v1_10_pre1 = new("1.10-pre1",204, true);
    public static readonly ProtocolVersion v16w21b = new("16w21b",203, true);
    public static readonly ProtocolVersion v16w21a = new("16w21a",202, true);
    public static readonly ProtocolVersion v16w20a = new("16w20a",201, true);
    public static readonly ProtocolVersion v1_9_4 = new("1.9.4",110, false);
    public static readonly ProtocolVersion v1_9_3_pre1 = new("1.9.3-pre1",109, true);
    public static readonly ProtocolVersion v1_9_1 = new("1.9.1",108, false);
    public static readonly ProtocolVersion v1_9_1_pre1 = new("1.9.1-pre1",107, true);
    public static readonly ProtocolVersion v1_9_pre4 = new("1.9-pre4",106, true);
    public static readonly ProtocolVersion v1_9_pre3 = new("1.9-pre3",105, true);
    public static readonly ProtocolVersion v1_9_pre2 = new("1.9-pre2",104, true);
    public static readonly ProtocolVersion v1_9_pre1 = new("1.9-pre1",103, true);
    public static readonly ProtocolVersion v16w07b = new("16w07b",102, true);
    public static readonly ProtocolVersion v16w07a = new("16w07a",101, true);
    public static readonly ProtocolVersion v16w06a = new("16w06a",100, true);
    public static readonly ProtocolVersion v16w05b = new("16w05b",99, true);
    public static readonly ProtocolVersion v16w05a = new("16w05a",98, true);
    public static readonly ProtocolVersion v16w04a = new("16w04a",97, true);
    public static readonly ProtocolVersion v16w03a = new("16w03a",96, true);
    public static readonly ProtocolVersion v16w02a = new("16w02a",95, true);
    public static readonly ProtocolVersion v15w51b = new("15w51b",94, true);
    public static readonly ProtocolVersion v15w51a = new("15w51a",93, true);
    public static readonly ProtocolVersion v15w50a = new("15w50a",92, true);
    public static readonly ProtocolVersion v15w49b = new("15w49b",91, true);
    public static readonly ProtocolVersion v15w49a = new("15w49a",90, true);
    public static readonly ProtocolVersion v15w47c = new("15w47c",89, true);
    public static readonly ProtocolVersion v15w47b = new("15w47b",88, true);
    public static readonly ProtocolVersion v15w47a = new("15w47a",87, true);
    public static readonly ProtocolVersion v15w46a = new("15w46a",86, true);
    public static readonly ProtocolVersion v15w45a = new("15w45a",85, true);
    public static readonly ProtocolVersion v15w44b = new("15w44b",84, true);
    public static readonly ProtocolVersion v15w44a = new("15w44a",83, true);
    public static readonly ProtocolVersion v15w43c = new("15w43c",82, true);
    public static readonly ProtocolVersion v15w43b = new("15w43b",81, true);
    public static readonly ProtocolVersion v15w43a = new("15w43a",80, true);
    public static readonly ProtocolVersion v15w42a = new("15w42a",79, true);
    public static readonly ProtocolVersion v15w41b = new("15w41b",78, true);
    public static readonly ProtocolVersion v15w41a = new("15w41a",77, true);
    public static readonly ProtocolVersion v15w40b = new("15w40b",76, true);
    public static readonly ProtocolVersion v15w40a = new("15w40a",75, true);
    public static readonly ProtocolVersion v15w39c = new("15w39c",74, true);
    public static readonly ProtocolVersion v15w38b = new("15w38b",73, true);
    public static readonly ProtocolVersion v15w38a = new("15w38a",72, true);
    public static readonly ProtocolVersion v15w37a = new("15w37a",71, true);
    public static readonly ProtocolVersion v15w36d = new("15w36d",70, true);
    public static readonly ProtocolVersion v15w36c = new("15w36c",69, true);
    public static readonly ProtocolVersion v15w36b = new("15w36b",68, true);
    public static readonly ProtocolVersion v15w36a = new("15w36a",67, true);
    public static readonly ProtocolVersion v15w35e = new("15w35e",66, true);
    public static readonly ProtocolVersion v15w35d = new("15w35d",65, true);
    public static readonly ProtocolVersion v15w35c = new("15w35c",64, true);
    public static readonly ProtocolVersion v15w35b = new("15w35b",63, true);
    public static readonly ProtocolVersion v15w35a = new("15w35a",62, true);
    public static readonly ProtocolVersion v15w34d = new("15w34d",61, true);
    public static readonly ProtocolVersion v15w34c = new("15w34c",60, true);
    public static readonly ProtocolVersion v15w34b = new("15w34b",59, true);
    public static readonly ProtocolVersion v15w34a = new("15w34a",58, true);
    public static readonly ProtocolVersion v15w33c = new("15w33c",57, true);
    public static readonly ProtocolVersion v15w33b = new("15w33b",56, true);
    public static readonly ProtocolVersion v15w33a = new("15w33a",55, true);
    public static readonly ProtocolVersion v15w32c = new("15w32c",54, true);
    public static readonly ProtocolVersion v15w32b = new("15w32b",53, true);
    public static readonly ProtocolVersion v15w32a = new("15w32a",52, true);
    public static readonly ProtocolVersion v15w31c = new("15w31c",51, true);
    public static readonly ProtocolVersion v15w31b = new("15w31b",50, true);
    public static readonly ProtocolVersion v15w31a = new("15w31a",49, true);
    public static readonly ProtocolVersion v1_8_9 = new("1.8.9",47, false);
    public static readonly ProtocolVersion v1_8_pre3 = new("1.8-pre3",46, true);
    public static readonly ProtocolVersion v1_8_pre2 = new("1.8-pre2",45, true);
    public static readonly ProtocolVersion v1_8_pre1 = new("1.8-pre1",44, true);
    public static readonly ProtocolVersion v14w34d = new("14w34d",43, true);
    public static readonly ProtocolVersion v14w34c = new("14w34c",42, true);
    public static readonly ProtocolVersion v14w34b = new("14w34b",41, true);
    public static readonly ProtocolVersion v14w34a = new("14w34a",40, true);
    public static readonly ProtocolVersion v14w33c = new("14w33c",39, true);
    public static readonly ProtocolVersion v14w33b = new("14w33b",38, true);
    public static readonly ProtocolVersion v14w33a = new("14w33a",37, true);
    public static readonly ProtocolVersion v14w32d = new("14w32d",36, true);
    public static readonly ProtocolVersion v14w32c = new("14w32c",35, true);
    public static readonly ProtocolVersion v14w32b = new("14w32b",34, true);
    public static readonly ProtocolVersion v14w32a = new("14w32a",33, true);
    public static readonly ProtocolVersion v14w31a = new("14w31a",32, true);
    public static readonly ProtocolVersion v14w30c = new("14w30c",31, true);
    public static readonly ProtocolVersion v14w30b = new("14w30b",30, true);
    public static readonly ProtocolVersion v14w29a = new("14w29a",29, true);
    public static readonly ProtocolVersion v14w28b = new("14w28b",28, true);
    public static readonly ProtocolVersion v14w28a = new("14w28a",27, true);
    public static readonly ProtocolVersion v14w27b = new("14w27b",26, true);
    public static readonly ProtocolVersion v14w26c = new("14w26c",25, true);
    public static readonly ProtocolVersion v14w26b = new("14w26b",24, true);
    public static readonly ProtocolVersion v14w26a = new("14w26a",23, true);
    public static readonly ProtocolVersion v14w25b = new("14w25b",22, true);
    public static readonly ProtocolVersion v14w25a = new("14w25a",21, true);
    public static readonly ProtocolVersion v14w21b = new("14w21b",20, true);
    public static readonly ProtocolVersion v14w21a = new("14w21a",19, true);
    public static readonly ProtocolVersion v14w20b = new("14w20b",18, true);
    public static readonly ProtocolVersion v14w19a = new("14w19a",17, true);
    public static readonly ProtocolVersion v14w18b = new("14w18b",16, true);
    public static readonly ProtocolVersion v14w17a = new("14w17a",15, true);
    public static readonly ProtocolVersion v14w11b = new("14w11b",14, true);
    public static readonly ProtocolVersion v14w10c = new("14w10c",13, true);
    public static readonly ProtocolVersion v14w08a = new("14w08a",12, true);
    public static readonly ProtocolVersion v14w07a = new("14w07a",11, true);
    public static readonly ProtocolVersion v14w06b = new("14w06b",10, true);
    public static readonly ProtocolVersion v14w05b = new("14w05b",9, true);
    public static readonly ProtocolVersion v14w04b = new("14w04b",8, true);
    public static readonly ProtocolVersion v14w04a = new("14w04a",7, true);
    public static readonly ProtocolVersion v14w03b = new("14w03b",6, true);
    public static readonly ProtocolVersion v1_7_10 = new("1.7.10",5, false);
    public static readonly ProtocolVersion v1_7_5 = new("1.7.5",4, false);
    public static readonly ProtocolVersion v1_7_1_pre = new("1.7.1-pre",3, true);
    public static readonly ProtocolVersion v13w43a = new("13w43a",2, true);
    public static readonly ProtocolVersion v13w42b = new("13w42b",1, true);
    public static readonly ProtocolVersion v13w41b = new("13w41b",0, true);
    #endregion
    
    public bool Snapshot { get; }
    
    private ProtocolVersion(string name, int protocol, bool snapshot) : base(name, protocol)
    {
        Snapshot = snapshot;
    }
}