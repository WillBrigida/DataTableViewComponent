using System.Collections;
namespace Sample.Components;
public partial class DataTableView : Grid
{
    bool scroled = false;
    public enum SeparatorTypeEnum { Default, HorizontalLine, EvenRow };
    public static Color _HeaderTextColor { get; set; } = Color.FromArgb("#444444");
    public static Color _HeaderBackgroundColor { get; set; } = Color.FromArgb("#50CCCCCC");
    public static double _ColumnWidthRequest { get; set; } = 100.0;
    public static double _RowHeightRequest { get; set; } = 35.0;
    public static double HeaderWidh { get; set; }
    public static bool _PinFirstColumn { get; set; } = false;
    public static SeparatorTypeEnum _SeparatorType { get; set; } = 0;

    public DataTableView()
    {
        InitializeComponent();

        //Medida de contorno para resolver o bug da orientação do scroll no Android - Propriedade Both Não funciona
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            var scroll = new ScrollView
            {
                HorizontalOptions = LayoutOptions.Fill,
                Padding = 0,
                Orientation = ScrollOrientation.Both,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always
            };

            scroll.Scrolled += ColunaCompletaHorizontalScrollView_Scrolled;
            scroll.Content = new HorizontalStackLayout();

            var scrollFixo = new ScrollView
            {
                HorizontalOptions = LayoutOptions.Fill,
                Padding = 0,
                Orientation = ScrollOrientation.Vertical,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always
            };

            scrollFixo.Scrolled += ColunaCompletaScrollView_Scrolled;
            scrollFixo.Content = scroll;

            DataTableGrid.Children.Remove(ColunaCompletaScrollView);
            DataTableGrid.Add(scrollFixo, 0, 1);
        }
    }

    private async void ColunaFixaScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (!scroled)
        {
            scroled = true;

            //Medida de contorno para resolver o bug da orientação do scroll no Android - Propriedade Both Não funciona

            if (DeviceInfo.Platform != DevicePlatform.Android)
                await Task.WhenAll(
                ColunaCompletaScrollView.ScrollToAsync(0, e.ScrollY, false),
                LinhasHorizontalScrollView.ScrollToAsync(0, e.ScrollY, false));

            else
            {
                await Task.WhenAll(
                    (DataTableGrid.Children[1] as ScrollView).ScrollToAsync(0, e.ScrollY, false),
                    LinhasHorizontalScrollView.ScrollToAsync(0, e.ScrollY, false));
            }
        }
        scroled = false;
    }

    private async void ColunaCompletaScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        if (!scroled)
        {
            scroled = true;

            //Medida de contorno para resolver o bug da orientação do scroll no Android - Propriedade Both Não funciona
            if (DeviceInfo.Platform != DevicePlatform.Android)
                await Task.WhenAll
                (
                    ColunaFixaScrollView.ScrollToAsync(0, e.ScrollY, false),
                    CabecalhoCompletoScrollView.ScrollToAsync(e.ScrollX, 0, false),
                    LinhasHorizontalScrollView.ScrollToAsync(0, e.ScrollY, false)
                );

            else
                await Task.WhenAll
                (
                    ColunaFixaScrollView.ScrollToAsync(0, e.ScrollY, false),
                    LinhasHorizontalScrollView.ScrollToAsync(0, e.ScrollY, false)
                );
        }
        scroled = false;
    }

    private async void ColunaCompletaHorizontalScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        //Android Only
        //Medida de contorno para resolver o bug da orientação do scroll no Android - Propriedade Both Não funciona

        if (!scroled)
        {
            scroled = true;
            await CabecalhoCompletoScrollView.ScrollToAsync(e.ScrollX, 0, false);
        }
        scroled = false;
    }

    #region _ SeparatorType . . .
    public static readonly BindableProperty SeparatorTypeProperty =
        BindableProperty.Create(nameof(SeparatorType),
            typeof(SeparatorTypeEnum),
            typeof(DataTableView),
            defaultValue: SeparatorTypeEnum.Default,
            defaultBindingMode: BindingMode.TwoWay,
            coerceValue: SeparatorTypeCoerceValue,
            propertyChanged: SeparatorTypePropertyChanged);

    public static object SeparatorTypeCoerceValue(BindableObject bindable, object value) => _SeparatorType = (SeparatorTypeEnum)value;

    static void SeparatorTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.CoerceValue(SeparatorTypeProperty);

    public SeparatorTypeEnum SeparatorType
    {
        get => (SeparatorTypeEnum)GetValue(SeparatorTypeProperty);
        set => SetValue(SeparatorTypeProperty, value);
    }

    #endregion

    #region _ RowHeightRequest . . .
    public static readonly BindableProperty RowHeightRequestProperty =
        BindableProperty.Create(nameof(RowHeightRequest),
            typeof(double),
            typeof(DataTableView),
            defaultValue: 30.0,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: RowHeightRequestCoerceValue,
            propertyChanged: RowHeightRequestPropertyChanged);

    public static object RowHeightRequestCoerceValue(BindableObject bindable, object value) => _RowHeightRequest = (double)value;

    static void RowHeightRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.CoerceValue(RowHeightRequestProperty);

    public double RowHeightRequest
    {
        get => (double)GetValue(RowHeightRequestProperty);
        set => SetValue(RowHeightRequestProperty, value);
    }

    #endregion

    #region _ ColumnWidthRequest . . .
    public static readonly BindableProperty ColumnWidthRequestProperty =
        BindableProperty.Create(nameof(ColumnWidthRequest),
            typeof(double),
            typeof(DataTableView),
            defaultValue: 50.0,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: ColumnWidthRequestCoerceValue,
            propertyChanged: ColumnWidthRequestPropertyChanged);

    private static object ColumnWidthRequestCoerceValue(BindableObject bindable, object value) => _ColumnWidthRequest = (double)value;


    static void ColumnWidthRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.CoerceValue(ColumnWidthRequestProperty);

    public double ColumnWidthRequest
    {
        get => (double)GetValue(ColumnWidthRequestProperty);
        set => SetValue(ColumnWidthRequestProperty, value);
    }
    #endregion

    #region _ HeaderBackgroundColor . . .
    public static readonly BindableProperty HeaderBackgroundColorProperty =
        BindableProperty.Create(nameof(HeaderBackgroundColor),
            typeof(Color),
            typeof(DataTableView),
            defaultValue: Colors.Transparent,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: HeaderBackgroundColorCoerceValue,
            propertyChanged: HeaderBackgroundColorPropertyChanged);


    public static object HeaderBackgroundColorCoerceValue(BindableObject bindable, object value) => _HeaderBackgroundColor = (Color)value;

    static void HeaderBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.CoerceValue(HeaderBackgroundColorProperty);

    public Color HeaderBackgroundColor
    {
        get => (Color)GetValue(HeaderBackgroundColorProperty);
        set => SetValue(HeaderBackgroundColorProperty, value);
    }
    #endregion

    #region _ HeaderTextColor . . .
    public static readonly BindableProperty HeaderTextColorProperty =
        BindableProperty.Create(nameof(HeaderTextColor),
            typeof(Color),
            typeof(DataTableView),
            defaultValue: Colors.Gray,
            defaultBindingMode: BindingMode.OneWay,
            coerceValue: HeaderTextColorCoerceValue,
            propertyChanged: HeaderTextColorPropertyChanged);

    public static object HeaderTextColorCoerceValue(BindableObject bindable, object value) => _HeaderTextColor = (Color)value;

    static void HeaderTextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => bindable.CoerceValue(HeaderTextColorProperty);

    public Color HeaderTextColor
    {
        get => (Color)GetValue(HeaderTextColorProperty);
        set => SetValue(HeaderTextColorProperty, value);
    }
    #endregion

    #region _ PinFirstColumn . . .
    public static readonly BindableProperty PinFirstColumnProperty =
        BindableProperty.Create(nameof(PinFirstColumn),
            typeof(bool),
            typeof(DataTableView),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay,
              coerceValue: PinFirstColumnCoerceValue,
            propertyChanged: PinFirstColumnPropertyChanged);

    public static object PinFirstColumnCoerceValue(BindableObject bindable, object value) => _PinFirstColumn = (bool)value;

    static void PinFirstColumnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
      => bindable.CoerceValue(PinFirstColumnProperty);

    public bool PinFirstColumn
    {
        get => (bool)GetValue(PinFirstColumnProperty);
        set => SetValue(PinFirstColumnProperty, value);
    }

    #endregion

    #region _ PropertiesName . . .
    public static readonly BindableProperty PropertiesNameProperty =
        BindableProperty.Create(nameof(PropertiesName),
            typeof(string[]),
            typeof(DataTableView),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

    public string[] PropertiesName
    {
        get => (string[])GetValue(PropertiesNameProperty);
        set => SetValue(PropertiesNameProperty, value);
    }
    #endregion

    #region _ HeaderTitles . . .
    public static readonly BindableProperty HeaderTitlesProperty =
        BindableProperty.Create(nameof(HeaderTitles),
            typeof(string[]),
            typeof(DataTableView),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

    public string[] HeaderTitles
    {
        get => (string[])GetValue(HeaderTitlesProperty);
        set => SetValue(HeaderTitlesProperty, value);
    }
    #endregion

    #region _ ItemsSource . . .
    public static BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource),
        typeof(IEnumerable),
            typeof(DataTableView),
            default(Nullable),
            defaultBindingMode: BindingMode.OneTime,
    propertyChanged: ItemsSourcePropertyChanged);



    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Type t = newValue.GetType().GetGenericArguments()[0];
        var properties = t.GetProperties().Select(x => x.Name).ToArray();

        var control = bindable as DataTableView;

        var propertiesName = (control.PropertiesName?.Length ?? 0) > 0
          ? control.PropertiesName
          : properties;

        var headerTitles = ((control.HeaderTitles?.Length ?? 0) == (propertiesName?.Length ?? 0) && (propertiesName?.Length ?? 0) != 0)
          ? control.HeaderTitles.ToList()
          : properties.ToList();

        HeaderWidh = _RowHeightRequest + 15.0;

        var itemsSource = (IEnumerable)newValue;

        control.CabecalhoCompletoScrollView.BackgroundColor = _HeaderBackgroundColor;

        HandleCollection(control, headerTitles, propertiesName, itemsSource);
    }

    private static void HandleCollection(DataTableView control, List<string> headerTitles, string[] propertiesName, IEnumerable itemsSource)
    {
        List<string> newHeaderTitles = new();
        newHeaderTitles.AddRange(headerTitles);

        if (_PinFirstColumn)
        {
            (control.CabecalhoFixoScrollView.Parent as Grid).WidthRequest = HeaderWidh;

            //Cabecalho fixo
            newHeaderTitles.RemoveAt(0);
            DataTemplate fistTitleTemplate = new(() => CreateCollection(".", 0, true));
            BindableLayout.SetItemsSource(control.CabecalhoFixoHorizontalStackLayout, new string[] { headerTitles[0] });
            BindableLayout.SetItemTemplate(control.CabecalhoFixoHorizontalStackLayout, fistTitleTemplate);

            //Coluna fixa
            VerticalStackLayout vsl0 = new();
            DataTemplate colunaFixaTemplate = new(() => CreateCollection(propertiesName[0]));
            BindableLayout.SetItemsSource(vsl0, itemsSource);
            BindableLayout.SetItemTemplate(vsl0, colunaFixaTemplate);
            control.ColunaFixaHorizontalStackLayout.Add(vsl0);
        }

        int index0 = 0;
        //Cabecalho completo
        foreach (var item in newHeaderTitles)
        {
            DataTemplate fullTitleTemplate = new(() => CreateCollection(item, index0, true));
            BindableLayout.SetItemsSource(control.CabecalhoCompletoHorizontalStackLayout, newHeaderTitles);
            BindableLayout.SetItemTemplate(control.CabecalhoCompletoHorizontalStackLayout, fullTitleTemplate);

            index0++;
        }


        //coluna Completa
        int index = 0;
        foreach (var item in propertiesName)
        {
            VerticalStackLayout vsl = new();
            DataTemplate colunaCompletaTemplate = new(() => CreateCollection(item, index));
            BindableLayout.SetItemsSource(vsl, itemsSource);
            BindableLayout.SetItemTemplate(vsl, colunaCompletaTemplate);
            if (_PinFirstColumn && index == 0) vsl.IsVisible = false;

            //Medida de contorno para resolver o bug da orientação do scroll no Android - Propriedade Both Não funciona
            if (DeviceInfo.Platform != DevicePlatform.Android)
                control.ColunaCompletaHorizontalStackLayout.Add(vsl);

            else
                (((control.DataTableGrid
                    .Children[1] as ScrollView)
                    .Children[0] as ScrollView)
                    .Children[0] as HorizontalStackLayout)
                    .Add(vsl);

            index++;
        }

        //Linhas Horizontal
        if (_SeparatorType != SeparatorTypeEnum.Default)
        {
            VerticalStackLayout vs2 = new();
            DataTemplate linhaemplate = new(() => CreateLines(propertiesName));
            BindableLayout.SetItemsSource(vs2, itemsSource);
            BindableLayout.SetItemTemplate(vs2, linhaemplate);
            control.LinhasHorizontalVerticalStackLayout.Margin = new Thickness(0, HeaderWidh, 0, 0);
            control.LinhasHorizontalVerticalStackLayout.Add(vs2);
        }
    }

    private static Grid CreateCollection(string propertiesName = null, int index = 0, bool isHeader = false)
    {
        Grid collectionTemplate = new();
        //var tapGestureRecognizer = new TapGestureRecognizer();
        //tapGestureRecognizer.Tapped += (s, e) =>
        //{
        //    // handle the tap
        //    e.
        //};
        //collectionTemplate.GestureRecognizers.Add(tapGestureRecognizer);

        Label description = new()
        {
            WidthRequest = index == 0 ? HeaderWidh : _ColumnWidthRequest,
            HeightRequest = isHeader ? HeaderWidh : _RowHeightRequest,
            Background = isHeader ? _HeaderBackgroundColor : Colors.Transparent,
            TextColor = isHeader ? _HeaderTextColor : Color.FromArgb("#444444"),
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
        };

        description.SetBinding(Label.TextProperty, isHeader ? "." : propertiesName);

        collectionTemplate.Add(description);

        return collectionTemplate;
    }

    private static Grid CreateLines(string[] propertiesName)
    {
        Grid collectionTemplate = new();

        Label description = new()
        {
            WidthRequest = _ColumnWidthRequest,
            HeightRequest = _RowHeightRequest,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
        };
        var hexColor = _HeaderBackgroundColor.ToArgbHex();
        var rowColor = "#40" + hexColor.Substring(1, hexColor.Length - 1);

        BoxView line = new()
        {
            HorizontalOptions = LayoutOptions.Fill,
            WidthRequest = _PinFirstColumn ? (_ColumnWidthRequest * propertiesName.Count()) - (_ColumnWidthRequest - HeaderWidh) : _ColumnWidthRequest * propertiesName.Count(),
            VerticalOptions = LayoutOptions.End,
            HeightRequest = _SeparatorType == SeparatorTypeEnum.EvenRow ? _RowHeightRequest : 1,

            Color = Color.FromArgb("#50CCCCCC")
        };

        if (_SeparatorType == SeparatorTypeEnum.EvenRow)
        {
            line.Color = Color.FromArgb(rowColor);
            DataTrigger dataTrigger = new DataTrigger(typeof(BoxView));
            Binding dataBinding = new Binding() { Path = "ColorRow" };
            dataTrigger.Binding = dataBinding;
            dataTrigger.Value = true;

            Setter dataSetter = new Setter();
            dataSetter.Property = BoxView.ColorProperty;
            dataSetter.Value = Colors.Transparent;//.FromArgb("#15CCCCCC");

            dataTrigger.Setters.Add(dataSetter);
            line.Triggers.Add(dataTrigger);
        }

        collectionTemplate.Add(description);
        collectionTemplate.Add(line);
        return collectionTemplate;
    }
}

