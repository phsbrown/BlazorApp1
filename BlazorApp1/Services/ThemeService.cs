using Microsoft.JSInterop;

namespace BlazorApp1.Services;

public class ThemeService(IJSRuntime jsRuntime)
{
    private const string ThemeKey = "app-theme";
    private readonly IJSRuntime _jsRuntime = jsRuntime;
    public bool IsDarkMode { get; private set; }

    public event Action? OnThemeChanged;

    public async Task InitializeAsync()
    {
        var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", ThemeKey);
        IsDarkMode = theme == "dark";
        await ApplyThemeAsync();
    }

    public async Task ToggleThemeAsync()
    {
        IsDarkMode = !IsDarkMode;
        await SaveThemeAsync();
        OnThemeChanged?.Invoke();
    }

    public async Task SetThemeAsync(bool isDark)
    {
        IsDarkMode = isDark;
        await SaveThemeAsync();
        OnThemeChanged?.Invoke();
    }

    private async Task SaveThemeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", ThemeKey, IsDarkMode ? "dark" : "light");
        await ApplyThemeAsync();
    }

    private async Task ApplyThemeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "data-bs-theme", IsDarkMode ? "dark" : "light");
    }
}
