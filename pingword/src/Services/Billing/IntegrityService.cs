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
                var json = _configuration["GOOGLE_SERVICE_ACCOUNT_JSON"]
                           ?? Environment.GetEnvironmentVariable("GOOGLE_SERVICE_ACCOUNT_JSON");
        
                if (string.IsNullOrEmpty(json))
                    throw new Exception("A chave GOOGLE_SERVICE_ACCOUNT_JSON está vazia ou nula!");
        
                // 1. Cria credencial
                var credential = GoogleCredential
                    .FromJson(json)
                    .CreateScoped("https://www.googleapis.com/auth/playintegrity");
        
                // 2. Gera access token
                var accessToken = await credential
                    .UnderlyingCredential
                    .GetAccessTokenForRequestAsync();
        
                // 3. Monta URL correta
                var url = $"https://playintegrity.googleapis.com/v1/projects/{_projectNumber}:decodeIntegrityToken";
        
                using var http = new HttpClient();
        
                http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        
                // 4. Body correto (IMPORTANTE: snake_case)
                var body = new
                {
                    integrity_token = integrityToken
                };
        
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(body),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );
        
                // 5. Request
                var response = await http.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
        
                if (!response.IsSuccessStatusCode)
                {
                    return $"Erro HTTP: {response.StatusCode} - {responseString}";
                }
        
                // 6. Desserializa
                var result = System.Text.Json.JsonSerializer.Deserialize<DecodeIntegrityTokenResponse>(responseString);
        
                var appIntegrity = result?.TokenPayloadExternal?.AppIntegrity;
        
                if (appIntegrity?.PackageName != _packageName)
                {
                    return $"Fraude: Package Name divergente ({appIntegrity?.PackageName})";
                }
        
                var appVerdict = appIntegrity?.AppRecognitionVerdict;
        
                if (appVerdict == "PLAY_RECOGNIZED")
                {
                    return "App Original e Seguro";
                }
        
                return $"App inválido: {appVerdict}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Play Integrity Error: {ex.Message}");
                return $"Erro técnico: {ex.Message}";
            }
        }
    }
}
