using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        private readonly SQLiteAsyncConnection _conn;
        private ObservableCollection<Course> courseList;
        private readonly Term currentTerm;
        public TermPage(Term term)
        {
            InitializeComponent();
            Title = term.Title;
            currentTerm = term;
            _conn = DependencyService.Get<ISQLiteDb>().GetConnection();
            courseListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Course_Tapped);
        }
        protected override async void OnAppearing()
        {
            termTitle.Text = currentTerm.Title;
            TermDetailStart.Text = currentTerm.StartDate.ToString("MM/dd/yy");
            TermDetailEnd.Text = currentTerm.EndDate.ToString("MM/dd/yy");
            await _conn.CreateTableAsync<Course>();
            List<Course> courseList = await _conn.QueryAsync<Course>($"Select * From Courses Where Term = '{currentTerm.Id}'");
            this.courseList = new ObservableCollection<Course>(courseList);
            courseListView.ItemsSource = this.courseList;
            base.OnAppearing();
        }
        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Course_Tapped(object sender, ItemTappedEventArgs e)
        {
            Course course = (Course)e.Item;
            await Navigation.PushModalAsync(new CoursePage(course));
        }

        private async void Add_Course_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddCourse(currentTerm));
        }

        private async void Edit_Term_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new EditTerm(currentTerm));
        }

        private async void OnCancel(object cancel, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Remove_Term_Click(object sender, EventArgs e)
        {
            bool confirmation = await DisplayAlert("Alert", "This term and all courses associated with it will be removed from the course tracker. Are you sure you want to remove this term?", "Yes", "No");
            if (confirmation)
            {
                await _conn.DeleteAsync(currentTerm);
                await Navigation.PopModalAsync();
            }
        }
    }
}