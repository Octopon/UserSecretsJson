using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace UserSecretsJson.Helpers {
    public class UserSecretHelper {
        private static UserSecretHelper _instance;
        private static readonly object _lock = new object();
        private JObject _secret;

        private const string Namespace = "UserSecretsJson";
        private const string FileName = "secrets.json";

        private UserSecretHelper() {
            try {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(UserSecretHelper)).Assembly;
                var stream = assembly.GetManifestResourceStream($"{Namespace}.{FileName}");
                using (var reader = new StreamReader(stream)) {
                    var file = reader.ReadToEnd();
                    _secret = JObject.Parse(file);
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Unable to loading file secrets.json {ex.Message}");
            }
        }

        //Singleton double-check multithreading
        public static UserSecretHelper Instance {
            get {
                if (_instance == null) {
                    lock (_lock) {
                        if (_instance == null) {
                            _instance = new UserSecretHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        //indexator
        public string this[string SecretKey] 
            {
            get {
                try 
                    { 
                    var path = SecretKey.Split(':');

                    JToken node = _secret[path[0]];
                    for (int index = 1; index < path.Length; index++) {
                        node = node[path[index]];
                    }
                    return node.ToString(); 
                }
                catch (Exception ex) 
                    {
                    Console.WriteLine( $"Unable to get secrets by key {ex.Message}" );
                    return "";
                }
            }
        }
    }
}
