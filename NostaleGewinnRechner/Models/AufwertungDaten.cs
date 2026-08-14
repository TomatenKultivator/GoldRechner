namespace NostaleGewinnRechner.Models;

/// <summary>Ein Material, das je Aufwertungs-Versuch verbraucht wird – der Preis kommt aus <see cref="MatPreise.AufwertungPreis"/> (Mats-Seite).</summary>
public record AufwertungMaterial(string Name, double Anzahl);

/// <summary>Was aufgewertet wird – bestimmt, welche Schutzrolle greift und welche Materialien die Premium-Rolle halbiert.</summary>
public enum AufwertungArt
{
    Waffe,
    Sp,
}

/// <summary>
/// Mit welcher Schutzrolle aufgewertet wird. Ohne Rolle geht die Ausrüstung bzw.
/// die Seele der Spezialistenkarte beim Fehlschlag verloren.
/// </summary>
public enum SchutzModus
{
    /// <summary>Ohne Rolle – nur unterhalb der Zerstörungsgrenze sinnvoll.</summary>
    Keine,

    /// <summary>Normale Rolle: schützt vor Zerstörung, senkt aber keine Kosten.</summary>
    Normal,

    /// <summary>Goldene bzw. Premium-Rolle: schützt und halbiert einen Teil der Materialien sowie das Gold.</summary>
    Premium,

    /// <summary>
    /// Je Stufe die billigere der beiden Rollen. Welche das ist, hängt von den
    /// Preisen ab: Die Premium-Rolle spart Material und Gold, kostet aber meist
    /// mehr – das lohnt sich erst, wenn die Stufe selbst teuer genug ist.
    /// </summary>
    Guenstigste,
}

/// <summary>
/// Die für einen Aufwertungs-Schritt eingesetzte Schutzrolle: ihr Preis je Versuch
/// und die Materialien, deren Verbrauch die Premium-Variante halbiert.
/// </summary>
/// <param name="Rolle">Preis einer Rolle – wird je Versuch verbraucht.</param>
/// <param name="Halbiert">Materialnamen, die eine Premium-Rolle um 50 % senkt (Gold immer).</param>
/// <param name="Name">Name der Rolle für die Anzeige – leer, wenn keine eingesetzt wird.</param>
public record SchutzOption(SchutzModus Modus, double Rolle, string[] Halbiert, string Name = "")
{
    /// <summary>Ohne Schutzrolle – keine Zusatzkosten, keine Ersparnis.</summary>
    public static readonly SchutzOption Ohne = new(SchutzModus.Keine, 0, []);

    /// <summary>Faktor auf ein Material bzw. auf das Gold: 0,5 mit Premium-Rolle, sonst 1.</summary>
    public double Faktor(string? material) =>
        Modus == SchutzModus.Premium && (material is null || Halbiert.Contains(material)) ? 0.5 : 1;
}

/// <summary>
/// Eine Aufwertungsstufe: Erfolgschance in % für den Schritt AUF diese Stufe
/// plus die Kosten je Versuch (Gold und Materialien).
/// </summary>
public record AufwertungStufe(int Ziel, double Chance, double Gold = 0, AufwertungMaterial[]? Materialien = null)
{
    /// <summary>
    /// Erwartete Versuche für diesen einen Schritt (geometrische Verteilung: 1/p) –
    /// bei 10 % Chance also im Schnitt 10 Versuche.
    /// </summary>
    public double Versuche => 100 / Chance;

    /// <summary>
    /// Effektive Erfolgschance mit Upgrade-Event: der Event-Faktor (z. B. 1,5 bei
    /// „+50 % Chance“) wirkt multiplikativ, gedeckelt bei 100 %.
    /// </summary>
    public double ChanceMitEvent(double faktor) => Math.Min(100, Chance * faktor);

    /// <summary>Erwartete Versuche für diesen Schritt mit Upgrade-Event: 100 / effektive Chance.</summary>
    public double VersucheMitEvent(double faktor) => 100 / ChanceMitEvent(faktor);

    /// <summary>Materialkosten je Versuch – 0, solange auf der Mats-Seite keine Preise eingetragen sind.</summary>
    public double MatKosten(MatPreise preise, SchutzOption? schutz = null) =>
        (Materialien ?? []).Sum(m => m.Anzahl * (schutz ?? SchutzOption.Ohne).Faktor(m.Name) * preise.AufwertungPreis(m.Name));

    /// <summary>
    /// Gesamtkosten je Versuch: Gold plus Materialien plus die verbrauchte Schutzrolle.
    /// Mit Premium-Rolle sind Gold und ein Teil der Materialien halbiert.
    /// </summary>
    public double KostenJeVersuch(MatPreise preise, SchutzOption? schutz = null)
    {
        var s = schutz ?? SchutzOption.Ohne;
        return Gold * s.Faktor(null) + MatKosten(preise, s) + s.Rolle;
    }

