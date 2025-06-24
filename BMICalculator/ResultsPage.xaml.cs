namespace IMT.Views;

public partial class ResultsPage : ContentPage
{
    public ResultsPage(double bmi, string category, Color color, string recommendation)
    {
        // Загрузка XAML вручную
        this.LoadFromXaml(typeof(ResultsPage));

        BindingContext = new
        {
            Bmi = bmi,
            BmiCategory = category,
            BmiColor = color,
            Recommendation = recommendation
        };
    }

    private void OnBackButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}