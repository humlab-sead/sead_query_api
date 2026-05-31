using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class TemplateLoader
{
    private static readonly Assembly _asm = typeof(TemplateLoader).Assembly;

    public static string Read(string name)
    {
        // name examples: "route.sql.sbn" or "subfolder.my-template.sql.sbn"
        string wanted = "SqlTemplates." + name;
        string? resName = _asm.GetManifestResourceNames()
            .FirstOrDefault(n => n.Equals(wanted, StringComparison.Ordinal) || n.EndsWith("." + name, StringComparison.Ordinal));
        if (resName is null)
            throw new FileNotFoundException(
                $"Embedded template '{name}' not found. " + $"Available: {string.Join(", ", _asm.GetManifestResourceNames())}"
            );

        using var s = _asm.GetManifestResourceStream(resName)!;
        using var r = new StreamReader(s);
        return r.ReadToEnd();
    }
}
