using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NostaleGewinnRechner.Models;

public static class Fmt
{
    private static readonly CultureInfo De = CultureInfo.GetCultureInfo("de-DE");

    public static string Gold(double v) => Math.Round(v).ToString("N0", De);

    /// <summary>Invariant-Formatierung für CSS-Werte (Punkt statt Komma).</summary>
    public static string Css(double v) => v.ToString("0.##", CultureInfo.InvariantCulture);
}

/// <summary>Eigener Charakter: erhöhte Gold-Rate in Prozent (z. B. durch Fee, Kostüm, Titel).</summary>
public class Charakter
{
    private double goldRate;

    /// <summary>Gold-Rate in Prozent – im Spiel bei 200 % gedeckelt.</summary>
    public double GoldRate
    {
        get => goldRate;
        set => goldRate = Math.Clamp(value, 0, 200);
    }

    /// <summary>Multiplikator für reine Gold-Einnahmen, z. B. 25 % → 1,25.</summary>
    [JsonIgnore]
    public double Faktor => 1 + GoldRate / 100;
}

/// <summary>
/// Globale Materialpreise – überschreiben beim Setzen die Preise in allen Rechnern.
/// Die Standardwerte sind die vom Nutzer erhobenen Bazar-Durchschnittspreise
/// (Stand 2026-08-14). Obsidian ist dort nicht handelbar – der Wert stammt noch
/// aus der ursprünglichen Vorlage und dient als Schätzung für die Act-7-Rechner.
/// </summary>
public class MatPreise
{
    public double Vollmond { get; set; } = 14_999;
    public double Engelsfeder { get; set; } = 26_250;
    public double ExpPot { get; set; } = 156_666;
    public double AtkPot { get; set; } = 5_977;
    public double DefPot { get; set; } = 6_500;
    public double EnergiePot { get; set; } = 935;
    /// <summary>Riesige Erholungstränke</summary>
    public double Riesige { get; set; } = 1_488;
    public double Fulli { get; set; } = 32_442;
    public double Obsidian { get; set; } = 13_000;

    // ---- Aufwertungs-Materialien (Uppen) – fließen in die Fahrpläne der Charakter-Analyse ----
    public double Cella { get; set; } = 1_400;
    public double Seelenstein { get; set; } = 397;
    /// <summary>Vollkommener Seelenstein (Waffe ab +6).</summary>
    public double SeelensteinVollkommen { get; set; } = 267;
    public double Drachenedelstein { get; set; } = 21_950;
    /// <summary>Glänzende Seele (je Stück, SP +6 bis +10).</summary>
    public double GlaenzendeSeele { get; set; } = 14_997;
    /// <summary>Seele der höheren Art (SP +11 bis +15, laut Guide Ork-Seelensteine / Seelen der Altehrwürdigen Helden).</summary>
    public double SeeleHoehere { get; set; } = 8_000;

    // ---- Runen-Materialien (Runenschnitzen auf Waffen) ----
    public double MorizioObsidian { get; set; } = 17_450;
    public double LoaRunenpulver { get; set; } = 5_156;
    public double Titanbarren { get; set; } = 124_999;

    // ---- Schutzrollen: je Versuch eine, ab der Zerstörungsgrenze ----
    // Die goldene bzw. Premium-Variante halbiert zusätzlich einen Teil der
    // Materialien und das Gold (siehe AufwertungDaten).
    /// <summary>Ausrüstungsschutzrolle (Waffe/Rüstung).</summary>
    public double RolleAusruestung { get; set; } = 2_346_950;

    /// <summary>Goldene Ausrüstungsschutzrolle – halbiert Cella und Gold.</summary>
    public double RolleAusruestungGolden { get; set; } = 2_830_000;

    /// <summary>Niedrige SP-Schutzrolle (bis Stufe +10).</summary>
    public double RolleSpNiedrig { get; set; } = 179_000;

    /// <summary>Niedrige SP-Schutzrolle (Premium) – halbiert Vollmonde, Federn und Gold.</summary>
    public double RolleSpNiedrigPremium { get; set; } = 168_999;

    /// <summary>Hohe SP-Schutzrolle (+11 bis +15).</summary>
    public double RolleSpHoch { get; set; } = 647_000;

    /// <summary>Hohe SP-Schutzrolle (Premium).</summary>
    public double RolleSpHochPremium { get; set; } = 2_198_888;

    /// <summary>Drachen-Kartenschutzrolle (+16 bis +20).</summary>
    public double RolleDrachen { get; set; } = 589_000;

    /// <summary>Drachen-Kartenschutzrolle (Premium).</summary>
    public double RolleDrachenPremium { get; set; } = 3_693_332;

    // ---- Raid-Zutaten (Eleraids) ----
    // Glace und Draco haben eigene Siegel: verschiedene Items, aus verschiedenen
    // Zutaten hergestellt – Glace aus Eisblumen und Eiswürfeln, Draco aus Gillion.
    /// <summary>Eisblumen – nur für das Glace-Siegel.</summary>
    public double Eisblume { get; set; } = 79_987;

    /// <summary>Eiswürfel – nur für das Glace-Siegel.</summary>
    public double Eiswuerfel { get; set; } = 700;

    /// <summary>Saat – geht in beide Siegel.</summary>
    public double Saat { get; set; } = 236;

    /// <summary>Gillion – nur für das Draco-Siegel.</summary>
    public double Gillion { get; set; } = 9_149;

    /// <summary>Fertiges Glace-Siegel.</summary>
    public double SiegelGlace { get; set; } = 1_588_999;

    /// <summary>Fertiges Draco-Siegel.</summary>
    public double SiegelDraco { get; set; } = 216_997;

    // ---- Boss-Siegel: nicht herstellbar, nur Drop oder Kauf ----
    public double SiegelKertos { get; set; } = 569_999;
    public double SiegelErenia { get; set; } = 489_000;
    public double SiegelZenas { get; set; } = 639_000;
    public double SiegelFernon { get; set; } = 1_700_000;
    public double SiegelCarno { get; set; } = 268_888;
    public double SiegelKirolla { get; set; } = 535_000;
    public double SiegelBelial { get; set; } = 499_998;
    public double SiegelAlzanor { get; set; } = 37_000;
    public double SiegelPollu { get; set; } = 29_300;

