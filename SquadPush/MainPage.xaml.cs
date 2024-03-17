using Plugin.Firebase.CloudMessaging;

namespace SquadPush
{
    public partial class MainPage : ContentPage
    {
      
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync(); 
            
            LblToken.Text = token;


            SemanticScreenReader.Announce(LblToken.Text);
#if DEBUG
            Console.WriteLine("TOKEN: "+token);
#endif
        }
    }

}
