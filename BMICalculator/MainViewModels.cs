using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IMT.Views;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace IMT.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private double weight; // Используйте lowercase для полей

        [ObservableProperty]
        private double height;

        [ObservableProperty]
        private string selectedGender = "Male";

        public ObservableCollection<BmiResult> History { get; } = new();

        [RelayCommand]
        private void SelectGender(string gender)
        {
            SelectedGender = gender;
        }

        [RelayCommand]
        private async Task Calculate()
        {
            if (weight <= 0 || height <= 0) // Используйте lowercase для приватных полей
            {
                await Shell.Current.DisplayAlert("Ошибка", "Введите корректные данные", "OK");
                return;
            }

            var bmi = weight / (height * height);
            var category = GetBmiCategory(bmi);
            var color = GetBmiColor(bmi);
            var recommendation = GetRecommendation(bmi, selectedGender);

            await Shell.Current.GoToAsync(nameof(ResultsPage), new Dictionary<string, object>
            {
                ["Bmi"] = bmi,
                ["BmiCategory"] = category,
                ["BmiColor"] = color,
                ["Recommendation"] = recommendation
            });
        }

        private string GetBmiCategory(double bmi)
        {
            return bmi switch
            {
                < 18.5 => "Недостаточный вес",
                >= 18.5 and < 25 => "Нормальный вес",
                >= 25 and < 30 => "Избыточный вес",
                >= 30 and < 35 => "Ожирение I степени",
                >= 35 and < 40 => "Ожирение II степени",
                _ => "Ожирение III степени"
            };
        }

        private Color GetBmiColor(double bmi)
        {
            return bmi switch
            {
                < 18.5 => Colors.LightBlue,
                >= 18.5 and < 25 => Colors.Green,
                >= 25 and < 30 => Colors.Yellow,
                >= 30 and < 35 => Colors.Orange,
                _ => Colors.Red
            };
        }

        private string GetRecommendation(double bmi, string gender)
        {
            return (bmi, gender) switch
            {
                ( < 18.5, "Male") => "Рекомендации для мужчин с недостаточным весом...",
                ( < 18.5, _) => "Рекомендации для женщин с недостаточным весом...",
                ( >= 18.5 and < 25, "Male") => "Рекомендации для мужчин с нормальным весом...",
                ( >= 18.5 and < 25, _) => "Рекомендации для женщин с нормальным весом...",
                ( >= 25 and < 30, "Male") => "Рекомендации для мужчин с избыточным весом...",
                ( >= 25 and < 30, _) => "Рекомендации для женщин с избыточным весом...",
                ( >= 30 and < 35, _) => "Рекомендации при ожирении I степени...",
                ( >= 35 and < 40, _) => "Рекомендации при ожирении II степени...",
                _ => "Рекомендации при ожирении III степени..."
            };
        }
    }

    public class BmiResult
    {
        public DateTime Date { get; set; } = DateTime.Now;
        public required double Value { get; set; }
        public required string Category { get; set; }
        public required double Weight { get; set; }
        public required double Height { get; set; }
        public required string Gender { get; set; }
    }
}