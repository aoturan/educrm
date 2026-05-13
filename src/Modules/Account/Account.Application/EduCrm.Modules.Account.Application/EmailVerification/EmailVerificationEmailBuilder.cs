using EduCrm.Modules.Account.Application.Email;

namespace EduCrm.Modules.Account.Application.EmailVerification;

public static class EmailVerificationEmailBuilder
{
    public static string BuildLink(string baseUrl, string email, string token)
    {
        var separator = baseUrl.Contains('?') ? '&' : '?';
        return $"{baseUrl}{separator}email={Uri.EscapeDataString(email)}&token={token}";
    }

    public static EmailMessage Build(string to, string fullName, string link, int lifetimeMinutes)
    {
        var encodedLink = System.Net.WebUtility.HtmlEncode(link);
        var lifetimeHours = lifetimeMinutes / 60;
        var lifetimeLabel = lifetimeHours >= 1
            ? $"{lifetimeHours} saat"
            : $"{lifetimeMinutes} dakika";

        var html =
            $"""
             <p>Merhaba {System.Net.WebUtility.HtmlEncode(fullName)},</p>
             <p>EduCRM hesabınızı oluşturduğunuz için teşekkür ederiz. Hesabınızı kullanmaya başlamadan önce e-posta adresinizi doğrulamanız gerekiyor. Aşağıdaki bağlantıya tıklayarak doğrulama işlemini tamamlayabilirsiniz:</p>
             <p><a href="{encodedLink}">{encodedLink}</a></p>
             <p>Bağlantı {lifetimeLabel} boyunca geçerlidir. Bu kaydı siz yapmadıysanız bu e-postayı yok sayabilirsiniz.</p>
             <p>EduCRM</p>
             """;

        var text =
            $"""
             Merhaba {fullName},

             EduCRM hesabınızı oluşturduğunuz için teşekkür ederiz. Hesabınızı kullanmaya başlamadan önce e-posta adresinizi doğrulamanız gerekiyor. Aşağıdaki bağlantıyı kullanarak doğrulama işlemini tamamlayabilirsiniz:

             {link}

             Bağlantı {lifetimeLabel} boyunca geçerlidir. Bu kaydı siz yapmadıysanız bu e-postayı yok sayabilirsiniz.

             EduCRM
             """;

        return new EmailMessage(
            To: to,
            Subject: "E-Posta Adresinizi Doğrulayın",
            HtmlBody: html,
            TextBody: text);
    }
}
