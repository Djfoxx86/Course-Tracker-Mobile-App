using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTerm : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly Term _term;
        public EditTerm(Term currentTerm)
        {
            InitializeComponent();
            _term = currentTerm;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
        }
        protected override async void OnAppearing()
        {
            await _conn.CreateTableAsync<Term>();
            TermTitle.Text = _term.Title;
            StartDate.Date = _term.StartDate;
            EndDate.Date = _term.EndDate;
            base.OnAppearing();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            _term.Title = TermTitle.Text;
            _term.StartDate = StartDate.Date;
            _term.EndDate = EndDate.Date;

            if (FieldCheck.IsNull(_term.Title))
            {
                if (_term.StartDate < _term.EndDate)
                {

                    await _conn.UpdateAsync(_term);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error.", "Start date must come before end date", "Ok");
            }
            else await DisplayAlert("Error.", "All fields should be populated.", "Ok");
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }


}
