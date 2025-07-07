# ImpowerRetro

Privacy-focused team retrospectives made simple. No registration, no data storage, just effective collaboration.

## Features

- **🚀 Zero Setup** - No registration required, just enter a username and start
- **🔐 Privacy First** - Zero data storage, everything is deleted after your session
- **⚡ Real-time Collaboration** - Live updates and easy idea sharing
- **🎯 Customizable Templates** - Pre-set retrospective formats (Default, MSG, Sailboat, 4Ls)
- **🕐 Built-in Timer** - Time-boxed discussions for focused sessions
- **🎭 Hidden Mode** - Unbiased input collection
- **🎉 Engagement Features** - Emoji reactions and special effects
- **📝 Note-taking** - Facilitator notes and action items
- **📊 Export** - Save retrospective as HTML report
- **🎨 Modern UI** - Clean interface with optional background animations
- **♿ Accessible** - High-contrast mode and responsive design

## Quick Start
### Prerequisites

- .NET 8.0 SDK
- Modern web browser

### Running Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/impower-ai/ImpowerRetro.git
   cd ImpowerRetro
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build and run the application**
   ```bash
   dotnet build
   dotnet run
   ```

4. **Open your browser**
   Navigate to `https://localhost:5001` or the URL shown in the terminal

5. **Start using ImpowerRetro**
   - Click "Start New Retrospective" to create a session
   - Share the Session ID with your team
   - Begin your retrospective immediately

## How to Use

### Creating a Session

1. Click "Start New Retrospective"
2. Enter your name and optional topic
3. Choose a retrospective template
4. Share the Session ID with your team, or copy the session link directly

### Joining a Session

**Option 1: Using the Session Link**
1. Click the session link shared by the session creator
2. Enter your name
3. Start contributing to the discussion

**Option 2: Manual Join**
1. Click "Join Existing Retrospective"  
2. Enter your name and the Session ID
3. Start contributing to the discussion

## Retrospective Templates

| Template | Columns | Best For |
|----------|---------|----------|
| **Default** | Good, Bad, Start, Stop | General retrospectives |
| **MSG** | Mad, Sad, Glad | Emotional feedback |
| **Sailboat** | Wind, Anchors, Rocks | Progress and obstacles |
| **4Ls** | Liked, Learned, Lacked, Longed For | Learning-focused |

## Technology Stack

- **Framework**: ASP.NET Core 8.0 with Blazor Server
- **UI Components**: Radzen.Blazor
- **Styling**: CSS with glass morphism design system
- **Serialization**: Newtonsoft.Json
- **Background Effects**: Vanta.js (optional)

## Architecture

```
ImpowerRetro/
├── Components/
│   ├── Controls/          # Reusable UI components
│   ├── Model/             # Data models and ServiceResult
│   ├── Pages/             # Blazor pages
│   └── Utilities/         # Constants and extensions
├── Services/              # Business logic services
│   ├── SessionService.cs  # Session management
│   ├── MemoryService.cs   # Local storage
│   └── LogService.cs      # Console logging
└── wwwroot/               # Static assets and CSS
```

**Simple Design Principles:**
- **No external configuration files** - All settings are compile-time constants
- **Memory-only storage** - No database or file persistence required
- **Service-based architecture** - Clean separation of concerns

## Configuration

### Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Development/Production
- `ASPNETCORE_URLS` - Binding URLs
- `IR_TEMPLATES` - Custom session templates (JSON format, optional)

### Session Templates

**Default Templates:**
Templates are built-in and defined in `Components/Utilities/Constants.cs`. The four available templates (Default, MSG, Sailboat, 4Ls) are designed to cover most retrospective scenarios.

**Custom Templates:**
You can override the default templates by setting the `IR_TEMPLATES` environment variable with JSON:

```bash
# Example: Add a custom "DACI" template
'[
  {
    "Name": "DACI",
    "Columns": ["Driver", "Approver", "Contributors", "Informed"]
  },
  {
    "Name": "Start Stop Continue",
    "Columns": ["Start", "Stop", "Continue"]
  }
]'
```

**Railway Deployment:**
In Railway dashboard, add the environment variable:
- **Key:** `IR_TEMPLATES`
- **Value:** Your JSON template definition

If the environment variable is invalid or empty, the application falls back to the built-in default templates.

## Deployment

### Railway (Recommended)

Deploy ImpowerRetro to Railway with one click:

[![Deploy on Railway](https://railway.com/button.svg)](https://railway.com/deploy/LZBVj9?referralCode=6X4KFq)

The template automatically sets up:
- The ImpowerRetro web application
- All required environment variables
- HTTPS with automatic certificates

### Manual Deployment

```bash
dotnet publish -c Release -o publish
# Copy publish folder to your server
```

## Development

### Code Style

- **Naming**: PascalCase for public members, camelCase with underscore for private fields
- **File Organization**: Partial classes separate UI from logic
- **Error Handling**: ServiceResult pattern with graceful degradation
- **Documentation**: Minimal comments focused on non-obvious aspects

## Privacy & Security Model

ImpowerRetro is designed for **transparent team collaboration**. Here's our privacy approach:

### **Transparency by Design**
- **Visible Contributions** - Team members can see who contributed what for effective follow-up discussions
- **Facilitator Oversight** - Session owners can manage participation and guide conversations
- **Open Collaboration** - Designed for teams that benefit from knowing contributors for accountability

### **Data Protection & Lifecycle**
- **No Data Persistence** - Sessions are stored in memory only and deleted when inactive
- **Automatic Cleanup** - All session data expires and is permanently removed after use
- **Minimal Data Collection** - Only usernames and retrospective content are collected
- **No Registration Required** - No accounts, emails, or personal information stored
- **Client-side Preferences** - User settings stored only in browser localStorage
- **HTTPS Required** - Secure communication in production environments

**Session Data Flow:**
1. **Session Creation** → Data exists only in server memory
2. **Active Session** → Real-time collaboration with visible usernames  
3. **Session End** → Optional HTML export, then permanent deletion
4. **No Retention** → Zero data storage after session completion

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes following the code style guidelines
4. Test your changes
5. Submit a pull request

## Support and Feedback

### Getting Help

If you encounter issues or have questions about ImpowerRetro:

- **Documentation**: This README contains all the information needed to get started
- **Bug Reports & Feature Requests**: Submit through our [GitHub Issues page](https://github.com/impower-ai/ImpowerRetro/issues)
- **Community**: Visit [Impower.AI](https://impower.ai) for more information

### Sample Usage

First-time users can immediately start using ImpowerRetro without any setup - just click "Start New Retrospective" and begin exploring the system's capabilities with your team.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

```
   Copyright 2025 Impower.AI

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
```

### Why Apache 2.0?

The Apache 2.0 license was chosen specifically to:

- Allow free use, modification, and distribution of our code
- Provide patent protection for innovations in retrospective tooling
- Prevent "submarine patenting" of techniques disclosed in this codebase
- Ensure attribution while enabling widespread adoption

This license supports our goal of showcasing code quality while protecting both users and contributors.

## Disclaimer

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

This software is not intended for use in any situation where failure could lead to injury, loss of life, or significant property damage. Users implementing this software in production environments do so at their own risk and are responsible for ensuring appropriate security, reliability, and compliance with all applicable laws and regulations.

---

Made with ❤️ by [Impower.AI](https://https://www.impowerconsulting.ai)
