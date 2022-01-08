namespace CommonHandicapLib.Helpers
{
  using System.Collections.Generic;
  using CommonLib.Enumerations;
  using Types;

  public static class AgeRating
  {
    private static List<int[]> Male5KmAgeStandards
    {
      get
      {
        return new List<int[]>
        {
          new int[2] {5, 1286 },
          new int[2] {6, 1181},
          new int[2] {7, 1098},
          new int[2] {8, 1031},
          new int[2] {9, 977},
          new int[2] {10, 932},
          new int[2] {11, 895},
          new int[2] {12, 866},
          new int[2] {13, 842},
          new int[2] {14, 822},
          new int[2] {15, 807},
          new int[2] {16, 795},
          new int[2] {17, 786},
          new int[2] {18, 780},
          new int[2] {19, 779},
          new int[2] {20, 779},
          new int[2] {21, 779},
          new int[2] {22, 779},
          new int[2] {23, 779},
          new int[2] {24, 779},
          new int[2] {25, 779},
          new int[2] {26, 779},
          new int[2] {27, 779},
          new int[2] {28, 779},
          new int[2] {29, 780},
          new int[2] {30, 781},
          new int[2] {31, 783},
          new int[2] {32, 785},
          new int[2] {33, 788},
          new int[2] {34, 792},
          new int[2] {35, 796},
          new int[2] {36, 800},
          new int[2] {37, 805},
          new int[2] {38, 811},
          new int[2] {39, 817},
          new int[2] {40, 823},
          new int[2] {41, 828},
          new int[2] {42, 834},
          new int[2] {43, 840},
          new int[2] {44, 846},
          new int[2] {45, 853},
          new int[2] {46, 859},
          new int[2] {47, 865},
          new int[2] {48, 872},
          new int[2] {49, 878},
          new int[2] {50, 885},
          new int[2] {51, 892},
          new int[2] {52, 899},
          new int[2] {53, 906},
          new int[2] {54, 913},
          new int[2] {55, 920},
          new int[2] {56, 927},
          new int[2] {57, 935},
          new int[2] {58, 943},
          new int[2] {59, 950},
          new int[2] {60, 958},
          new int[2] {61, 966},
          new int[2] {62, 974},
          new int[2] {63, 982},
          new int[2] {64, 991},
          new int[2] {65, 999},
          new int[2] {66, 1008},
          new int[2] {67, 1017},
          new int[2] {68, 1026},
          new int[2] {69, 1037},
          new int[2] {70, 1048},
          new int[2] {71, 1061},
          new int[2] {72, 1075},
          new int[2] {73, 1090},
          new int[2] {74, 1107},
          new int[2] {75, 1125},
          new int[2] {76, 1145},
          new int[2] {77, 1166},
          new int[2] {78, 1190},
          new int[2] {79, 1216},
          new int[2] {80, 1244},
          new int[2] {81, 1275},
          new int[2] {82, 1308},
          new int[2] {83, 1345},
          new int[2] {84, 1386},
          new int[2] {85, 1431},
          new int[2] {86, 1480},
          new int[2] {87, 1535},
          new int[2] {88, 1597},
          new int[2] {89, 1665},
          new int[2] {90, 1743},
          new int[2] {91, 1830},
          new int[2] {92, 1930},
          new int[2] {93, 2044},
          new int[2] {94, 2177},
          new int[2] {95, 2332},
          new int[2] {96, 2517},
          new int[2] {97, 2739},
          new int[2] {98, 3012},
          new int[2] {99, 3353},
          new int[2] {100, 3794}
        };
      }
    }

    private static List<int[]> Female5KmAgeStandards
    {
      get
      {
        return new List<int[]>
        {
          new int[2] {5, 1264 },
          new int[2] {6, 1207},
          new int[2] {7, 1157},
          new int[2] {8, 1114},
          new int[2] {9, 1076},
          new int[2] {10, 1043},
          new int[2] {11, 1014},
          new int[2] {12, 989},
          new int[2] {13, 967},
          new int[2] {14, 947},
          new int[2] {15, 931},
          new int[2] {16, 915},
          new int[2] {17, 900},
          new int[2] {18, 890},
          new int[2] {19, 886},
          new int[2] {20, 886},
          new int[2] {21, 886},
          new int[2] {22, 886},
          new int[2] {23, 886},
          new int[2] {24, 886},
          new int[2] {25, 886},
          new int[2] {26, 886},
          new int[2] {27, 886},
          new int[2] {28, 886},
          new int[2] {29, 886},
          new int[2] {30, 886},
          new int[2] {31, 886},
          new int[2] {32, 887},
          new int[2] {33, 888},
          new int[2] {34, 890},
          new int[2] {35, 892},
          new int[2] {36, 894 },
          new int[2] {37, 898},
          new int[2] {38, 901},
          new int[2] {39, 905},
          new int[2] {40, 910},
          new int[2] {41, 915},
          new int[2] {42, 921},
          new int[2] {43, 928},
          new int[2] {44, 935},
          new int[2] {45, 943},
          new int[2] {46, 951},
          new int[2] {47, 960},
          new int[2] {48, 970},
          new int[2] {49, 981},
          new int[2] {50, 991},
          new int[2] {51, 1002},
          new int[2] {52, 1013},
          new int[2] {53, 1025},
          new int[2] {54, 1035},
          new int[2] {55, 1048},
          new int[2] {56, 1061},
          new int[2] {57, 1073},
          new int[2] {58, 1086},
          new int[2] {59, 1099},
          new int[2] {60, 1112},
          new int[2] {61, 1126},
          new int[2] {62, 1140},
          new int[2] {63, 1155},
          new int[2] {64, 1169},
          new int[2] {65, 1084},
          new int[2] {66, 1200},
          new int[2] {67, 1216},
          new int[2] {68, 1232},
          new int[2] {69, 1249},
          new int[2] {70, 1267},
          new int[2] {71, 1284},
          new int[2] {72, 1303},
          new int[2] {73, 1322},
          new int[2] {74, 1341},
          new int[2] {75, 1361},
          new int[2] {76, 1382},
          new int[2] {77, 1403},
          new int[2] {78, 1425},
          new int[2] {79, 1448},
          new int[2] {80, 1473},
          new int[2] {81, 1502},
          new int[2] {82, 1535},
          new int[2] {83, 1572},
          new int[2] {84, 1613},
          new int[2] {85, 1659},
          new int[2] {86, 1711},
          new int[2] {87, 1771},
          new int[2] {88, 1837},
          new int[2] {89, 1913},
          new int[2] {90, 2000},
          new int[2] {91, 2099},
          new int[2] {92, 2214},
          new int[2] {93, 2348},
          new int[2] {94, 2506},
          new int[2] {95, 2695},
          new int[2] {96, 2923},
          new int[2] {97, 3205},
          new int[2] {98, 3560},
          new int[2] {99, 4020},
          new int[2] {100, 4641}
        };
      }
    }

    /// <summary>
    /// Gets the age graded rating as a percentage string.
    /// </summary>
    /// <param name="age">athlete age</param>
    /// <param name="time">race time</param>
    /// <param name="sex">is male or female</param>
    /// <returns></returns>
    public static string GetAgeGradedRating(
      int age,
      RaceTimeType time,
      SexType sex
      )
    {
      if (sex == SexType.NotSpecified)
      {
        return string.Empty;
      }

      double ageGradedSeconds =
        AgeRating.GetAgeGradedSeconds(
          age,
          sex == SexType.Male ? AgeRating.Male5KmAgeStandards : AgeRating.Female5KmAgeStandards);

      double rating =  ageGradedSeconds / time.TotalSeconds * 100;

      return rating.ToString("#.##") + "%";
    }

    private static double GetAgeGradedSeconds(
      int age,
      List<int[]> ageStandards)
    {
      for (int index = 0; index < ageStandards.Count; ++index)
      {
        if (ageStandards[index][0] == age)
        {
          return (double)ageStandards[index][1];
        }
      }

      return 0;
    }
  }
}