    // ---- Truhen und Kisten je Aktivität ----
    // Bewusst je Aktivität getrennt: „Kisten Low Rare“ aus Carno und aus Belial
    // sind verschiedene Items mit verschiedenen Werten.
    /// <summary>Blaue Steine aus den Eleraid-Boxen – nicht handelbar, Wert geschätzt.</summary>
    public double BlaueSteine { get; set; } = 100_000;
    public double TruheGlace { get; set; } = 161_998;
    public double TruheDraco { get; set; } = 68_448;
    public double TruheAct5 { get; set; } = 147_999;
    /// <summary>Intakte Items aus den Act-5-Truhen.</summary>
    public double Act5Intakt { get; set; } = 50_000;
    /// <summary>Kaputte Items aus den Act-5-Truhen.</summary>
    public double Act5Kaputt { get; set; } = 10_000;
    public double TruheErenia { get; set; } = 165_968;
    public double TruheZenas { get; set; } = 135_000;
    public double TruheCarnoLow { get; set; } = 69_900;
    public double TruheCarnoHigh { get; set; } = 469_000;
    public double TruheKirollaLow { get; set; } = 22_999;
    public double TruheKirollaHigh { get; set; } = 333_333;
    public double TruheBelialLow { get; set; } = 28_581;
    public double TruheBelialHigh { get; set; } = 389_894;
    public double TruheAct8 { get; set; } = 247_999;
    public double TruheAct8High { get; set; } = 1_100_000;
    public double TruheAct9 { get; set; } = 350_000;
    public double TruheAct9High { get; set; } = 1_600_000;

    /// <summary>Alle Preise mit ihrem Schlüssel – Grundlage für <see cref="RechnerState.MatsAnwenden"/>.</summary>
    public IEnumerable<(string Schluessel, double Preis)> Alle =>
    [
        ("vollmond", Vollmond), ("engelsfeder", Engelsfeder), ("exppot", ExpPot),
        ("atkpot", AtkPot), ("defpot", DefPot), ("energiepot", EnergiePot),
        ("riesige", Riesige), ("fulli", Fulli), ("obsidian", Obsidian),
        ("cella", Cella), ("seelenstein", Seelenstein), ("seelensteinvollkommen", SeelensteinVollkommen),
        ("drachenedelstein", Drachenedelstein), ("glaenzendeseele", GlaenzendeSeele), ("seelehoehere", SeeleHoehere),
        ("morizioobsidian", MorizioObsidian), ("loarunenpulver", LoaRunenpulver), ("titanbarren", Titanbarren),
        ("rolleausruestung", RolleAusruestung), ("rolleausruestunggolden", RolleAusruestungGolden),
        ("rollespniedrig", RolleSpNiedrig), ("rollespniedrigpremium", RolleSpNiedrigPremium),
        ("rollesphoch", RolleSpHoch), ("rollesphochpremium", RolleSpHochPremium),
        ("rolledrachen", RolleDrachen), ("rolledrachenpremium", RolleDrachenPremium),
        ("eisblume", Eisblume), ("eiswuerfel", Eiswuerfel), ("saat", Saat),
        ("gillion", Gillion), ("siegelglace", SiegelGlace), ("siegeldraco", SiegelDraco),
        ("siegelkertos", SiegelKertos), ("siegelerenia", SiegelErenia), ("siegelzenas", SiegelZenas),
        ("siegelfernon", SiegelFernon), ("siegelcarno", SiegelCarno), ("siegelkirolla", SiegelKirolla),
        ("siegelbelial", SiegelBelial), ("siegelalzanor", SiegelAlzanor), ("siegelpollu", SiegelPollu),
        ("blauesteine", BlaueSteine), ("truheglace", TruheGlace), ("truhedraco", TruheDraco),
        ("truheact5", TruheAct5), ("act5intakt", Act5Intakt), ("act5kaputt", Act5Kaputt),
        ("truheerenia", TruheErenia), ("truhezenas", TruheZenas),
        ("truhecarnolow", TruheCarnoLow), ("truhecarnohigh", TruheCarnoHigh),
        ("truhekirollalow", TruheKirollaLow), ("truhekirollahigh", TruheKirollaHigh),
        ("truhebeliallow", TruheBelialLow), ("truhebelialhigh", TruheBelialHigh),
        ("truheact8", TruheAct8), ("truheact8high", TruheAct8High),
        ("truheact9", TruheAct9), ("truheact9high", TruheAct9High),
    ];

    /// <summary>
    /// Preis eines Aufwertungs-Materials aus <see cref="AufwertungDaten"/> –
    /// Zuordnung über den Material-Namen; Vollmondkristalle und Engelsfedern
    /// nutzen die bestehenden Drop-Preise.
    /// </summary>
    public double AufwertungPreis(string material) => material switch
    {
        "Cella" => Cella,
        "Seelenstein" => Seelenstein,
        "Vollkommener Seelenstein" => SeelensteinVollkommen,
        "Vollmondkristall" => Vollmond,
        "Engelsfeder" => Engelsfeder,
        "Drachenedelstein" => Drachenedelstein,
        "Morizios-Obsidian" => MorizioObsidian,
        "Loa-Runenpulver" => LoaRunenpulver,
        "Schwarzer Titanbarren" => Titanbarren,
        _ when material.StartsWith("Glänzende Seele") => GlaenzendeSeele,
        _ when material.StartsWith("Seele (höhere Art)") => SeeleHoehere,
        _ => 0,
    };

}

/// <summary>Zeile mit fester Anzahl und editierbarem Preis.</summary>
public class ItemRow
{
    public required string Name { get; set; }
    public double Anzahl { get; set; }
    public double Preis { get; set; }
    public double Gesamt => Anzahl * Preis;

    public void Nullen()
    {
        Anzahl = 0;
        Preis = 0;
    }
}

/// <summary>Zeile, deren Anzahl mit einer Einheit skaliert (pro Char, pro Team, pro Rolle …).</summary>
public class FaktorRow
{
    public required string Name { get; set; }
    public double Faktor { get; set; }
    public double Preis { get; set; }
    public double Anzahl(double einheiten) => Faktor * einheiten;
    public double Gesamt(double einheiten) => Faktor * einheiten * Preis;
}

// ---------------------------------------------------------------- IC

