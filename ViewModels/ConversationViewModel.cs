using System.Collections.ObjectModel;
using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class ConversationViewModel : BaseViewModel
{
    private readonly IChatService _chatService;
    private readonly Action _goBackAction;

    private Conversation? _currentConversation;
    private string _otherPartyName = "Jean D.";
    private string _relatedItemTitle = "Black Leather Wallet";
    private string _messageInput = string.Empty;
    private string _originScreen = "Home";

    public ConversationViewModel(IChatService chatService, Action goBackAction)
    {
        _chatService = chatService;
        _goBackAction = goBackAction;

        Messages = [];

        SendMessageCommand = new Command(async () => await ExecuteSendMessageAsync());
        GoBackCommand = new Command(() => _goBackAction());
    }

    public ObservableCollection<ChatMessage> Messages { get; }

    public Conversation? CurrentConversation
    {
        get => _currentConversation;
        set => SetProperty(ref _currentConversation, value);
    }

    public string OtherPartyName
    {
        get => _otherPartyName;
        set => SetProperty(ref _otherPartyName, value);
    }

    public string RelatedItemTitle
    {
        get => _relatedItemTitle;
        set => SetProperty(ref _relatedItemTitle, value);
    }

    public string MessageInput
    {
        get => _messageInput;
        set => SetProperty(ref _messageInput, value);
    }

    public string OriginScreen
    {
        get => _originScreen;
        set => SetProperty(ref _originScreen, value);
    }

    public ICommand SendMessageCommand { get; }
    public ICommand GoBackCommand { get; }

    public async Task InitializeConversationAsync(string otherPartyName, string relatedItemId, string relatedItemTitle, string originScreen)
    {
        OtherPartyName = otherPartyName;
        RelatedItemTitle = relatedItemTitle;
        OriginScreen = originScreen;

        IsBusy = true;
        CurrentConversation = await _chatService.GetOrCreateConversationAsync(otherPartyName, relatedItemId, relatedItemTitle);
        
        Messages.Clear();
        foreach (var msg in CurrentConversation.Messages)
        {
            Messages.Add(msg);
        }
        IsBusy = false;
    }

    private async Task ExecuteSendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(MessageInput) || CurrentConversation == null)
            return;

        var text = MessageInput.Trim();
        MessageInput = string.Empty;

        var newMsg = await _chatService.SendMessageAsync(CurrentConversation.Id, text);
        Messages.Add(newMsg);
    }
}
