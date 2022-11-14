using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Sample;

public partial class MainPageViewModel : ObservableObject
{
    public ObservableCollection<Models.DataTable> Items { get; set; } = new();

    public MainPageViewModel()
    {
        
    }

    private static async IAsyncEnumerable<Models.DataTable> GetData(bool fullLoad = false)
    {
        for (var i = 1; i <= 50; i++)
        {
            await Task.Delay(40);

            yield return new Models.DataTable(i)
            {
                Id = i,
                Column1 = $"Col 1 | Row {i}",
                Column2 = $"Col 2 | Row {i}",
                Column3 = $"Col 3 | Row {i}",
                Column4 = $"Col 4 | Row {i}",
                Column5 = $"Col 5 | Row {i}",
                Column6 = $"Col 6 | Row {i}",
                Column7 = $"Col 7 | Row {i}",
                Column8 = $"Col 8 | Row {i}",
                Column9 = $"Col 9 | Row {i}",
            };
        }
    }

    internal async Task LoadItems()
    {
        //var lst = new List<Models.DataTable>();

        //await foreach (var item in GetData())
        //    Items.Add(item);

        //Items.AddRange(lst);

        for (var i = 1; i <= 150; i++)
        {
            Items.Add(new Models.DataTable(i)
            {
                Id = i,
                Column1 = $"Col 1 | Row {i}",
                Column2 = $"Col 2 | Row {i}",
                Column3 = $"Col 3 | Row {i}",
                Column4 = $"Col 4 | Row {i}",
                Column5 = $"Col 5 | Row {i}",
                Column6 = $"Col 6 | Row {i}",
                Column7 = $"Col 7 | Row {i}",
                Column8 = $"Col 8 | Row {i}",
                Column9 = $"Col 9 | Row {i}",
            }) ;
        }
    }
}

public class Models
{
    //public record struct DataTable(int Id, string Column1, string Column2, string Column3, string Column4, string Column5, string Column6, string Column7, string Column8, string Column9, bool ColorRow) : DataTablePlus;
    //public record struct DataTablePlus(bool ColorRow);


    public class DataTable : DataTablePlus
    {
        public DataTable(int index) : base(index)
        {

        }

        public int Id { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string Column9 { get; set; }
    }

    public class DataTablePlus
    {
        public DataTablePlus(int index)
        {
            ColorRow = GetColorRow(index);
        }
        public bool ColorRow { get; set; }

        private bool GetColorRow(int idx) => idx % 2 == 0;
    }
}