public class IcRechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public double AnzahlChars { get; set; } = 15;
    public double BargoldProChar { get; set; } = 225_000;

    public List<FaktorRow> Items { get; set; } =
    [
        new() { Name = "Federn", Faktor = 20, Preis = 26_000 },
        new() { Name = "Vollmonde", Faktor = 5, Preis = 21_000 },
        new() { Name = "ATK-Tränke", Faktor = 6, Preis = 6_500 },
        new() { Name = "DEF-Tränke", Faktor = 3, Preis = 6_000 },
        new() { Name = "Energietränke", Faktor = 3, Preis = 1_200 },
        new() { Name = "Riesige", Faktor = 40, Preis = 2_100 },
    ];

    public double Bargold => BargoldProChar * AnzahlChars * F;
    public double Gewinn => Items.Sum(i => i.Gesamt(AnzahlChars)) + Bargold;
    public double GewinnProChar => AnzahlChars > 0 ? Gewinn / AnzahlChars : 0;

    // Fulli-Herstellung (Ansatz für 10 Fullis)
    public List<ItemRow> FulliKosten { get; set; } =
    [
        new() { Name = "Riesige", Anzahl = 30, Preis = 0 },
        new() { Name = "Makrelen", Anzahl = 3, Preis = 0 },
        new() { Name = "Zaubertrank", Anzahl = 20, Preis = 100 },
        new() { Name = "Glasflasche", Anzahl = 20, Preis = 500 },
    ];
    public double FulliVerkaufspreis { get; set; }
    public double FulliKostenGesamt => FulliKosten.Sum(r => r.Gesamt);
    public double FulliErloes => 10 * FulliVerkaufspreis;
    public double FulliGewinnProZehn => FulliErloes - FulliKostenGesamt;

    public void Nullen()
    {
        AnzahlChars = 0;
        // BargoldProChar bleibt als Vorgabe stehen (fester Spielwert).
        Items.ForEach(r => r.Preis = 0);
        FulliKosten.ForEach(r => r.Nullen());
        FulliVerkaufspreis = 0;
    }
}

// ---------------------------------------------------------------- Eleraids

public class EleraidRechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public double Teams { get; set; } = 4;

    public List<FaktorRow> GlaceKosten { get; set; } =
    [
        new() { Name = "Eisblumen", Faktor = 100, Preis = 0 },
        new() { Name = "Eiswürfel", Faktor = 100, Preis = 0 },
        new() { Name = "Saat", Faktor = 100, Preis = 0 },
        new() { Name = "Siegel", Faktor = 5, Preis = 1_600_000 },
    ];

    // Draco braucht ein eigenes Siegel: anderes Item als bei Glace und aus
    // anderen Zutaten hergestellt (Gillion statt Eisblumen und Eiswürfel).
    public List<FaktorRow> DracoKosten { get; set; } =
    [
        new() { Name = "Gillion", Faktor = 100, Preis = 0 },
        new() { Name = "Saat", Faktor = 100, Preis = 0 },
        new() { Name = "Siegel", Faktor = 5, Preis = 0 },
    ];

    public double RaidsProBoss => Teams * 5;
    public double BoxenProBoss => RaidsProBoss * 15;
    public double RaidsGesamt => RaidsProBoss * 2;

    /// <summary>Name der Siegel-Zeile in beiden Kostenlisten.</summary>
    private const string SiegelZeile = "Siegel";

    // Ein Siegel wird entweder gekauft oder aus den Zutaten selbst hergestellt –
    // niemals beides. Vorher summierte der Rechner alle Zeilen und zählte damit
    // den doppelten Weg. Die Vorgabe ist Kaufen, weil nur dafür ein Preis hinterlegt ist.
    /// <summary>true = Glace-Siegel kaufen, false = aus Eisblumen, Eiswürfeln und Saat herstellen.</summary>
    public bool GlaceSiegelKaufen { get; set; } = true;

    /// <summary>true = Draco-Siegel kaufen, false = aus Gillion und Saat herstellen.</summary>
    public bool DracoSiegelKaufen { get; set; } = true;

    /// <summary>Zählt diese Zeile beim gewählten Weg mit?</summary>
    public static bool ZeileZaehlt(FaktorRow zeile, bool kaufen) => (zeile.Name == SiegelZeile) == kaufen;

    private double BossKosten(List<FaktorRow> zeilen, bool kaufen) =>
        zeilen.Where(r => ZeileZaehlt(r, kaufen)).Sum(r => r.Gesamt(Teams));

    public double GlaceGesamt => BossKosten(GlaceKosten, GlaceSiegelKaufen);
    public double DracoGesamt => BossKosten(DracoKosten, DracoSiegelKaufen);

    public double Gesamtkosten => GlaceGesamt + DracoGesamt;

    // Drops während der Raids (Glace + Draco zusammen)
    /// <summary>Name der Bargold-Zeile – nur sie bekommt die Gold-Rate des Charakters.</summary>
    public const string GoldZeile = "Gold-Drops";

    public List<FaktorRow> Drops { get; set; } =
    [
        new() { Name = GoldZeile, Faktor = 8, Preis = 20_000 },
        new() { Name = "Vios", Faktor = 3, Preis = 20_000 },
    ];

    /// <summary>
    /// Erlös einer Drop-Zeile. Bargold zählt mit der Gold-Rate des Charakters,
    /// Item-Drops (z. B. Vios) nicht – die Rate wirkt im Spiel nur auf Gold.
    /// </summary>
    public double DropErloes(FaktorRow zeile) =>
        zeile.Gesamt(RaidsGesamt) * (zeile.Name == GoldZeile ? F : 1);

    public double DropsGesamt => Drops.Sum(DropErloes);
    public double KostenVerrechnet => Gesamtkosten - DropsGesamt;

    // Variante A: Boxen öffnen und Inhalt verkaufen
    public List<ItemRow> BoxenInhalt { get; set; } =
    [
        new() { Name = "Blaue", Anzahl = 177, Preis = 100_000 },
        new() { Name = "Engelsfedern", Anzahl = 580, Preis = 8_000 },
        new() { Name = "Vollmonde", Anzahl = 250, Preis = 37_000 },
        new() { Name = "EXP-Pots", Anzahl = 44, Preis = 185_000 },
    ];
    public double InhaltErloes => BoxenInhalt.Sum(r => r.Gesamt);
    public double GewinnOeffnen => InhaltErloes - KostenVerrechnet;

    // Variante B: Boxen direkt verkaufen
    public double BoxenpreisGlace { get; set; } = 150_000;
    public double BoxenpreisDraco { get; set; } = 70_000;
    public double VerkaufGlace => BoxenProBoss * BoxenpreisGlace;
    public double VerkaufDraco => BoxenProBoss * BoxenpreisDraco;
    public double GewinnVerkaufen => VerkaufGlace + VerkaufDraco - KostenVerrechnet;

    public double BesterGewinn => Math.Max(GewinnOeffnen, GewinnVerkaufen);

    public void Nullen()
    {
        Teams = 0;
        GlaceKosten.ForEach(r => r.Preis = 0);
        DracoKosten.ForEach(r => r.Preis = 0);
        // Der Gold-Drop-Wert bleibt als Vorgabe stehen (fester Spielwert).
        Drops.ForEach(r => { r.Faktor = 0; if (r.Name != GoldZeile) { r.Preis = 0; } });
        BoxenInhalt.ForEach(r => r.Nullen());
        BoxenpreisGlace = 0;
        BoxenpreisDraco = 0;
    }
}

