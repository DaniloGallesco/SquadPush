using Plugin.Firebase.CloudMessaging;
using System.Collections.ObjectModel;

namespace SquadPush
{
    public partial class MyPushes : ContentPage
    {
        ObservableCollection<Aaa> employees = new ObservableCollection<Aaa>();
        public ObservableCollection<Aaa> Employees { get { return employees; } }

        public MyPushes()
        {
            InitializeComponent();
            pushes.ItemsSource = Employees;


            var result = Preferences.Default.Get("pushes", "");
            if (result != "")
            {
                var lista2 = System.Text.Json.JsonSerializer.Deserialize<List<FCMNotification>>(result);
                foreach (var item in lista2)
                {
                    employees.Add(new Aaa() { Mensagem = System.Text.Json.JsonSerializer.Serialize(item)});
                }

            }
        }   
    }

    public class Aaa
    {
        public string Mensagem { get; set; }
    }

}
