namespace EPGP.API.Responses;

public class RaiderIoCharactersProfileResponse
{
    private Data.Enums.Class @class;

    public string Name { get; set; }
    public string Race { get; set; }
    public string Class
    {
        get => @class.ToString();
        set
        {
            if (!Enum.TryParse(value.Replace(" ", ""), out Data.Enums.Class classEnum))
            {
                classEnum = Data.Enums.Class.Unknown;
            }

            @class = classEnum;
        }
    }

    public string ActiveSpecName { get; set; }
    public string ActiveSpecRole { get; set; }
    public string Gender { get; set; }
    public string Faction { get; set; }
    public string Region { get; set; }
    public string Realm { get; set; }
    public string ProfileUrl { get; set; }
    public Gear Gear { get; set; }
    public RaidProgression RaidProgression { get; set; }
    public MythicPlusRanks MythicPlusRanks { get; set; }
    public MythicPlusScores MythicPlusScores { get; set; }
    public MythicPlusRanks PreviousMythicPlusRanks { get; set; }
    public MythicPlusScores PreviousMythicPlusScores { get; set; }
    public MythicPlusRuns[] MythicPlusRecentRuns { get; set; }
    public MythicPlusRuns[] MythicPlusBestRuns { get; set; }
    public MythicPlusRuns[] MythicPlusAlternateRuns { get; set; }
}

public class Gear
{
    public int ItemLevelEquipped { get; set; }
    public int ItemLevelTotal { get; set; }
    public int ArtifactTraits { get; set; }
}

public class RaidProgression
{
    public VaultOfTheIncarnates vaultoftheincarnates { get; set; }
    public FatedSepulcherOfTheFirstOnes fatedsepulcherofthefirstones { get; set; }
    public FatedSanctumOfDomination fatedsanctumofdomination { get; set; }
    public FatedCastleNathria fatedcastlenathria { get; set; }
    public SepulcherOfTheFirstOnes sepulcherofthefirstones { get; set; }
    public SanctumOfDomination sanctumofdomination { get; set; }
    public CastleNathria castlenathria { get; set; }
    public NyalothaTheWakingCity nyalothathewakingcity { get; set; }
    public TheEternalPalace theeternalpalace { get; set; }
    public CrucibleOfStorms crucibleofstorms { get; set; }
    public BattleOfDazaralor battleofdazaralor { get; set; }
    public Uldir uldir { get; set; }
    public AntorusTheBurningThrone antorustheburningthrone { get; set; }
    public TombOfSargeras tombofsargeras { get; set; }
    public TheNighthold thenighthold { get; set; }
    public TrialOfValor trialofvalor { get; set; }
    public TheEmeraldNightmare theemeraldnightmare { get; set; }
}

public class VaultOfTheIncarnates
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class FatedSepulcherOfTheFirstOnes
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class FatedSanctumOfDomination
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class FatedCastleNathria
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class SepulcherOfTheFirstOnes
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class SanctumOfDomination
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class CastleNathria
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class NyalothaTheWakingCity
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class TheEternalPalace
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class CrucibleOfStorms
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class BattleOfDazaralor
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class Uldir
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class AntorusTheBurningThrone
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class TombOfSargeras
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class TheNighthold
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class TrialOfValor
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class TheEmeraldNightmare
{
    public string summary { get; set; }
    public int total_bosses { get; set; }
    public int normal_bosses_killed { get; set; }
    public int heroic_bosses_killed { get; set; }
    public int mythic_bosses_killed { get; set; }
}

public class MythicPlusRanks
{
    public MythicPlusRanking Overall { get; set; }
    public MythicPlusRanking Tank { get; set; }
    public MythicPlusRanking Healer { get; set; }
    public MythicPlusRanking Dps { get; set; }
    public MythicPlusRanking Class { get; set; }
    public MythicPlusRanking classTank { get; set; }
    public MythicPlusRanking ClassHealer { get; set; }
    public MythicPlusRanking ClassDps { get; set; }
}

public class MythicPlusRanking
{
    public int World { get; set; }
    public int Region { get; set; }
    public int Realm { get; set; }
}

public class MythicPlusScores
{
    public int All { get; set; }
    public int Dps { get; set; }
    public int Healer { get; set; }
    public int Tank { get; set; }
}

public class PreviousMythicPlusRanks
{
    public MythicPlusRanking Overall { get; set; }
    public MythicPlusRanking Tank { get; set; }
    public MythicPlusRanking Healer { get; set; }
    public MythicPlusRanking Dps { get; set; }
    public MythicPlusRanking Class { get; set; }
    public MythicPlusRanking ClassTank { get; set; }
    public MythicPlusRanking ClassHealer { get; set; }
    public MythicPlusRanking ClassDps { get; set; }
}

public class PreviousMythicPlusScores
{
    public int All { get; set; }
    public int Dps { get; set; }
    public int Healer { get; set; }
    public int Tank { get; set; }
}

public class MythicPlusRuns
{
    public string Dungeon { get; set; }
    public string ShortMame { get; set; }
    public int MythicLevel { get; set; }
    public DateTime CompletedAt { get; set; }
    public int ClearTimeMs { get; set; }
    public int NumKeystoneUpgrades { get; set; }
    public float Score { get; set; }
    public string Url { get; set; }
}