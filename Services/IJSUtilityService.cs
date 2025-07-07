using Microsoft.JSInterop;

namespace ImpowerRetro.Services;

/// <summary>
/// Service interface for JavaScript utility functions through JSInterop
/// </summary>
public interface IJSUtilityService
{
    /// <summary>
    /// Copies data to the system clipboard
    /// </summary>
    /// <param name="data">The data to copy to clipboard</param>
    Task CopyToClipboard(object data);
    
    /// <summary>
    /// Creates and downloads an HTML file with the specified content
    /// </summary>
    /// <param name="fileName">Name of the file to download</param>
    /// <param name="content">HTML content for the file</param>
    Task DownloadHtmlFile(string fileName, string content);
    
    /// <summary>
    /// Initializes the Vanta.js background effects
    /// </summary>
    Task EnableVantaBackground();
    
    /// <summary>
    /// Destroys the Vanta.js background effects to free resources
    /// </summary>
    Task DisableVantaBackground();
    
    /// <summary>
    /// Gets a value from localStorage by key
    /// </summary>
    /// <param name="key">The localStorage key</param>
    /// <returns>The stored value or null if not found</returns>
    Task<string> GetItem(string key);
    
    /// <summary>
    /// Sets a value in localStorage
    /// </summary>
    /// <param name="key">The localStorage key</param>
    /// <param name="value">The value to store</param>
    Task SetItem(string key, string value);
    
    /// <summary>
    /// Removes a value from localStorage
    /// </summary>
    /// <param name="key">The localStorage key to remove</param>
    Task DeleteItem(string key);
    
    /// <summary>
    /// Triggers star confetti animation
    /// </summary>
    Task PlayStarsAnimation();
    
    /// <summary>
    /// Triggers fireworks confetti animation
    /// </summary>
    Task PlayConfettiAnimation();
    
    /// <summary>
    /// Scrolls an element into view
    /// </summary>
    /// <param name="element">The element to scroll into view</param>
    Task ScrollElementIntoView(IJSObjectReference element);
}