// ---------------------------------------------------------------- Act 5 (Kertos)

public class Act5Rechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public double Raids { get; set; } = 26;
    public double SiegelPreis { get; set; } = 600_000;
    public double DropsProRaid { get; set; } = 10;
    public double GoldProDrop { get; set; } = 20_000;
    /// <summary>true = selbst gezähltes Gesamt-Gold nutzen statt Drops × Gold pro Drop.</summary>
    public bool GoldDirekt { get; set; }
    public double GoldGehoben { get; set; }
    public double BargoldProRaid => DropsProRaid * GoldProDrop;
    public double NettoBargold => GoldDirekt
        ? GoldGehoben * F - SiegelPreis * Raids
        : (BargoldProRaid * F - SiegelPreis) * Raids;

    // Ausbeute einer Stunde, von Fettbong gezählt. Gemessen wurde während eines
    // Events mit 50 % mehr Box-Drops – nicht mit doppelter Menge. Die Rohwerte
    // (734 Boxen, 1136 intakte und 880 kaputte Diamanten) sind darum durch 1,5
    // geteilt, damit die Vorgabe den Normalfall abbildet und nicht das Event.
    // Beide Varianten beschreiben dieselbe Ausbeute: Variante A öffnet die 489
    // Boxen, Variante B verkauft sie – rund 2,75 Diamanten je Box.
    public List<ItemRow> Oeffnen { get; set; } =
    [
        new() { Name = "Intakte Items", Anzahl = 757, Preis = 50_000 },
        new() { Name = "Kaputte Items", Anzahl = 587, Preis = 10_000 },
    ];
    public double OeffnenErloes => Oeffnen.Sum(r => r.Gesamt);
    public double GewinnOeffnen => NettoBargold + OeffnenErloes;

    public double BoxenAnzahl { get; set; } = 489;
    public double Boxenpreis { get; set; } = 100_000;
    public double VerkaufErloes => BoxenAnzahl * Boxenpreis;
    public double GewinnVerkaufen => NettoBargold + VerkaufErloes;

    public double BesterGewinn => Math.Max(GewinnOeffnen, GewinnVerkaufen);

    public void Nullen()
    {
        Raids = 0;
        SiegelPreis = 0;
        DropsProRaid = 0;
        GoldDirekt = false;
        GoldGehoben = 0;
        // GoldProDrop und die NPC-Preise der Items bleiben als Vorgabe stehen (Spiel-Konstanten).
        Oeffnen.ForEach(r => r.Anzahl = 0);
        BoxenAnzahl = 0;
        Boxenpreis = 0;
    }
}

// ---------------------------------------------------------------- Act 6 (Erenia / Zenas / Fernon)

public class Act6Boss
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public required string Name { get; set; }
    public double Raids { get; set; }
    public double SiegelPreis { get; set; }
    public double DropsProRaid { get; set; }
    public double GoldProDrop { get; set; }
    /// <summary>true = selbst gezähltes Gesamt-Gold nutzen statt Drops × Gold pro Drop.</summary>
    public bool GoldDirekt { get; set; }
    public double GoldGehoben { get; set; }
    public double BoxenProRaid { get; set; }
    public double Boxenwert { get; set; }

    public double GoldProRaid => DropsProRaid * GoldProDrop;
    public double GoldgewinnProRaid => GoldProRaid * F - SiegelPreis;
    public double GoldGesamt => GoldDirekt
        ? GoldGehoben * F - SiegelPreis * Raids
        : GoldgewinnProRaid * Raids;
    public double BoxenGesamt => BoxenProRaid * Boxenwert * Raids;
    public double Gesamt => GoldGesamt + BoxenGesamt;

    public void Nullen()
    {
        Raids = 0;
        SiegelPreis = 0;
        DropsProRaid = 0;
        GoldDirekt = false;
        GoldGehoben = 0;
        // GoldProDrop bleibt als Vorgabe stehen (Spiel-Konstante).
        BoxenProRaid = 0;
        Boxenwert = 0;
    }
}

public class Act6Rechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public Act6Boss Erenia { get; set; } = new()
    {
        Name = "Erenia", Raids = 19, SiegelPreis = 700_000,
        DropsProRaid = 75, GoldProDrop = 5_000, BoxenProRaid = 15, Boxenwert = 180_000,
    };

    public Act6Boss Zenas { get; set; } = new()
    {
        Name = "Zenas", Raids = 17, SiegelPreis = 700_000,
        DropsProRaid = 75, GoldProDrop = 5_000, BoxenProRaid = 20, Boxenwert = 200_000,
    };

    // Fernon
    public double FernonRaids { get; set; } = 36;
    public double FernonSiegel { get; set; } = 1_650_000;
    public double FernonGoldGehoben { get; set; } = 92_000_000;
    public bool HeberDabei { get; set; }
    public double GoldHeber { get; set; } = 8_000_000;
    public double HeberFullis { get; set; }
    public double HeberFulliPreis { get; set; } = 30_000;

    public double FernonSiegelkosten => FernonSiegel * FernonRaids;
    public double FernonGoldgewinn => FernonGoldGehoben * F - FernonSiegelkosten;
    public double FernonGoldProRaid => FernonRaids > 0 ? FernonGoldGehoben * F / FernonRaids : 0;
    public double HeberKosten => GoldHeber + HeberFullis * HeberFulliPreis;
    public double FernonGewinn => HeberDabei ? FernonGoldgewinn - HeberKosten : FernonGoldgewinn;

    public double Gesamt => Erenia.Gesamt + Zenas.Gesamt + FernonGewinn;

    public void Nullen()
    {
        Erenia.Nullen();
        Zenas.Nullen();
        FernonRaids = 0;
        FernonSiegel = 0;
        FernonGoldGehoben = 0;
        HeberDabei = false;
        GoldHeber = 0;
        HeberFullis = 0;
        HeberFulliPreis = 0;
    }
}

// ---------------------------------------------------------------- Act 7 (Carno / Kirolla / Belial)

