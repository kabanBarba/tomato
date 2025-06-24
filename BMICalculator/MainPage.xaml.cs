namespace IMT.Views;

public partial class MainPage : ContentPage
{
    private string selectedGender = "Мужской";

    public MainPage()
    {
        InitializeComponent();
        MaleButton.BackgroundColor = Colors.LightBlue;
    }

    private void OnGenderSelected(object sender, EventArgs e)
    {
        var button = (Button)sender;
        selectedGender = button.Text;

        MaleButton.BackgroundColor = (button == MaleButton) ? Colors.LightBlue : Colors.Transparent;
        FemaleButton.BackgroundColor = (button == FemaleButton) ? Colors.LightPink : Colors.Transparent;
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        if (double.TryParse(WeightEntry.Text, out double weight) &&
            double.TryParse(HeightEntry.Text, out double height) &&
            height > 0 && weight > 0)
        {
            double bmi = Math.Round(weight / (height * height), 1);
            var (category, color) = GetBmiCategory(bmi);
            string recommendation = GetRecommendation(bmi, selectedGender);

            Navigation.PushAsync(new ResultsPage(bmi, category, color, recommendation));
        }
        else
        {
            DisplayAlert("Ошибка", "Пожалуйста, введите корректные значения веса и роста", "OK");
        }
    }

    private (string category, Color color) GetBmiCategory(double bmi)
    {
        return bmi switch
        {
            < 18.5 => ("Недостаточный вес", Colors.LightBlue),
            >= 18.5 and < 25 => ("Нормальный вес", Colors.Green),
            >= 25 and < 30 => ("Избыточный вес", Colors.Yellow),
            >= 30 and < 35 => ("Ожирение I степени", Colors.Orange),
            >= 35 and < 40 => ("Ожирение II степени", Colors.OrangeRed),
            _ => ("Ожирение III степени", Colors.Red)
        };
    }

    private string GetRecommendation(double bmi, string gender)
    {
        string genderText = gender == "Мужской" ? "мужчинам" : "женщинам";

        return bmi switch
        {
            < 18.5 => $"Для {genderText} с недостаточным весом рекомендуется...",
            >= 18.5 and < 25 => $"Для {genderText} с нормальным весом рекомендуется...",
            >= 25 and < 30 => $"Для {genderText} с избыточным весом рекомендуется...",
            _ => $"Для {genderText} с ожирением рекомендуется..."
        };
    }
}