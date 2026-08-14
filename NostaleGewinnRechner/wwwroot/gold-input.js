// Tausenderpunkt-Maske für .gold-input-Felder.
// Formatiert live beim Tippen und hält den Cursor an der richtigen Stelle.
// Blazor rendert anschließend denselben formatierten Text, daher bleibt der
// DOM-Wert stabil und der Cursor springt nicht.
// Hell/Dunkel-Design lesen und umschalten (gespeichert im Browser).
window.goldTheme = {
    lesen: function () {
        return document.documentElement.dataset.theme || "dark";
    },
    setzen: function (theme) {
        document.documentElement.dataset.theme = theme;
        try { localStorage.setItem("gewinnrechner-theme", theme); } catch (e) { }
    }
};

// Startet einen Datei-Download mit dem übergebenen Textinhalt (für den Werte-Export).
window.goldDatei = {
    speichern: function (dateiname, inhalt) {
        const blob = new Blob([inhalt], { type: "application/json" });
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = dateiname;
        document.body.appendChild(a);
        a.click();
        a.remove();
        URL.revokeObjectURL(url);
    }
};

// Beim Fokussieren den Inhalt markieren – so kann man direkt lostippen.
document.addEventListener("focusin", function (e) {
    if (e.target.classList && e.target.classList.contains("gold-input")) {
        e.target.select();
    }
});

document.addEventListener("input", function (e) {
    const el = e.target;
    if (!el.classList || !el.classList.contains("gold-input")) {
        return;
    }

    const digitsBeforeCaret = el.value
        .slice(0, el.selectionStart)
        .replace(/\D/g, "").length;

    let digits = el.value.replace(/\D/g, "").slice(0, 15);
    digits = digits.replace(/^0+(?=\d)/, "");
    const formatted = digits === "" ? "0" : Number(digits).toLocaleString("de-DE");

    if (el.value !== formatted) {
        el.value = formatted;
    }

    let pos = 0;
    let seen = 0;
    while (pos < formatted.length && seen < digitsBeforeCaret) {
        if (/\d/.test(formatted[pos])) {
            seen++;
        }
        pos++;
    }
    el.setSelectionRange(pos, pos);
});
