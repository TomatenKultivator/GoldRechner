using Microsoft.AspNetCore.Components;
using NostaleGewinnRechner.Models;

namespace NostaleGewinnRechner.Components;

/// <summary>
/// Basisklasse für alle Rechner-Seiten: rendert die Seite neu,
/// wenn im Vorlagen-Panel eine Vorlage geladen oder alles genullt wird.
/// </summary>
public class RechnerPage : ComponentBase, IDisposable
{
    [Inject] protected RechnerState State { get; set; } = default!;
    [Inject] protected SpeicherService Speicher { get; set; } = default!;

    protected override void OnInitialized() => State.OnChange += HandleChanged;

    // Jede Interaktion rendert die Seite neu – das entprellte Speichern
    // sichert die Werte danach automatisch im Browser.
    protected override void OnAfterRender(bool firstRender) => Speicher.PlaneSpeichern();

    private void HandleChanged() => InvokeAsync(StateHasChanged);

    public void Dispose() => State.OnChange -= HandleChanged;
}
