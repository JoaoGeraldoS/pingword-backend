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
        
                // 1. O corpo da requisição com o token vindo do Android
                var decodeRequest = new DecodeIntegrityTokenRequest { IntegrityToken = integrityToken };
                
                // 2. O recurso do projeto DEVE ter o prefixo "projects/"
                var projectResource = $"projects/{_projectNumber}"; 
        
                // 3. 🚩 A MUDANÇA: Use a navegação explícita via .Projects
                // Isso garante que o Google não confunda o ID do projeto com o Package Name
                var result = await service.Projects
                    .DecodeIntegrityToken(decodeRequest, projectResource)
                    .ExecuteAsync();
        
                var appIntegrity = result.TokenPayloadExternal?.AppIntegrity;
                var deviceIntegrity = result.TokenPayloadExternal?.DeviceIntegrity;
        
                // Logs para Debug no Render
                Console.WriteLine($"Token Decodificado para Package: {appIntegrity?.PackageName}");
        
                // 4. Validação do Package Name
                if (appIntegrity?.PackageName != _packageName)
                {
                    return $"Fraude: Package Name divergente ({appIntegrity?.PackageName})";
                }
        
                var appVerdict = appIntegrity?.AppRecognitionVerdict;
        
                // 5. Lógica flexível para aceitar versões de teste (USB/Sideload)
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
