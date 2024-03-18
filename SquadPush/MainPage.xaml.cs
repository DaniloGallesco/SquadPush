using Plugin.Firebase.CloudMessaging;

namespace SquadPush
{
    public partial class MainPage : ContentPage
    {      
        public MainPage()
        {
            InitializeComponent();
            GerarToken();
        }

        private async void GerarToken()
        {
            CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

            LblToken.Text = token;


            SemanticScreenReader.Announce(LblToken.Text);
#if DEBUG
            Console.WriteLine("TOKEN: "+token);
#endif
        }

        private async void Dog_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DogPage));
        }

        private async void Cat_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CatPage));
        }

        private async void MyPushes_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MyPushes));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Preferences.Default.Set("pushes", "");
        }
    }

}
