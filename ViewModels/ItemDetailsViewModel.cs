using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class ItemDetailsViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly Action<string, object?> _navigateAction;
    private readonly Action _goBackAction;

    private Item? _item;
    private string _originScreen = "Home";
    private int _matchesCount;

    public ItemDetailsViewModel(IItemService itemService, Action<string, object?> navigateAction, Action goBackAction)
    {
        _itemService = itemService;
        _navigateAction = navigateAction;
        _goBackAction = goBackAction;

        GoBackCommand = new Command(() => _goBackAction());
        ViewMatchesCommand = new Command(() =>
        {
            if (Item != null)
                _navigateAction("Matches", Item);
        });
        ContactSecurelyCommand = new Command(() =>
        {
            if (Item != null)
            {
                var otherPerson = Item.ReporterName.Contains("Arielle") ? "Jean D." : Item.ReporterName;
                _navigateAction("Conversation", (otherPerson, Item.Id, Item.Title));
            }
        });
    }

    public Item? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public string OriginScreen
    {
        get => _originScreen;
        set => SetProperty(ref _originScreen, value);
    }

    public int MatchesCount
    {
        get => _matchesCount;
        set => SetProperty(ref _matchesCount, value);
    }

    public ICommand GoBackCommand { get; }
    public ICommand ViewMatchesCommand { get; }
    public ICommand ContactSecurelyCommand { get; }

    public async Task SetItemAsync(Item item, string originScreen)
    {
        Item = item;
        OriginScreen = originScreen;
        var matches = await _itemService.FindMatchesAsync(item);
        MatchesCount = matches.Count;
    }
}