public class Act7Rechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    // Carno
    public double CarnoRaids { get; set; } = 38;
    public double CarnoSiegel { get; set; } = 150_000;
    public double CarnoDropsProRaid { get; set; } = 10;
    public double CarnoGoldProDrop { get; set; } = 5_000;
    public bool CarnoGoldDirekt { get; set; }
    public double CarnoGoldGehoben { get; set; }
    public double CarnoBargoldProRaid => CarnoDropsProRaid * CarnoGoldProDrop;
    public List<ItemRow> CarnoKisten { get; set; } =
    [
        new() { Name = "Kisten Low Rare", Anzahl = 293, Preis = 137_000 },
        new() { Name = "Kisten High Rare", Anzahl = 15, Preis = 795_000 },
    ];
    public double CarnoGoldgewinn => CarnoGoldDirekt
        ? CarnoGoldGehoben * F - CarnoSiegel * CarnoRaids
        : (CarnoBargoldProRaid * F - CarnoSiegel) * CarnoRaids;
    public double CarnoGewinn => CarnoGoldgewinn + CarnoKisten.Sum(r => r.Gesamt);

    // Kirolla
    public double KirollaRaids { get; set; } = 48;
    public double KirollaSiegel { get; set; } = 415_000;
    public double KirollaDropsProRaid { get; set; } = 10;
    public double KirollaGoldProDrop { get; set; } = 5_000;
    public bool KirollaGoldDirekt { get; set; }
    public double KirollaGoldGehoben { get; set; }
    public double KirollaGoldProRaid => KirollaDropsProRaid * KirollaGoldProDrop;
    public ItemRow KirollaObsidiane { get; set; } = new() { Name = "Obsidiane", Anzahl = 350, Preis = 13_000 };
    public List<ItemRow> KirollaBoxen { get; set; } =
    [
        new() { Name = "Kisten High Rare", Anzahl = 43, Preis = 580_000 },
        new() { Name = "Kisten Low Rare", Anzahl = 1_028, Preis = 50_000 },
    ];
    public double KirollaGoldgewinn => KirollaGoldDirekt
        ? KirollaGoldGehoben * F - KirollaSiegel * KirollaRaids
        : (KirollaGoldProRaid * F - KirollaSiegel) * KirollaRaids;
    public double KirollaBoxenErloes => KirollaBoxen.Sum(r => r.Gesamt);
    public double KirollaGewinn => KirollaGoldgewinn + KirollaObsidiane.Gesamt + KirollaBoxenErloes;

    // Belial
    public double BelialRaids { get; set; } = 36;
    public double BelialSiegel { get; set; } = 280_000;
    public double BelialDropsProRaid { get; set; } = 10;
    public double BelialGoldProDrop { get; set; } = 37_000;
    public bool BelialGoldDirekt { get; set; }
    public double BelialGoldGehoben { get; set; }
    public double BelialBargoldProRaid => BelialDropsProRaid * BelialGoldProDrop;
    public bool BelialOeffnen { get; set; }
    public List<ItemRow> BelialOeffnenItems { get; set; } =
    [
        new() { Name = "MSG" },
        new() { Name = "Obsidiane" },
        new() { Name = "Runen-Pulver" },
        new() { Name = "Jade" },
        new() { Name = "Orktränke" },
        new() { Name = "EQ", Preis = 50_000 },
    ];
    public List<ItemRow> BelialVerkauf { get; set; } =
    [
        new() { Name = "Kisten Low Rare", Anzahl = 698, Preis = 50_000 },
        new() { Name = "Kisten High Rare", Anzahl = 28, Preis = 1_000_000 },
    ];
    public double BelialGoldgewinn => BelialGoldDirekt
        ? BelialGoldGehoben * F - BelialSiegel * BelialRaids
        : (BelialBargoldProRaid * F - BelialSiegel) * BelialRaids;
    public double BelialBoxenErloes =>
        BelialOeffnen ? BelialOeffnenItems.Sum(r => r.Gesamt) : BelialVerkauf.Sum(r => r.Gesamt);
    public double BelialGewinn => BelialGoldgewinn + BelialBoxenErloes;

    public double Gesamt => CarnoGewinn + KirollaGewinn + BelialGewinn;

    public void Nullen()
    {
        // Die Gold-pro-Drop-Werte bleiben als Vorgabe stehen (Spiel-Konstanten).
        CarnoRaids = 0;
        CarnoSiegel = 0;
        CarnoDropsProRaid = 0;
        CarnoGoldDirekt = false;
        CarnoGoldGehoben = 0;
        CarnoKisten.ForEach(r => r.Nullen());
        KirollaRaids = 0;
        KirollaSiegel = 0;
        KirollaDropsProRaid = 0;
        KirollaGoldDirekt = false;
        KirollaGoldGehoben = 0;
        KirollaObsidiane.Nullen();
        KirollaBoxen.ForEach(r => r.Nullen());
        BelialRaids = 0;
        BelialSiegel = 0;
        BelialDropsProRaid = 0;
        BelialGoldDirekt = false;
        BelialGoldGehoben = 0;
        BelialOeffnen = false;
        BelialOeffnenItems.ForEach(r => r.Nullen());
        BelialVerkauf.ForEach(r => r.Nullen());
    }
}

// ---------------------------------------------------------------- Act 8 (Alzanor)

public class Act8Rechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public double Raids { get; set; } = 1;
    public double SiegelPreis { get; set; }
    public double DropsProRaid { get; set; } = 1;
    public double GoldProDrop { get; set; } = 30_000;
    /// <summary>true = selbst gezähltes Gesamt-Gold nutzen statt Drops × Gold pro Drop.</summary>
    public bool GoldDirekt { get; set; }
    public double GoldGehoben { get; set; }
    // Kisten zählen für den gesamten Run, nicht pro Raid
    public double BoxenAnzahl { get; set; } = 146;
    public double Boxenwert { get; set; } = 450_000;

    // Erweitert: High-Rare-Kisten (gesamter Run)
    public double HighRareAnzahl { get; set; } = 16;
    public double HighRarePreis { get; set; } = 1_700_000;

    public double GoldProRaid => DropsProRaid * GoldProDrop;
    public double GoldgewinnProRaid => GoldProRaid * F - SiegelPreis;
    public double GoldGesamt => GoldDirekt
        ? GoldGehoben * F - SiegelPreis * Raids
        : GoldgewinnProRaid * Raids;
    public double BoxenGesamt => BoxenAnzahl * Boxenwert;
    public double HighRareGesamt => HighRareAnzahl * HighRarePreis;
    public double Gesamt => GoldGesamt + BoxenGesamt + HighRareGesamt;

    public void Nullen()
    {
        Raids = 0;
        SiegelPreis = 0;
        DropsProRaid = 0;
        GoldDirekt = false;
        GoldGehoben = 0;
        // GoldProDrop bleibt als Vorgabe stehen (fester Spielwert).
        BoxenAnzahl = 0;
        Boxenwert = 0;
        HighRareAnzahl = 0;
        HighRarePreis = 0;
    }
}

// ---------------------------------------------------------------- Act 9 (Pollu)

public class Act9Rechner
{
    [JsonIgnore] public Charakter? Charakter { get; set; }
    private double F => Charakter?.Faktor ?? 1;

