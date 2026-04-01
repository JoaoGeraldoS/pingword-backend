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
                ApplicationName = "PingWord"
            });
        }

        public async Task<string> VerifyTokenAsync(string integrityToken)
        {
            try
            {
                var service = GetPlayIntegrityService();
        
                // 1. Prepara o corpo com o token
                var decodeRequest = new DecodeIntegrityTokenRequest { IntegrityToken = integrityToken };
                

                var resourceName = $"projects/{_projectNumber}/apps/{_packageName}";
                Console.WriteLine($"RESOURCE: '{resourceName}'");
                // 3. 🚩 A SOLUÇÃO: Use o construtor da classe ProjectsResource
                // Isso elimina a ambiguidade da ordem dos parâmetros
                 var requestExecute = service.V1.DecodeIntegrityToken(
                     decodeRequest,
                    $"projects/{_projectNumber}/apps/{_packageName}"
                 );
                        
                // 4. Executa a chamada
                var result = await requestExecute.ExecuteAsync();
        
                // --- O restante do seu código de validação ---
                var appIntegrity = result.TokenPayloadExternal?.AppIntegrity;
                
                if (appIntegrity?.PackageName != _packageName)
                {
                    return $"Fraude: Package Name divergente ({appIntegrity?.PackageName})";
                }
        
                var appVerdict = appIntegrity?.AppRecognitionVerdict;
        
                if (appVerdict == "PLAY_RECOGNIZED" || appVerdict == "UNEVALUATED" || appVerdict == "UNRECOGNIZED_VERSION")
                {
                    return "App Original e Seguro";
                }
        
                return $"App não reconhecido: {appVerdict}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Play Integrity Error: {ex.Message}");
                return $"Erro técnico: {ex.Message}";
            }
        }
    }
}
