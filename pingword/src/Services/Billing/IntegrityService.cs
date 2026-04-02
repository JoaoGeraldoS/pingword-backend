using Google.Apis.Auth.OAuth2;
using Google.Apis.PlayIntegrity.v1;
using Google.Apis.PlayIntegrity.v1.Data;
using Google.Apis.Services;

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

        

      public async Task<string> VerifyTokenAsync(string integrityToken)
{
    try
    {
        var json = _configuration["GOOGLE_SERVICE_ACCOUNT_JSON"]
                   ?? Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON");

        if (string.IsNullOrEmpty(json))
            throw new Exception("GOOGLE_SERVICE_ACCOUNT_JSON está vazia!");

        var credential = GoogleCredential
            .FromJson(json)
            .CreateScoped(PlayIntegrityService.Scope.Playintegrity);

        var service = new PlayIntegrityService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "PingWord"
        });

        var requestBody = new DecodeIntegrityTokenRequest
        {
            IntegrityToken = integrityToken
        };

        // Classic API usa packageName em vez de projectNumber
        var request = service.V1.DecodeIntegrityToken(requestBody, _packageName);
        var response = await request.ExecuteAsync();

        var appIntegrity = response.TokenPayloadExternal?.AppIntegrity;

        if (appIntegrity?.PackageName != _packageName)
            return $"Fraude: Package Name divergente ({appIntegrity?.PackageName})";

        return appIntegrity?.AppRecognitionVerdict == "PLAY_RECOGNIZED"
            ? "App Original e Seguro"
            : $"App inválido: {appIntegrity?.AppRecognitionVerdict}";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Play Integrity Error: {ex.Message}");
        return $"Erro técnico: {ex.Message}";
    }
}
    }
}
