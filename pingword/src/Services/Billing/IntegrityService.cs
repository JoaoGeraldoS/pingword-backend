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
        private readonly PlayIntegrityService _playIntegrityService;
        private readonly string _packageName = "com.pingword.app";
        private readonly long _projectNumber = 540468689107;

        public IntegrityService()
        {
            
            var json = Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON");

            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("Variável de ambiente GOOGLE_SERVICE_ACCOUNT_JSON não encontrada!");
            }

            // Forma moderna de carregar o JSON sem o aviso de obsoleto
            GoogleCredential credential = GoogleCredential.FromJson(json)
                .CreateScoped(PlayIntegrityService.Scope.Playintegrity);

            _playIntegrityService = new PlayIntegrityService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "PingWord-Backend"
            });
        }

        public async Task<string> VerifyTokenAsync(string integrityToken)
        {
            var request = new DecodeIntegrityTokenRequest { IntegrityToken = integrityToken };
            string projectResource = $"projects/{_projectNumber}";

           
            var result = await _playIntegrityService.V1
                .DecodeIntegrityToken(request, projectResource)
                .ExecuteAsync();

            var deviceVerdict = result.TokenPayloadExternal.DeviceIntegrity.DeviceRecognitionVerdict;
            var appVerdict = result.TokenPayloadExternal.AppIntegrity.AppRecognitionVerdict;

            
            var tokenPackage = result.TokenPayloadExternal.AppIntegrity.PackageName;
            if (tokenPackage != _packageName)
            {
                return "Fraude: Package Name divergente";
            }

            
            if (appVerdict == "PLAY_RECOGNIZED" && deviceVerdict.Contains("MEETS_DEVICE_INTEGRITY"))
            {
                return "App Original e Seguro";
            }

            return $"Possível Fraude: App={appVerdict}, Device={string.Join(",", deviceVerdict)}";
        }
    }
}