    /// <summary>Kompakter Materialtext, z. B. „220× Cella, 1× Vollkommener Seelenstein“.</summary>
    public string MaterialText =>
        string.Join(", ", (Materialien ?? []).Select(m => $"{Fmt.Gold(m.Anzahl)}× {m.Name}"));
}

/// <summary>
/// Erfolgschancen und Kosten beim Aufwerten von Waffe und Spezialistenkarte.
/// Chancen und Materialien laut den offiziellen Forum-Guides
/// (Thread 286 „Upgreadekosten &amp; Chancen“ für Ausrüstung,
/// Thread 10234 „Spezialistenkarten Upgrades“ für SPs).
/// Fehlschlag-Folgen ohne Schutz (Abstufung/Zerstörung) sind nicht modelliert –
/// die erwarteten Versuche gelten für Aufwerten mit Schutz (Stufe bleibt erhalten);
/// Schutz-Items (Engelsfedern, Schutzrollen) sind darum nicht als Pflicht-Material gelistet.
/// Die Materialpreise kommen von der Mats-Seite (<see cref="MatPreise"/>) und
/// fließen automatisch in Kosten je Versuch und erwartete Gesamtkosten ein.
/// </summary>
public static class AufwertungDaten
{
    /// <summary>
    /// Waffe +1 bis +13. +1–10 laut Thread 286; der Schritt auf +10 ist dort nur als
    /// „&lt;1 %“ angegeben – hier mit 0,3 % angesetzt, bis ein genauer Wert vorliegt.
    /// Seelensteine: bis +5 normale, ab +6 Vollkommene.
    /// +11–13 kamen mit Akt 10 (Aufwertungserweiterung samt Pity-System: nach genug
    /// Fehlversuchen ist der nächste Versuch garantiert – der Pity-Balken bleibt beim
    /// Handeln erhalten). Offizielle Chancen/Materialien sind nicht veröffentlicht;
    /// die Chancen hier sind darum als EFFEKTIVE Schätzwerte inklusive Pity angesetzt
    /// (Ø-Versuche = 100/Chance soll ungefähr die echten Versuche bis zum Erfolg
    /// treffen) und werden ersetzt, sobald echte Werte vorliegen.
    /// </summary>
    public static readonly AufwertungStufe[] Waffe =
    [
        new(1, 100, Gold: 500, [new("Cella", 20), new("Seelenstein", 1)]),
        new(2, 100, Gold: 1_500, [new("Cella", 50), new("Seelenstein", 1)]),
        new(3, 90, Gold: 3_000, [new("Cella", 80), new("Seelenstein", 2)]),
        new(4, 80, Gold: 10_000, [new("Cella", 120), new("Seelenstein", 2)]),
        new(5, 60, Gold: 30_000, [new("Cella", 160), new("Seelenstein", 3)]),
        new(6, 40, Gold: 80_000, [new("Cella", 220), new("Vollkommener Seelenstein", 1)]),
        new(7, 20, Gold: 150_000, [new("Cella", 280), new("Vollkommener Seelenstein", 1)]),
        new(8, 5, Gold: 400_000, [new("Cella", 380), new("Vollkommener Seelenstein", 2)]),
        new(9, 1, Gold: 700_000, [new("Cella", 480), new("Vollkommener Seelenstein", 2)]),
        new(10, 0.3, Gold: 1_000_000, [new("Cella", 600), new("Vollkommener Seelenstein", 3)]),
        // ---- Akt-10-Erweiterung, effektive Schätzwerte inkl. Pity (siehe Kommentar oben) ----
        new(11, 5, Gold: 1_500_000, [new("Cella", 700), new("Vollkommener Seelenstein", 3)]),
        new(12, 4, Gold: 2_000_000, [new("Cella", 800), new("Vollkommener Seelenstein", 4)]),
        new(13, 3, Gold: 3_000_000, [new("Cella", 1_000), new("Vollkommener Seelenstein", 5)]),
    ];

    /// <summary>Höchste Aufwertungsstufe der Waffe.</summary>
    public const int MaxStufe = 13;

    /// <summary>Höchste Aufwertungsstufe der Spezialistenkarte.</summary>
    public const int SpMaxStufe = 20;

