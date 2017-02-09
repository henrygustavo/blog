namespace Blog.Storage
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Script.Serialization;
    using Business.Logic.Interfaces;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Auth.OAuth2.Flows;
    using Google.Apis.Auth.OAuth2.Responses;
    using Google.Apis.Drive.v2;
    using Google.Apis.Services;

    public static class GoogleAuthentication
    {
        /// <summary>
        /// Authenticate to Google Using Oauth2
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="clientId">From Google Developer console https://console.developers.google.com</param>
        /// <param name="clientSecret">From Google Developer console https://console.developers.google.com</param>
        /// <param name="userName">A string used to identify a user.</param>
        /// <returns></returns>
        public static DriveService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {

            var folder = HttpContext.Current.Server.MapPath("/App_Data/Drive.Auth.Store");

            //Google Drive scopes Documentation:   https://developers.google.com/drive/web/scopes
            string[] scopes = { DriveService.Scope.Drive,  // view and manage your files and documents
                                             DriveService.Scope.DriveAppdata,  // view and manage its own configuration data
                                             DriveService.Scope.DriveAppsReadonly,   // view your drive apps
                                             DriveService.Scope.DriveFile,   // view and manage files created by this app
                                             DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
                                             DriveService.Scope.DriveReadonly,   // view files and documents on your drive
                                             DriveService.Scope.DriveScripts };  // modify your app scripts


            try
            {

                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new SavedDataStore(folder)).Result;//new FileDataStore("Drive.Auth.Store")).Result;



                DriveService service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ConfigurationManager.AppSettings["googleApplicationName"]
                });
                return service;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException.Message);
            }

        }

        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>

        /// <returns></returns>
        public static DriveService AuthenticateServiceAccount()
        {
            string serviceAccountEmail = ConfigurationManager.AppSettings["googleServiceAccount"];
            string keyFilePath = HttpContext.Current.Server.MapPath("/App_Data/file.p12");

            // check the file exists
            if (!File.Exists(keyFilePath))
            {
                Console.WriteLine("An Error occurred - Key file does not exist");
                return null;
            }

            //Google Drive scopes Documentation:   https://developers.google.com/drive/web/scopes
            string[] scopes = { DriveService.Scope.Drive,  // view and manage your files and documents
                                             DriveService.Scope.DriveAppdata,  // view and manage its own configuration data
                                             DriveService.Scope.DriveAppsReadonly,   // view your drive apps
                                             DriveService.Scope.DriveFile,   // view and manage files created by this app
                                             DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
                                             DriveService.Scope.DriveReadonly,   // view files and documents on your drive
                                             DriveService.Scope.DriveScripts };  // modify your app scripts     

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                // Create the service.
                DriveService service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ConfigurationManager.AppSettings["googleApplicationName"]

                });

                return service;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DriveService CreateServie(IGoogleApiDataStoreBL GoogleApiDataStoreBL)
        {
            var tokenResponse = new TokenResponse
            {
                AccessToken = GetAccessToken().access_token,
                RefreshToken = ConfigurationManager.AppSettings["googleDriveRefreshToken"]
            };

            string[] scopes = { DriveService.Scope.Drive,  // view and manage your files and documents
                                             DriveService.Scope.DriveAppdata,  // view and manage its own configuration data
                                             DriveService.Scope.DriveAppsReadonly,   // view your drive apps
                                             DriveService.Scope.DriveFile,   // view and manage files created by this app
                                             DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
                                             DriveService.Scope.DriveReadonly,   // view files and documents on your drive
                                             DriveService.Scope.DriveScripts };  // modify your app scripts 

            //string folder = ConfigurationManager.AppSettings["isTestGoogleDrive"] != "1" ? HttpContext.Current.Server.MapPath("/App_Data/Drive.Auth.Store") : Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, @"App_Data\Drive.Auth.Store");

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = ConfigurationManager.AppSettings["googleClientId"],
                    ClientSecret = ConfigurationManager.AppSettings["googleClientSecret"]
                },
                Scopes = scopes,
                DataStore = new DatabaseDataStore(GoogleApiDataStoreBL)
            });

            var credential = new UserCredential(apiCodeFlow, ConfigurationManager.AppSettings["googleApplicationName"], tokenResponse);


            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ConfigurationManager.AppSettings["googleApplicationName"]
            });

            return service;
        }

        private static AccessTokenResponse GetAccessToken()
        {
            var url = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token",
                ConfigurationManager.AppSettings["googleClientId"],
                ConfigurationManager.AppSettings["googleClientSecret"],
                ConfigurationManager.AppSettings["googleDriveRefreshToken"]
                );

            var req = WebRequest.Create("https://www.googleapis.com/oauth2/v3/token");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] data = encoder.GetBytes(url);
            req.ContentLength = data.Length;
            req.GetRequestStream().Write(data, 0, data.Length);

            AccessTokenResponse tokenResponseAuth = new JavaScriptSerializer().Deserialize(new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd(), typeof(AccessTokenResponse)) as AccessTokenResponse;

            return tokenResponseAuth;
        }
    }
}
