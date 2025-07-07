using ImpowerRetro.Components.Utilities;
using Microsoft.JSInterop;

namespace ImpowerRetro.Services;

/// <summary>
/// Service implementation for JavaScript utility functions through JSInterop
/// </summary>
public class JSUtilityService : IJSUtilityService
{
    private readonly IJSRuntime _jsRuntime;
    
    /// <summary>
    /// Creates a new instance of JSUtilityService
    /// </summary>
    /// <param name="jsRuntime">The JSInterop runtime</param>
    public JSUtilityService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    /// <summary>
    /// Copies data to the system clipboard
    /// </summary>
    /// <param name="data">The data to copy to clipboard</param>
    public async Task CopyToClipboard(object data)
    {
        await _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", data);
    }
    
    /// <summary>
    /// Creates and downloads an HTML file with the specified content
    /// </summary>
    /// <param name="fileName">Name of the file to download</param>
    /// <param name="content">HTML content for the file</param>
    public async Task DownloadHtmlFile(string fileName, string content)
    {
        await _jsRuntime.InvokeVoidAsync("eval", $$"""
                                          var blob = new Blob([`{{content}}`], { type: 'text/html' });
                                          var url = URL.createObjectURL(blob);
                                          var link = document.createElement('a');
                                          link.href = url;
                                          link.download = '{{fileName}}.html';
                                          document.body.appendChild(link);
                                          link.click();
                                          document.body.removeChild(link);
                                          URL.revokeObjectURL(url);
                                      """);
    }
    
    /// <summary>
    /// Initializes the Vanta.js background effects
    /// </summary>
    public async Task EnableVantaBackground()
    {
        await _jsRuntime.InvokeVoidAsync(Constants.JavaScript.InitVantaBackground, Constants.JavaScript.VantaElementId, new
        {
            mouseControls = false,
            touchControls = false,
            gyroControls = false
        });
    }
    
    /// <summary>
    /// Destroys the Vanta.js background effects to free resources
    /// </summary>
    public async Task DisableVantaBackground()
    {
        await _jsRuntime.InvokeVoidAsync(Constants.JavaScript.DestroyVantaBackground);
    }
    
    /// <summary>
    /// Gets a value from localStorage by key
    /// </summary>
    /// <param name="key">The localStorage key</param>
    /// <returns>The stored value or null if not found</returns>
    public async Task<string> GetItem(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }
    
    /// <summary>
    /// Sets a value in localStorage
    /// </summary>
    /// <param name="key">The localStorage key</param>
    /// <param name="value">The value to store</param>
    public async Task SetItem(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value ?? "");
    }
    
    /// <summary>
    /// Removes a value from localStorage
    /// </summary>
    /// <param name="key">The localStorage key to remove</param>
    public async Task DeleteItem(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
    
    /// <summary>
    /// Triggers star confetti animation
    /// </summary>
    public async Task PlayStarsAnimation()
    {
        await _jsRuntime.InvokeVoidAsync(Constants.JavaScript.ConfettiStars);
    }
    
    /// <summary>
    /// Triggers fireworks confetti animation
    /// </summary>
    public async Task PlayConfettiAnimation()
    {
        await _jsRuntime.InvokeVoidAsync(Constants.JavaScript.ConfettiFireworks);
    }
    
    /// <summary>
    /// Scrolls an element into view
    /// </summary>
    /// <param name="element">The element to scroll into view</param>
    public async Task ScrollElementIntoView(IJSObjectReference element)
    {
        await _jsRuntime.InvokeVoidAsync("scrollElementIntoView", element);
    }
}