    /// <summary>
    /// SP +1 bis +20 (Thread 10234). „Glänzende Seelen“ werden je Art benötigt
    /// (Anzahl = gesamt über alle drei Arten); die Seelen-Art ab +11 weicht laut
    /// Guide ab (Ork-Seelensteine / Seelen der Altehrwürdigen Helden) – beim
    /// Eintragen der Preise prüfen.
    /// </summary>
    public static readonly AufwertungStufe[] Sp =
    [
        new(1, 80, Gold: 200_000, [new("Vollmondkristall", 1), new("Engelsfeder", 3), new("Drachenedelstein", 2)]),
        new(2, 75, Gold: 200_000, [new("Vollmondkristall", 3), new("Engelsfeder", 5), new("Drachenedelstein", 4)]),
        new(3, 70, Gold: 200_000, [new("Vollmondkristall", 5), new("Engelsfeder", 8), new("Drachenedelstein", 6)]),
        new(4, 60, Gold: 200_000, [new("Vollmondkristall", 7), new("Engelsfeder", 10), new("Drachenedelstein", 8)]),
        new(5, 50, Gold: 200_000, [new("Vollmondkristall", 10), new("Engelsfeder", 15), new("Drachenedelstein", 10)]),
        new(6, 40, Gold: 500_000, [new("Vollmondkristall", 12), new("Engelsfeder", 20), new("Glänzende Seele (je Art 1)", 3)]),
        new(7, 35, Gold: 500_000, [new("Vollmondkristall", 14), new("Engelsfeder", 25), new("Glänzende Seele (je Art 2)", 6)]),
        new(8, 30, Gold: 500_000, [new("Vollmondkristall", 16), new("Engelsfeder", 30), new("Glänzende Seele (je Art 3)", 9)]),
        new(9, 25, Gold: 500_000, [new("Vollmondkristall", 18), new("Engelsfeder", 35), new("Glänzende Seele (je Art 4)", 12)]),
        new(10, 20, Gold: 500_000, [new("Vollmondkristall", 20), new("Engelsfeder", 40), new("Glänzende Seele (je Art 5)", 15)]),
        new(11, 10, Gold: 1_000_000, [new("Vollmondkristall", 22), new("Engelsfeder", 45), new("Seele (höhere Art)", 1), new("Drachenedelstein", 1)]),
        new(12, 7, Gold: 1_000_000, [new("Vollmondkristall", 24), new("Engelsfeder", 50), new("Seele (höhere Art)", 2), new("Drachenedelstein", 2)]),
        new(13, 5, Gold: 1_000_000, [new("Vollmondkristall", 26), new("Engelsfeder", 55), new("Seele (höhere Art)", 3), new("Drachenedelstein", 3)]),
        new(14, 3, Gold: 1_000_000, [new("Vollmondkristall", 28), new("Engelsfeder", 60), new("Seele (höhere Art)", 4), new("Drachenedelstein", 4)]),
        new(15, 1.5, Gold: 1_000_000, [new("Vollmondkristall", 30), new("Engelsfeder", 70), new("Seele (höhere Art)", 5), new("Drachenedelstein", 5)]),
        new(16, 1.2, Gold: 1_250_000, [new("Vollmondkristall", 32), new("Engelsfeder", 80), new("Drachenedelstein", 2)]),
        new(17, 1, Gold: 1_500_000, [new("Vollmondkristall", 34), new("Engelsfeder", 90), new("Drachenedelstein", 4)]),
        new(18, 0.8, Gold: 1_750_000, [new("Vollmondkristall", 36), new("Engelsfeder", 100), new("Drachenedelstein", 6)]),
        new(19, 0.6, Gold: 2_000_000, [new("Vollmondkristall", 38), new("Engelsfeder", 110), new("Drachenedelstein", 8)]),
        new(20, 0.4, Gold: 2_250_000, [new("Vollmondkristall", 40), new("Engelsfeder", 120), new("Drachenedelstein", 10)]),
    ];

    // ---- Schutzrollen ----
    //
    // Ohne Rolle geht die Ausrüstung bzw. die Seele der Spezialistenkarte beim
    // Fehlschlag verloren; gespielt wird darum ab der Zerstörungsgrenze mit Rolle.
    // Die goldene bzw. Premium-Variante schützt zusätzlich und halbiert einen Teil
    // der Kosten (Quelle: die Item-Beschreibungen auf nosapki.com):
    //   * Goldene Ausrüstungsschutzrolle (Id 5369): „Die erforderliche Menge von
    //     Cellapuder und Gold ist um 50 % verringert.“ – Seelensteine bleiben voll.
    //   * SP-Rollen Premium (Ids 9497/9498): „Der Verbrauch von Engelsfedern,
    //     Vollmondkristallen und Gold sinkt um 50 %.“ – Edelsteine und Seelen bleiben voll.
    // Für SPs greift je nach Stufe eine andere Rolle: Niedrige bis +10,
    // Hohe für +11 bis +15, Drachen-Kartenschutzrolle für +16 bis +20.

    /// <summary>Ab dieser Zielstufe kann die Ausrüstung beim Fehlschlag zerstört werden (Thread 286).</summary>
    public const int WaffeZerstoerungAb = 3;

    /// <summary>Ab dieser Zielstufe kann die Seele der Spezialistenkarte zerstört werden.</summary>
    public const int SpZerstoerungAb = 3;

