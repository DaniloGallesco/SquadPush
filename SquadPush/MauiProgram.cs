using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Auth;
using Plugin.Firebase.Bundled.Shared;
using Plugin.Firebase.Crashlytics;
using Plugin.Firebase.CloudMessaging;

#if IOS
using Plugin.Firebase.Bundled.Platforms.iOS;
#else
using Plugin.Firebase.Bundled.Platforms.Android;
#endif

namespace SquadPush
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterFirebaseServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddTransient<DogPage>();
            builder.Services.AddTransient<CatPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MyPushes>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            CrossFirebaseCloudMessaging.Current.NotificationReceived += (sender, e) =>
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Notification",  e.Notification}
                };

                if (navigationParameter.Any())
                {
                    FCMNotification aa = (FCMNotification)navigationParameter.FirstOrDefault().Value;
                    List<FCMNotification> lista = new List<FCMNotification>();


                    var result = Preferences.Default.Get("pushes", "");
                    if (result != "")
                    {
                        lista = System.Text.Json.JsonSerializer.Deserialize<List<FCMNotification>>(result);
                        lista.Add(aa);                        
                        Preferences.Default.Set("pushes", System.Text.Json.JsonSerializer.Serialize(lista));
                    }
                    else
                    {
                        lista.Add(aa);
                        Preferences.Default.Set("pushes", System.Text.Json.JsonSerializer.Serialize(lista));
                    }
                  
                }
            };
            CrossFirebaseCloudMessaging.Current.NotificationTapped += (sender, e) =>
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "Notification",  e.Notification}
                };

                if (navigationParameter.Any())
                {
                    FCMNotification aa = (FCMNotification)navigationParameter.FirstOrDefault().Value;
                    var urlclick = aa.Data.First(kvp => kvp.Key == "Click").Value;

                    switch (urlclick)
                    {
                        case "DOG":
                            Shell.Current.GoToAsync(nameof(DogPage));
                            break;
                        case "CAT":
                            Shell.Current.GoToAsync(nameof(CatPage));
                            break;
                        default:
                            Shell.Current.GoToAsync(nameof(MainPage));
                            break;
                    }
                }
            };
            
            return builder.Build();
        }

        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events =>
            {
#if IOS
            events.AddiOS(iOS => iOS.FinishedLaunching((app, launchOptions) => {
                CrossFirebase.Initialize(CreateCrossFirebaseSettings());
                return false;
            }));
#else
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                    CrossFirebase.Initialize(activity, CreateCrossFirebaseSettings())));
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
#endif
            });

            builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);
            return builder;
        }

        private static CrossFirebaseSettings CreateCrossFirebaseSettings()
        {
            return new CrossFirebaseSettings(isAuthEnabled: true, isCloudMessagingEnabled: true, isAnalyticsEnabled: true);
        }

    }
}
