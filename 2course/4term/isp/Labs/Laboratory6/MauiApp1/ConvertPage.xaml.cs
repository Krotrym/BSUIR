using Laboratornay6.Entities;
using Laboratornay6.Services;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Laboratornay6;

public partial class ConvertPage : ContentPage
{
    private readonly IRateService _rs;

    List<Country> country = new List<Country>();
    public ConvertPage(IRateService rs)
	{
        _rs = rs;
		InitializeComponent();
        country.Add(new Country("Российский рубль:","Российских рублей"));
        country.Add(new Country("Евро:", "Евро"));
        country.Add(new Country("ДоларСША:", "Доллар США"));
        country.Add(new Country("Швейцарский франк:", "Швейцарский франк"));
        country.Add(new Country("Китайский юань:", "Китайских юаней"));
        country.Add(new Country("Фунт стерлинга:", "Фунт стерлингов"));

    }
    private async void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        if (picker.Items != null)
            picker.Items.Clear();

        DateTime selectedDate = (DateTime)e.NewDate;
        var result = await _rs.GetRates(selectedDate);
        var list = result.ToList();

        for (int i = 0; i < country.Count; i++) 
        {
            country[i].Value = (decimal)(result.First(g => g.Cur_Name == country[i].Name_ID).Cur_OfficialRate / result.First(g => g.Cur_Name == country[i].Name_ID).Cur_Scale);
            picker.Items.Add(country[i].Name);
        }
        LessonsList.ItemsSource = null;
        LessonsList.ItemsSource = country;
    }

    public decimal SendValue()
    {
        var rrr = picker.SelectedItem.ToString();
        var selectedCounty = country.Find(g => g.Name == rrr);
        return selectedCounty.Value;
    }

    public void CompletedText(object sender, EventArgs e)
    {
        decimal belRubles = decimal.Parse(entryBel.Text);
        decimal convertValue = belRubles / SendValue();
        entryValue.Text = convertValue.ToString();
    }

    public void CompletedText1(object sender, EventArgs e)
    {
        decimal Value = decimal.Parse(entryValue.Text);
        decimal convertValue = Value * SendValue();
        entryBel.Text = convertValue.ToString();
    }
}