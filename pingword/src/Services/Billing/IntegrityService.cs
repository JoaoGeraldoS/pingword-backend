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

        private PlayIntegrityService GetPlayIntegrityService()
        {

            var json = _configuration["GOOGLE_SERVICE_ACCOUNT_JSON"]
                       ?? Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON");

            if (string.IsNullOrEmpty(json))
                throw new Exception("A chave GOOGLE_SERVICE_ACCOUNT_JSON está vazia ou nula!");

            var credential = CredentialFactory
            .FromJson<ServiceAccountCredential>(json)
            .ToGoogleCredential()
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
                var projectResource = _projectNumber.ToString();

                Console.WriteLine($"🔍 Verificando token para project: {projectResource}");
                Console.WriteLine($"📦 Package esperado: {_packageName}");

                var result = await service.V1
                    .DecodeIntegrityToken(request, projectResource)
                    .ExecuteAsync();

                var appIntegrity = result.TokenPayloadExternal?.AppIntegrity;
                var deviceIntegrity = result.TokenPayloadExternal?.DeviceIntegrity;

                // 🔍 LOGS DETALHADOS
                Console.WriteLine($"PackageName: {appIntegrity?.PackageName}");
                Console.WriteLine($"AppRecognitionVerdict: {appIntegrity?.AppRecognitionVerdict}");
                Console.WriteLine($"DeviceRecognitionVerdict: {deviceIntegrity?.DeviceRecognitionVerdict}");

                if (appIntegrity?.PackageName != _packageName)
                {
                    Console.WriteLine("❌ Package Name divergente");
                    return "Fraude: Package Name divergente";
                }

                var appVerdict = appIntegrity.AppRecognitionVerdict;
                Console.WriteLine($"App Verdict: {appVerdict}");

                if (appVerdict == "PLAY_RECOGNIZED")
                    return "App Original e Seguro";

                // UNEVALUATED pode ser aceito em alguns casos
                if (appVerdict == "UNEVALUATED")
                    return "App Original (UNEVALUATED)"; // Mude para aceitar se necessário

                return $"App não reconhecido: {appVerdict}";
            }
            catch (Google.GoogleApiException gex) when (gex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //Remover esse catch em produção
                if (gex.Message.Contains("App is not found"))
                {
                    Console.WriteLine("✅ Modo TESTE: App em fase interna (Play Integrity skip)");
                    return "App Original e Seguro";
                    
                    
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Play Integrity Error: {ex.Message}");
                return $"Erro técnico: {ex.Message}";
            }
        }
    }
}
