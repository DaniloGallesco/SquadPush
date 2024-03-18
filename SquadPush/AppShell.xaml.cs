namespace SquadPush
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CatPage), typeof(CatPage));
            Routing.RegisterRoute(nameof(DogPage), typeof(DogPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(MyPushes), typeof(MyPushes));
        }
    }
}