    public double Raids { get; set; } = 1;
    public double SiegelPreis { get; set; }
    public double DropsProRaid { get; set; } = 1;
    public double GoldProDrop { get; set; } = 52_000;
    /// <summary>true = selbst gezähltes Gesamt-Gold nutzen statt Drops × Gold pro Drop.</summary>
    public bool GoldDirekt { get; set; }
    public double GoldGehoben { get; set; }
    // Kisten und Vollmonde zählen für den gesamten Run, nicht pro Raid
    public double BoxenAnzahl { get; set; } = 155;
    public double Boxenwert { get; set; } = 777_000;
    public double VollmondeAnzahl { get; set; } = 190;
    public double VollmondPreis { get; set; } = 22_000;

    // Erweitert: High-Rare-Kisten (gesamter Run)
    public double HighRareAnzahl { get; set; } = 13;
    public double HighRarePreis { get; set; } = 1_800_000;

    public double GoldProRaid => DropsProRaid * GoldProDrop;
    public double GoldgewinnProRaid => GoldProRaid * F - SiegelPreis;
    public double GoldGesamt => GoldDirekt
        ? GoldGehoben * F - SiegelPreis * Raids
        : GoldgewinnProRaid * Raids;
    public double BoxenGesamt => BoxenAnzahl * Boxenwert;
    public double VollmondeGesamt => VollmondeAnzahl * VollmondPreis;
    public double HighRareGesamt => HighRareAnzahl * HighRarePreis;
    public double Gesamt => GoldGesamt + BoxenGesamt + VollmondeGesamt + HighRareGesamt;

    public void Nullen()
    {
        Raids = 0;
        SiegelPreis = 0;
        DropsProRaid = 0;
        GoldDirekt = false;
        GoldGehoben = 0;
        // GoldProDrop bleibt als Vorgabe stehen (Spiel-Konstante).
        BoxenAnzahl = 0;
        Boxenwert = 0;
        VollmondeAnzahl = 0;
        VollmondPreis = 0;
        HighRareAnzahl = 0;
        HighRarePreis = 0;
    }
}

// ---------------------------------------------------------------- Gesamtzustand

/// <summary>
/// Pro Nutzer-Sitzung gehaltener Zustand aller Rechner (Scoped Service).
/// Startet leer; die Vorlage „Fettbong“ lädt die Werte aus „Nostale Gewinnrechner.ods“.
/// </summary>
public class RechnerState
{
    public IcRechner Ic { get; private set; } = new();
    public EleraidRechner Eleraids { get; private set; } = new();
    public Act5Rechner Act5 { get; private set; } = new();
    public Act6Rechner Act6 { get; private set; } = new();
    public Act7Rechner Act7 { get; private set; } = new();
    public Act8Rechner Act8 { get; private set; } = new();
    public Act9Rechner Act9 { get; private set; } = new();
    public Charakter Charakter { get; private set; } = new();
    public MatPreise Mats { get; private set; } = new();

    /// <summary>Wird gefeuert, wenn eine Vorlage geladen oder alles genullt wurde.</summary>
    public event Action? OnChange;

    public string AktiveVorlage { get; private set; } = "Leer";

    /// <summary>
    /// Stand der hinterlegten Preise (Datum als Zahl, muss nur monoton steigen).
    /// Wird er erhöht, bekommen auch bereits gespeicherte Stände beim Laden die
    /// neuen Preise – siehe <see cref="ImportJson"/>.
    /// </summary>
    public const int PreisStand = 20_260_816;

    public RechnerState()
    {
        NullenKern();
        Verkabeln();
    }

    /// <summary>Meldet eine Änderung (z. B. an der Gold-Rate), damit offene Seiten neu rechnen.</summary>
    public void MeldeAenderung() => OnChange?.Invoke();

