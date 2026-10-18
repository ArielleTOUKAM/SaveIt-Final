using System.Collections.ObjectModel;
using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class MatchesViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly Action<string, object?> _navigateAction;

    private Item? _sourceItem;
    private bool _isEmpty;

    public MatchesViewModel(IItemService itemService, Action<string, object?> navigateAction)
    {
        _itemService = itemService;
        _navigateAction = navigateAction;

        Matches = [];

        ViewDetailsCommand = new Command<MatchResult>((match) =>
        {
            if (match != null)
                _navigateAction("Details", match.CandidateItem);
        });

        ContactCommand = new Command<MatchResult>((match) =>
        {
            if (match != null)
            {
                var otherPerson = match.CandidateItem.ReporterName;
                _navigateAction("Conversation", (otherPerson, match.CandidateItem.Id, match.CandidateItem.Title));
            }
        });
    }

    public ObservableCollection<MatchResult> Matches { get; }

    public Item? SourceItem
    {
        get => _sourceItem;
        set => SetProperty(ref _sourceItem, value);
    }

    public bool IsEmpty
    {
        get => _isEmpty;
        set => SetProperty(ref _isEmpty, value);
    }

    public ICommand ViewDetailsCommand { get; }
    public ICommand ContactCommand { get; }

    public async Task LoadMatchesAsync(Item? specificItem = null)
    {
        IsBusy = true;
        SourceItem = specificItem;
        Matches.Clear();

        IReadOnlyList<MatchResult> results;
        if (specificItem != null)
        {
            results = await _itemService.FindMatchesAsync(specificItem);
        }
        else
        {
            results = await _itemService.GetAllMatchesAsync();
        }

        foreach (var m in results)
        {
            Matches.Add(m);
        }

        IsEmpty = Matches.Count == 0;
        IsBusy = false;
    }
}
