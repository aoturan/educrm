using System.Text;

namespace EduCrm.WebApi.Helpers;

public static class CsvWriter
{
    private const char Delimiter = ';';
    private static readonly byte[] Utf8Bom = { 0xEF, 0xBB, 0xBF };
    private static readonly char[] EscapeTriggers = { ';', '"', '\r', '\n' };

    public static async Task WriteAsync<T>(
        Stream output,
        IEnumerable<T> rows,
        IReadOnlyList<(string Header, Func<T, string?> Selector)> columns,
        CancellationToken ct)
    {
        var sb = new StringBuilder();
        AppendRow(sb, columns.Select(c => (string?)c.Header));

        foreach (var row in rows)
        {
            ct.ThrowIfCancellationRequested();
            AppendRow(sb, columns.Select(c => c.Selector(row)));
        }

        await output.WriteAsync(Utf8Bom, ct);
        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        await output.WriteAsync(bytes, ct);
    }

    private static void AppendRow(StringBuilder sb, IEnumerable<string?> fields)
    {
        var first = true;
        foreach (var field in fields)
        {
            if (!first) sb.Append(Delimiter);
            first = false;
            if (field is not null) sb.Append(Escape(field));
        }
        sb.Append("\r\n");
    }

    private static string Escape(string value)
    {
        if (value.IndexOfAny(EscapeTriggers) < 0) return value;
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    public static string AsExcelText(string value)
    {
        var inner = value.Replace("\"", "\"\"");
        return $"=\"{inner}\"";
    }
}