    private static readonly string[] WaffeHalbiert = ["Cella"];
    private static readonly string[] SpHalbiert = ["Vollmondkristall", "Engelsfeder"];

    /// <summary>Kann auf dieser Zielstufe überhaupt etwas zerstört werden – wird also eine Rolle eingesetzt?</summary>
    public static bool SchutzNoetig(AufwertungArt art, int ziel) =>
        ziel >= (art == AufwertungArt.Waffe ? WaffeZerstoerungAb : SpZerstoerungAb);

    /// <summary>Die konkrete Rolle (normal oder Premium) für eine Zielstufe.</summary>
    private static SchutzOption Rolle(MatPreise preise, AufwertungArt art, int ziel, bool premium)
    {
        var (preis, name) = art == AufwertungArt.Waffe
            ? premium
                ? (preise.RolleAusruestungGolden, "Goldene Ausrüstungsschutzrolle")
                : (preise.RolleAusruestung, "Ausrüstungsschutzrolle")
            : ziel switch
            {
                <= 10 => premium
                    ? (preise.RolleSpNiedrigPremium, "Niedrige SP-Schutzrolle (Premium)")
                    : (preise.RolleSpNiedrig, "Niedrige SP-Schutzrolle"),
                <= 15 => premium
                    ? (preise.RolleSpHochPremium, "Hohe SP-Schutzrolle (Premium)")
                    : (preise.RolleSpHoch, "Hohe SP-Schutzrolle"),
                _ => premium
                    ? (preise.RolleDrachenPremium, "Drachen-Kartenschutzrolle (Premium)")
                    : (preise.RolleDrachen, "Drachen-Kartenschutzrolle"),
            };

        return new(premium ? SchutzModus.Premium : SchutzModus.Normal, preis,
            art == AufwertungArt.Waffe ? WaffeHalbiert : SpHalbiert, name);
    }

    /// <summary>
    /// Die für diesen Schritt eingesetzte Rolle. Unterhalb der Zerstörungsgrenze wird
    /// keine gebraucht, dort fallen also weder Kosten noch Ersparnis an. Bei
    /// <see cref="SchutzModus.Guenstigste"/> gewinnt die Rolle mit den niedrigeren
    /// Gesamtkosten für genau diesen Schritt.
    /// </summary>
    public static SchutzOption Schutz(MatPreise preise, AufwertungArt art, AufwertungStufe stufe, SchutzModus modus)
    {
        if (modus == SchutzModus.Keine || !SchutzNoetig(art, stufe.Ziel))
        {
            return SchutzOption.Ohne;
        }

        if (modus != SchutzModus.Guenstigste)
        {
            return Rolle(preise, art, stufe.Ziel, modus == SchutzModus.Premium);
        }

        var normal = Rolle(preise, art, stufe.Ziel, premium: false);
        var premium = Rolle(preise, art, stufe.Ziel, premium: true);
        return stufe.KostenJeVersuch(preise, premium) <= stufe.KostenJeVersuch(preise, normal) ? premium : normal;
    }

    /// <summary>Erwartete Versuche insgesamt, um die Waffe von Stufe <paramref name="von"/> auf <paramref name="bis"/> zu bringen.</summary>
    public static double ErwarteteVersuche(int von, int bis, double faktor = 1) =>
        ErwarteteVersuche(Waffe, von, bis, faktor);

    /// <summary>
    /// Erwartete Versuche insgesamt für eine beliebige Chancen-Tabelle.
    /// <paramref name="faktor"/> ist der Chancen-Multiplikator eines Upgrade-Events (1 = kein Event).
    /// </summary>
    public static double ErwarteteVersuche(AufwertungStufe[] tabelle, int von, int bis, double faktor = 1) =>
        tabelle.Where(s => s.Ziel > von && s.Ziel <= bis).Sum(s => s.VersucheMitEvent(faktor));

    /// <summary>
    /// Erwartete Gesamtkosten (Gold plus Materialien, soweit Preise bekannt), um von
    /// Stufe <paramref name="von"/> auf <paramref name="bis"/> zu kommen:
    /// je Schritt erwartete Versuche × Kosten je Versuch.
    /// <paramref name="faktor"/> ist der Chancen-Multiplikator eines Upgrade-Events (1 = kein Event).
    /// </summary>
    public static double ErwarteteKosten(AufwertungStufe[] tabelle, int von, int bis, MatPreise preise,
        double faktor = 1, AufwertungArt art = AufwertungArt.Waffe, SchutzModus modus = SchutzModus.Keine) =>
        tabelle.Where(s => s.Ziel > von && s.Ziel <= bis)
               .Sum(s => s.VersucheMitEvent(faktor) * s.KostenJeVersuch(preise, Schutz(preise, art, s, modus)));
}
