using Google.Apis.AndroidPublisher.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PlayIntegrity.v1;
using Google.Apis.PlayIntegrity.v1.Data;
using Google.Apis.Services;
using System.Text;

namespace pingword.src.Services.Billing
{
    public class IntegrityService
{
    private readonly string _packageName = "com.pingword.app";
    private readonly long _projectNumber = 540468689107;
    private readonly IConfiguration _configuration;

    public IntegrityService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private PlayIntegrityService GetPlayIntegrityService()
    {
        
        var json = _configuration["GOOGLE_SERVICE_ACCOUNT_JSON"]
                   ?? Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON");

        if (string.IsNullOrEmpty(json))
            throw new Exception("A chave GOOGLE_SERVICE_ACCOUNT_JSON está vazia ou nula!");

        var credential = GoogleCredential.FromJson(json)
            .CreateScoped(PlayIntegrityService.Scope.Playintegrity);

        return new PlayIntegrityService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "PingWord-Backend"
        });
    }

    public async Task<string> VerifyTokenAsync(string integrityToken)
    {
        try
        {
            var service = GetPlayIntegrityService();
            var request = new DecodeIntegrityTokenRequest { IntegrityToken = integrityToken };
            string projectResource = $"projects/{_projectNumber}";

            var result = await service.V1
                .DecodeIntegrityToken(request, projectResource)
                .ExecuteAsync();

            var appIntegrity = result.TokenPayloadExternal.AppIntegrity;
            var deviceIntegrity = result.TokenPayloadExternal.DeviceIntegrity;

            if (appIntegrity.PackageName != _packageName)
                return "Fraude: Package Name divergente";

            // PLAY_RECOGNIZED é o veredito de sucesso total
            if (appIntegrity.AppRecognitionVerdict == "PLAY_RECOGNIZED")
                return "App Original e Seguro";

            return $"Atenção: App={appIntegrity.AppRecognitionVerdict}";
        }
        catch (Exception ex)
        {
            return $"Erro técnico: {ex.Message}";
        }
    }
}
}