    /// <summary>
    /// Setzt einen globalen Materialpreis und überschreibt die passenden
    /// Preisfelder in allen Rechnern (Zuordnung über die Zeilennamen).
    /// </summary>
    public void SetzeMat(string mat, double preis)
    {
        switch (mat)
        {
            case "vollmond":
                Mats.Vollmond = preis;
                FaktorPreis(Ic.Items, "Vollmonde", preis);
                ItemPreis(Eleraids.BoxenInhalt, "Vollmonde", preis);
                Act9.VollmondPreis = preis;
                break;
            case "engelsfeder":
                Mats.Engelsfeder = preis;
                FaktorPreis(Ic.Items, "Federn", preis);
                ItemPreis(Eleraids.BoxenInhalt, "Engelsfedern", preis);
                break;
            case "exppot":
                Mats.ExpPot = preis;
                ItemPreis(Eleraids.BoxenInhalt, "EXP-Pots", preis);
                break;
            case "atkpot":
                Mats.AtkPot = preis;
                FaktorPreis(Ic.Items, "ATK-Tränke", preis);
                break;
            case "defpot":
                Mats.DefPot = preis;
                FaktorPreis(Ic.Items, "DEF-Tränke", preis);
                break;
            case "energiepot":
                Mats.EnergiePot = preis;
                FaktorPreis(Ic.Items, "Energietränke", preis);
                break;
            case "riesige":
                Mats.Riesige = preis;
                FaktorPreis(Ic.Items, "Riesige", preis);
                ItemPreis(Ic.FulliKosten, "Riesige", preis);
                break;
            case "fulli":
                Mats.Fulli = preis;
                Ic.FulliVerkaufspreis = preis;
                Act6.HeberFulliPreis = preis;
                break;
            case "obsidian":
                Mats.Obsidian = preis;
                Act7.KirollaObsidiane.Preis = preis;
                ItemPreis(Act7.BelialOeffnenItems, "Obsidiane", preis);
                break;
            // Aufwertungs-Materialien: wirken direkt über Mats in den Fahrplänen der Analyse.
            case "cella":
                Mats.Cella = preis;
                break;
            case "seelenstein":
                Mats.Seelenstein = preis;
                break;
            case "seelensteinvollkommen":
                Mats.SeelensteinVollkommen = preis;
                break;
            case "drachenedelstein":
                Mats.Drachenedelstein = preis;
                break;
            case "glaenzendeseele":
                Mats.GlaenzendeSeele = preis;
                break;
            case "seelehoehere":
                Mats.SeeleHoehere = preis;
                break;
            // Runen-Materialien: wirken direkt über Mats im Runen-Fahrplan der Analyse.
            case "morizioobsidian":
                Mats.MorizioObsidian = preis;
                break;
            case "loarunenpulver":
                Mats.LoaRunenpulver = preis;
                ItemPreis(Act7.BelialOeffnenItems, "Runen-Pulver", preis);
                break;
            case "titanbarren":
                Mats.Titanbarren = preis;
                break;
            // Schutzrollen: wirken über Mats in den Aufwertungs-Fahrplänen.
            case "rolleausruestung":
                Mats.RolleAusruestung = preis;
                break;
            case "rolleausruestunggolden":
                Mats.RolleAusruestungGolden = preis;
                break;
            case "rollespniedrig":
                Mats.RolleSpNiedrig = preis;
                break;
            case "rollespniedrigpremium":
                Mats.RolleSpNiedrigPremium = preis;
                break;
            case "rollesphoch":
                Mats.RolleSpHoch = preis;
                break;
            case "rollesphochpremium":
                Mats.RolleSpHochPremium = preis;
                break;
            case "rolledrachen":
                Mats.RolleDrachen = preis;
                break;
            case "rolledrachenpremium":
                Mats.RolleDrachenPremium = preis;
                break;

            // ---- Raid-Zutaten ----
            case "eisblume":
                Mats.Eisblume = preis;
                FaktorPreis(Eleraids.GlaceKosten, "Eisblumen", preis);
                break;
            case "eiswuerfel":
                Mats.Eiswuerfel = preis;
                FaktorPreis(Eleraids.GlaceKosten, "Eiswürfel", preis);
                break;
            case "saat":
                Mats.Saat = preis;
                FaktorPreis(Eleraids.GlaceKosten, "Saat", preis);
                FaktorPreis(Eleraids.DracoKosten, "Saat", preis);
                break;
            case "gillion":
                Mats.Gillion = preis;
                FaktorPreis(Eleraids.DracoKosten, "Gillion", preis);
                break;
            case "siegelglace":
                Mats.SiegelGlace = preis;
                FaktorPreis(Eleraids.GlaceKosten, "Siegel", preis);
                break;
            case "siegeldraco":
                Mats.SiegelDraco = preis;
                FaktorPreis(Eleraids.DracoKosten, "Siegel", preis);
                break;

            // ---- Boss-Siegel (nur Drop oder Kauf) ----
            case "siegelkertos":
                Mats.SiegelKertos = preis;
                Act5.SiegelPreis = preis;
                break;
            case "siegelerenia":
                Mats.SiegelErenia = preis;
                Act6.Erenia.SiegelPreis = preis;
                break;
            case "siegelzenas":
                Mats.SiegelZenas = preis;
                Act6.Zenas.SiegelPreis = preis;
                break;
            case "siegelfernon":
                Mats.SiegelFernon = preis;
                Act6.FernonSiegel = preis;
                break;
            case "siegelcarno":
                Mats.SiegelCarno = preis;
                Act7.CarnoSiegel = preis;
                break;
            case "siegelkirolla":
                Mats.SiegelKirolla = preis;
                Act7.KirollaSiegel = preis;
                break;
            case "siegelbelial":
                Mats.SiegelBelial = preis;
                Act7.BelialSiegel = preis;
                break;
            case "siegelalzanor":
                Mats.SiegelAlzanor = preis;
                Act8.SiegelPreis = preis;
                break;
            case "siegelpollu":
                Mats.SiegelPollu = preis;
                Act9.SiegelPreis = preis;
                break;

            // ---- Truhen und Kisten ----
            case "blauesteine":
                Mats.BlaueSteine = preis;
                ItemPreis(Eleraids.BoxenInhalt, "Blaue", preis);
                break;
            case "truheglace":
                Mats.TruheGlace = preis;
                Eleraids.BoxenpreisGlace = preis;
                break;
            case "truhedraco":
                Mats.TruheDraco = preis;
                Eleraids.BoxenpreisDraco = preis;
                break;
            case "truheact5":
                Mats.TruheAct5 = preis;
                Act5.Boxenpreis = preis;
                break;
            case "act5intakt":
                Mats.Act5Intakt = preis;
                ItemPreis(Act5.Oeffnen, "Intakte Items", preis);
                break;
            case "act5kaputt":
                Mats.Act5Kaputt = preis;
                ItemPreis(Act5.Oeffnen, "Kaputte Items", preis);
                break;
            case "truheerenia":
                Mats.TruheErenia = preis;
                Act6.Erenia.Boxenwert = preis;
                break;
            case "truhezenas":
                Mats.TruheZenas = preis;
                Act6.Zenas.Boxenwert = preis;
                break;
            case "truhecarnolow":
                Mats.TruheCarnoLow = preis;
                ItemPreis(Act7.CarnoKisten, "Kisten Low Rare", preis);
                break;
            case "truhecarnohigh":
                Mats.TruheCarnoHigh = preis;
                ItemPreis(Act7.CarnoKisten, "Kisten High Rare", preis);
                break;
            case "truhekirollalow":
                Mats.TruheKirollaLow = preis;
                ItemPreis(Act7.KirollaBoxen, "Kisten Low Rare", preis);
                break;
            case "truhekirollahigh":
                Mats.TruheKirollaHigh = preis;
                ItemPreis(Act7.KirollaBoxen, "Kisten High Rare", preis);
                break;
            case "truhebeliallow":
                Mats.TruheBelialLow = preis;
                ItemPreis(Act7.BelialVerkauf, "Kisten Low Rare", preis);
                break;
            case "truhebelialhigh":
                Mats.TruheBelialHigh = preis;
                ItemPreis(Act7.BelialVerkauf, "Kisten High Rare", preis);
                break;
            case "truheact8":
                Mats.TruheAct8 = preis;
                Act8.Boxenwert = preis;
                break;
            case "truheact8high":
                Mats.TruheAct8High = preis;
                Act8.HighRarePreis = preis;
                break;
            case "truheact9":
                Mats.TruheAct9 = preis;
                Act9.Boxenwert = preis;
                break;
            case "truheact9high":
                Mats.TruheAct9High = preis;
                Act9.HighRarePreis = preis;
                break;
        }

        OnChange?.Invoke();
    }

    /// <summary>
    /// Schreibt alle hinterlegten Materialpreise in sämtliche Rechner durch. Die
    /// einzelnen Rechner bringen eigene Vorgabewerte für dieselben Materialien mit
    /// (die IC-Feder war z. B. mit 26.000 hinterlegt, der Eleraid-Vollmond mit
    /// 37.000) – nach diesem Aufruf gilt überall der Preis von der Mats-Seite.
    /// </summary>
    public void MatsAnwenden()
    {
        foreach (var (schluessel, preis) in Mats.Alle)
        {
            SetzeMat(schluessel, preis);
        }
    }

    private static void FaktorPreis(List<FaktorRow> zeilen, string name, double preis)
    {
        var zeile = zeilen.FirstOrDefault(z => z.Name == name);
        if (zeile is not null)
        {
            zeile.Preis = preis;
        }
    }

