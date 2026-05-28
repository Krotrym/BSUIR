using Laboratornay5.Services;

namespace Laboratornay5;

public partial class NewPage1 : ContentPage
{

    private readonly IDbService _db;
    public NewPage1(IDbService db)
    {
        _db = db;
        InitializeComponent();
    }
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        LoadCars();
    }

    private void LoadCars()
    {
        _db.Init();
        MyPicker.ItemsSource = _db.GetAllCars().ToList();
        MyPicker.ItemDisplayBinding = new Binding("Name");
    }
    public void LoadElements(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedName = picker.SelectedItem?.ToString();

        var selectedCarBrand = (CarBrand)picker.SelectedItem;
        var elements = _db.GetCarsMembers(selectedCarBrand.Id);

        LessonsList.ItemsSource = elements;
    }
}
