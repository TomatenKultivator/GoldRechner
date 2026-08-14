using Microsoft.JSInterop;

namespace NostaleGewinnRechner.Models;

/// <summary>
/// Speichert die Rechner-Werte automatisch im localStorage des Browsers
/// und stellt sie beim nächsten Besuch wieder her.
/// </summary>
public class SpeicherService(IJSRuntime js, RechnerState state)
{
    private const string Schluessel = "gewinnrechner-werte";

    private CancellationTokenSource? _cts;
    private bool _bereit;

    /// <summary>Stellt gespeicherte Werte her; erst danach wird Speichern erlaubt.</summary>
    public async Task LadenAsync()
    {
        if (_bereit)
        {
            return;
        }

        try
        {
            var json = await js.InvokeAsync<string?>("localStorage.getItem", Schluessel);
            if (!string.IsNullOrEmpty(json))
            {
                state.ImportJson(json, "Gespeichert");
            }
        }
        catch
        {
            // localStorage nicht verfügbar (z. B. blockiert) – App läuft ohne Speichern weiter.
        }

        _bereit = true;
    }

    /// <summary>Speichert verzögert (entprellt), damit nicht jeder Tastendruck einzeln schreibt.</summary>
    public void PlaneSpeichern()
    {
        if (!_bereit)
        {
            return;
        }

        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        _ = SpeichernAsync(_cts.Token);
    }

    private async Task SpeichernAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(400, token);
            await js.InvokeVoidAsync("localStorage.setItem", Schluessel, state.ExportJson());
        }
        catch
        {
            // Abgebrochen (neuere Eingabe) oder localStorage nicht verfügbar.
        }
    }
}