    private static void ItemPreis(List<ItemRow> zeilen, string name, double preis)
    {
        var zeile = zeilen.FirstOrDefault(z => z.Name == name);
        if (zeile is not null)
        {
            zeile.Preis = preis;
        }
    }

    /// <summary>Verbindet den Charakter (Gold-Rate) mit allen Rechnern.</summary>
    private void Verkabeln()
    {
        Ic.Charakter = Charakter;
        Eleraids.Charakter = Charakter;
        Act5.Charakter = Charakter;
        Act6.Charakter = Charakter;
        Act6.Erenia.Charakter = Charakter;
        Act6.Zenas.Charakter = Charakter;
        Act7.Charakter = Charakter;
        Act8.Charakter = Charakter;
        Act9.Charakter = Charakter;
    }

    /// <summary>Setzt alle Eingaben auf 0.</summary>
    public void Nullen()
    {
        NullenKern();
        OnChange?.Invoke();
    }

    /// <summary>
    /// Lädt Fettbongs Werte aus seiner Tabelle in alle Rechner.
    /// Der eigene Charakter (Gold-Rate) bleibt dabei unangetastet.
    /// </summary>
    public void LadeFettbong()
    {
        Ic = new();
        Eleraids = new();
        Act5 = new();
        Act6 = new();
        Act7 = new();
        Act8 = new();
        Act9 = new();
        Verkabeln();
        MatsAnwenden();
        AktiveVorlage = "Fettbong";
        OnChange?.Invoke();
    }

    private void NullenKern()
    {
        Ic.Nullen();
        Eleraids.Nullen();
        Act5.Nullen();
        Act6.Nullen();
        Act7.Nullen();
        Act8.Nullen();
        Act9.Nullen();
        Charakter.GoldRate = 0;

        // Marktpreise sind keine Eingaben des Nutzers, sondern hinterlegte
        // Bazar-Durchschnitte. „Leer“ setzt sie darum auf die Standardwerte zurück
        // statt auf 0 – sonst müsste man sie nach jedem Zurücksetzen neu eintragen.
        Mats = new MatPreise();
        MatsAnwenden();
        AktiveVorlage = "Leer";
    }

    /// <summary>Alle aktuellen Werte als JSON für den Datei-Export.</summary>
    public string ExportJson() =>
        JsonSerializer.Serialize(new ExportDaten
        {
            PreisStand = PreisStand,
            Ic = Ic,
            Eleraids = Eleraids,
            Act5 = Act5,
            Act6 = Act6,
            Act7 = Act7,
            Act8 = Act8,
            Act9 = Act9,
            Charakter = Charakter,
            Mats = Mats,
        }, ExportJsonContext.Default.ExportDaten);

    /// <summary>Liest eine exportierte JSON-Datei wieder ein. Liefert false bei ungültigem Inhalt.</summary>
    public bool ImportJson(string json, string vorlage = "Import")
    {
        try
        {
            var daten = JsonSerializer.Deserialize(json, ExportJsonContext.Default.ExportDaten);
            if (daten is null)
            {
                return false;
            }

            Ic = daten.Ic ?? Ic;
            Eleraids = daten.Eleraids ?? Eleraids;
            Act5 = daten.Act5 ?? Act5;
            Act6 = daten.Act6 ?? Act6;
            Act7 = daten.Act7 ?? Act7;
            Act8 = daten.Act8 ?? Act8;
            Act9 = daten.Act9 ?? Act9;
            Charakter = daten.Charakter ?? Charakter;
            Mats = daten.Mats ?? Mats;
            Verkabeln();

            // Stände von vor der letzten Preispflege bekommen die aktuellen Preise –
            // sonst blieben in Speicherständen die alten Werte stehen.
            if (daten.PreisStand < PreisStand)
            {
                Mats = new MatPreise();
                MatsAnwenden();
            }
            AktiveVorlage = vorlage;
            OnChange?.Invoke();
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public record Aktivitaet(string Name, string Detail, double Gewinn, string Href);

    public List<Aktivitaet> Aktivitaeten =>
    [
        new("IC", $"{Fmt.Gold(Ic.AnzahlChars)} Chars · ×4, da ein Run nur 15 Min dauert", Ic.Gewinn * 4, "ic"),
        new("Eleraids", $"{Fmt.Gold(Eleraids.RaidsGesamt)} Raids, beste Variante", Eleraids.BesterGewinn, "eleraids"),
        new("Act 5 · Kertos", $"{Fmt.Gold(Act5.Raids)} Raids, beste Variante", Act5.BesterGewinn, "act5"),
        new("Act 6 · Erenia", $"{Fmt.Gold(Act6.Erenia.Raids)} Raids", Act6.Erenia.Gesamt, "act6/erenia"),
        new("Act 6 · Zenas", $"{Fmt.Gold(Act6.Zenas.Raids)} Raids", Act6.Zenas.Gesamt, "act6/zenas"),
        new("Act 6 · Fernon", $"{Fmt.Gold(Act6.FernonRaids)} Raids", Act6.FernonGewinn, "act6/fernon"),
        new("Act 7 · Carno", $"{Fmt.Gold(Act7.CarnoRaids)} Raids", Act7.CarnoGewinn, "act7/carno"),
        new("Act 7 · Kirolla", $"{Fmt.Gold(Act7.KirollaRaids)} Raids", Act7.KirollaGewinn, "act7/kirolla"),
        new("Act 7 · Belial", $"{Fmt.Gold(Act7.BelialRaids)} Raids", Act7.BelialGewinn, "act7/belial"),
        new("Act 8 · Alzanor", $"{Fmt.Gold(Act8.Raids)} Raids", Act8.Gesamt, "act8"),
        new("Act 9 · Pollu", $"{Fmt.Gold(Act9.Raids)} Raids", Act9.Gesamt, "act9"),
    ];

    public double GesamtGewinn => Aktivitaeten.Sum(a => a.Gewinn);
}

// ---------------------------------------------------------------- Export / Import

/// <summary>Dateiformat für Export/Import der Nutzerwerte.</summary>
public class ExportDaten
{
    public int Version { get; set; } = 1;

    /// <summary>Preisstand der Datei – ältere Stände werden beim Laden auf die aktuellen Preise gehoben.</summary>
    public int PreisStand { get; set; }
    public IcRechner? Ic { get; set; }
    public EleraidRechner? Eleraids { get; set; }
    public Act5Rechner? Act5 { get; set; }
    public Act6Rechner? Act6 { get; set; }
    public Act7Rechner? Act7 { get; set; }
    public Act8Rechner? Act8 { get; set; }
    public Act9Rechner? Act9 { get; set; }
    public Charakter? Charakter { get; set; }
    public MatPreise? Mats { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ExportDaten))]
internal partial class ExportJsonContext : JsonSerializerContext;
