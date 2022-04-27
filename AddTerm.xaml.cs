using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTerm : ContentPage
    {
        public MainPage _mainPage;
        private readonly SQLiteAsyncConnection _conn;
        

        public AddTerm(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            
        }
        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

            var term = new Term();
            term.Title = TermTitle.Text;
            term.StartDate = StartDate.Date;
            term.EndDate = EndDate.Date;
          
            if (FieldCheck.IsNull(term.Title))
            {
                if (term.StartDate < term.EndDate)
                {
                    await _conn.InsertAsync(term);
                    _mainPage.termList.Add(term);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error.", "Start date must come before end date.", "Ok");
            }
            else await DisplayAlert("Error.", "All fields should be populated.", "Ok");
        }

    }
}